namespace SBME.Common;

public class AppSettings
{
    public int MaxHeaderEntrySize { get; set; }
    
    public int MaxNumOfHeaders { get; set; }

    public int MaxPayloadSize { get; set; }

    public int MaxPayloadSizeInByte => MaxPayloadSize * 1024;
}