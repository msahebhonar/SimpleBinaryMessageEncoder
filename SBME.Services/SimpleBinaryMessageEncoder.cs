using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SBME.Common;
using SBME.Entities.Interfaces;
using SBME.Services.Handlers;
using SBME.Services.Interfaces;

namespace SBME.Services;

public class SimpleBinaryMessageEncoder : ISimpleBinaryMessageEncoder
{
    private readonly IMessageBuilder _messageBuilder;
    private readonly IHeaderCollectionBuilder _headerCollectionBuilder;
    private readonly AppSettings _appSettings;
    private readonly ILogger<SimpleBinaryMessageEncoder> _logger;

    public SimpleBinaryMessageEncoder(
        IMessageBuilder messageBuilder,
        IHeaderCollectionBuilder headerCollectionBuilder,
        IOptions<AppSettings> appSettings,
        ILogger<SimpleBinaryMessageEncoder> logger)
    {
        _messageBuilder = messageBuilder;
        _headerCollectionBuilder = headerCollectionBuilder;
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    public byte[] Encode(IMessage message)
    {
        try
        {
            var encodingHandler = new EncodingHandler(_logger);
            
            // Encode headers
            var headerBytes = encodingHandler.EncodeHeaders(message.Headers.ReadOnlyDictionary);

            // Create message buffer
            var messageBuffer = new byte[headerBytes.Length + message.Payload.Length];

            // Copy headers and payload to the message buffer
            Array.Copy(headerBytes, 0, messageBuffer, 0, headerBytes.Length);
            Array.Copy(message.Payload, 0, messageBuffer, headerBytes.Length, message.Payload.Length);

            return messageBuffer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(Encode), ex.Message));
            throw;
        }
    }

    public IMessage Decode(byte[] data)
    {
        try
        {
            var decodeHandler = new DecodingHandler(_logger, _appSettings);
            
            // Decode headers
            var headers = decodeHandler.DecodeHeaders(data, out var headerBytesLength);

            // Extract payload from the message
            var payload = decodeHandler.DecodePayload(data, headerBytesLength);

            Array.Copy(data, headerBytesLength, payload, 0, payload.Length);

            var headerCollection = _headerCollectionBuilder.Create(headers);
            return _messageBuilder.Create(headerCollection, payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(Decode), ex.Message));
            throw;
        }
    }
}