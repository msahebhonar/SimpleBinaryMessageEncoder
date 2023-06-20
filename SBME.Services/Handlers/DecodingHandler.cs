using System.Text;
using Microsoft.Extensions.Logging;
using SBME.Common;

namespace SBME.Services.Handlers;

public class DecodingHandler
{
    private readonly ILogger<SimpleBinaryMessageEncoder> _logger;
    private readonly AppSettings _appSettings;
    
    public DecodingHandler(ILogger<SimpleBinaryMessageEncoder> logger, AppSettings appSettings)
    {
        _logger = logger;
        _appSettings = appSettings;
    }
    
    public Dictionary<string, string> DecodeHeaders(byte[] message, out int headerBytesLength)
    {
        try
        {
            var index = 0;
            int headerCount = message[index++];

            // Ensure the header count limit
            if (headerCount > _appSettings.MaxNumOfHeaders)
                throw new ArgumentException(ErrorMessages.ExceededHeaderCount);

            var headers = new Dictionary<string, string>();
            for (var i = 0; i < headerCount; i++)
            {
                // Decode header name
                var name = DecodeHeaderEntry(message, ref index);

                // Decode header value
                var value = DecodeHeaderEntry(message, ref index);

                headers.Add(name, value);
            }

            headerBytesLength = index;
            return headers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(DecodeHeaders), ex.Message));
            throw;
        }
    }

    public byte[] DecodePayload(IReadOnlyCollection<byte> data, int headerBytesLength)
    {
        try
        {
            var payload = new byte[data.Count - headerBytesLength];
            if (payload.Length > _appSettings.MaxPayloadSize)
                throw new ArgumentException(ErrorMessages.InvalidPayloadSize);
            return payload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(DecodePayload), ex.Message));
            throw;
        }
    }

    private string DecodeHeaderEntry(byte[] message, ref int index)
    {
        try
        {
            // Decode header entry length
            var entryLength = GetEntryLength(message, ref index);

            // Decode header entry value
            var keyBytes = new byte[entryLength];
            Array.Copy(message, index, keyBytes, 0, entryLength);
            var name = Encoding.ASCII.GetString(keyBytes);
            
            index += entryLength;
            return name;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(DecodeHeaderEntry), ex.Message));
            throw;
        }
    }

    private int GetEntryLength(IReadOnlyList<byte> message, ref int index)
    {
        try
        {
            var entryBlockSize = message[index++];
            var entryLength = 0;
            for (var i = 0; i < entryBlockSize; i++)
            {
                entryLength += message[index++];
            }
        
            if (entryLength > _appSettings.MaxHeaderEntrySize)
                throw new ArgumentException(ErrorMessages.InvalidHeaderEntrySize);
        
            return entryLength;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(GetEntryLength), ex.Message));
            throw;
        }
    }
}