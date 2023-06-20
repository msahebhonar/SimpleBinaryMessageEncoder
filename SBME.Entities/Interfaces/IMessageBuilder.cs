namespace SBME.Entities.Interfaces;

public interface IMessageBuilder
{
    IMessage Create(IHeaderCollection headers, byte[] payload);
}