namespace SBME.Entities.Interfaces;

public interface IMessage
{
    IHeaderCollection Headers { get; }
    
    byte[] Payload { get; }
}