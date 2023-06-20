using SBME.Entities.Interfaces;

namespace SBME.Services.Interfaces;

public interface ISimpleBinaryMessageEncoder
{
    byte[] Encode(IMessage message);
    
    IMessage Decode(byte[] data);
}