using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Options;
using SBME.Common;
using SBME.Entities.Interfaces;

namespace SBME.Entities;

public class HeaderCollection:IHeaderCollection
{
    private readonly AppSettings _appSettings;
    private readonly Dictionary<string, string> _headers;
    
    internal HeaderCollection(Dictionary<string, string> headers, IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        _headers = new Dictionary<string, string>();

        if (headers.Count == 0)
            throw new ArgumentException(ErrorMessages.HeadersRequired);
        
        // Ensure new object meets the collection standards
        foreach (var header in headers)
            Add(header.Key, header.Value);
    }
    
    public string this[string key]
    {
        get
        {
            if (_headers.TryGetValue(key, out var value))
            {
                return value;
            }

            throw new KeyNotFoundException($"The key '{key}' does not exist in the dictionary.");
        }
    }
    
    public int Count => _headers.Count;
    
    public IReadOnlyDictionary<string, string> ReadOnlyDictionary => new ReadOnlyDictionary<string, string>(_headers);
    
    private void Add(string name, string value)
    {
        // Ensure header count is within limits
        if (Count >= _appSettings.MaxNumOfHeaders)
            throw new ArgumentException(ErrorMessages.ExceededHeaderCount);

        // Ensure header name and value are not null or whitespace
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(ErrorMessages.InvalidHeaderEntry);

        // Ensure header name and value are ASCII encoded
        if (!name.IsAscii() || !value.IsAscii())
            throw new ArgumentException(ErrorMessages.InvalidHeaderEntryFormat);
        
        // Ensure header name and value sizes are within limits
        if (name.Length > _appSettings.MaxHeaderEntrySize || value.Length > _appSettings.MaxHeaderEntrySize)
            throw new ArgumentException(ErrorMessages.InvalidHeaderEntrySize);
        
        _headers.Add(name, value);
    }
    
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _headers.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}