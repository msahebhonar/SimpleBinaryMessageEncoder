using System.Text;

namespace SBME.Common;

public static class CommonExtensions
{
    public static bool IsAscii(this string input)
    {
        return Encoding.UTF8.GetByteCount(input) == input.Length;
    }
}