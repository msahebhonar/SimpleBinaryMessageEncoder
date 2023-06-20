using System.ComponentModel;

namespace SBME.Api.Dtos;

public class MessageDto
{
    [DefaultValue("{\"Content-Type\":\"application/json\",\"Authorization\":\"Bearer e.abcdef.123456\"}")]
    public Dictionary<string, string> Headers { get; set; }

    [DefaultValue("Embrace the power of connection with Sinch!")]
    public string Payload { get; set; }
}