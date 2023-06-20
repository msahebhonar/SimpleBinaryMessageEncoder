namespace SBME.Common;

public static class ErrorMessages
{
    public const string HeadersRequired = "Headers are required.";
    
    public const string ExceededHeaderCount = "Exceeded maximum number of headers.";

    public const string InvalidHeaderEntry = "Header entry cannot be null or empty.";

    public const string InvalidHeaderEntryFormat = "Header entry is not an ASCII encoded.";

    public const string InvalidHeaderEntrySize = "Exceeded header entry size.";

    public const string PayloadRequired = "Payload is required.";

    public const string EncodingFailed = "Encoding failed. No encoded values generated.";

    public const string MessageEmpty = "Message cannot be null or empty";

    public const string InvalidMessage = "Invalid input format. No bytes found.";

    public const string InvalidPayloadSize = "Exceeded maximum payload size.";
    
    public static string FormatErrorLog(string method, string error)
    {
        return $"{method} : {error}";
    }

    public static string InvalidByte(string index)
    {
        return $"Invalid byte value at index {index}";
    }
}