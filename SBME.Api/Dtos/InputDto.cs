using System.ComponentModel;

namespace SBME.Api.Dtos;

public class InputDto
{
    [DefaultValue("2 1 8 76 97 110 103 117 97 103 101 1 5 101 110 45 85 83 1 7 86 101 114 115 105 111 110 1 3 49 46 48 72 105 32 116 104 101 114 101 33")]
    public string Message { get; set; }
}