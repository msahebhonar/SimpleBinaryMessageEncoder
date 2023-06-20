using Microsoft.Extensions.Options;
using SBME.Common;
using SBME.Entities.Interfaces;

namespace SBME.Entities;

public class Message:IMessage
{
    private readonly AppSettings _appSettings;
    private readonly byte[] _payload;
    
    internal Message(IHeaderCollection headers, byte[] payload, IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        Headers = headers ?? throw new ArgumentNullException(nameof(headers), ErrorMessages.HeadersRequired);
        Payload = payload;
    }
    
    public IHeaderCollection Headers { get; init; }
    
    public byte[] Payload
    {
        get => _payload;
        init
        {
            if (value == null)
                throw new ArgumentNullException(nameof(Payload), ErrorMessages.PayloadRequired);

                // Ensure payload size is within limits
            if (value.Length > _appSettings.MaxPayloadSizeInByte)
                throw new ArgumentException(ErrorMessages.InvalidPayloadSize);
            
            _payload = value;
        }
    }
}