using System.Text;
using Microsoft.Extensions.Logging;
using SBME.Common;

namespace SBME.Services.Handlers;

public class EncodingHandler
{
    private readonly ILogger<SimpleBinaryMessageEncoder> _logger;

    public EncodingHandler(ILogger<SimpleBinaryMessageEncoder> logger)
    {
        _logger = logger;
    }

    public byte[] EncodeHeaders(IReadOnlyDictionary<string, string> headers)
    {
        try
        {
            var headerBytes = new List<byte> { (byte)headers.Count };

            foreach (var header in headers)
            {
                // Encode header key
                EncodeEntry(headerBytes, header.Key);

                // Encode header value
                EncodeEntry(headerBytes, header.Value);
            }

            return headerBytes.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(EncodeHeaders), ex.Message));
            throw;
        }
    }

    private void EncodeEntry(List<byte> headerBytes, string entry)
    {
        try
        {
            // Blocks of bytes for entry 
            var entryBlockCount = (int)Math.Ceiling((float)entry.Length / byte.MaxValue);

            // Save block size for entry
            headerBytes.Add((byte)entryBlockCount);

            // Reserve blocks of bytes for entry
            var remainingLength = entry.Length;
            while (remainingLength > 0)
            {
                var length = Math.Min(remainingLength, byte.MaxValue);
                headerBytes.Add((byte)length);
                remainingLength -= length;
            }

            headerBytes.AddRange(Encoding.ASCII.GetBytes(entry));
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.FormatErrorLog(nameof(EncodeEntry), ex.Message));
            throw;
        }
    }
}