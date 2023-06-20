using Microsoft.Extensions.Options;
using SBME.Common;
using SBME.Entities.Interfaces;

namespace SBME.Entities.Builders;

public class MessageBuilder:IMessageBuilder
{
    private readonly IOptions<AppSettings> _appSettings;
    
    public MessageBuilder(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }
    
    public IMessage Create(IHeaderCollection headers, byte[] payload)
    {
        return new Message(headers, payload, _appSettings);
    }
}