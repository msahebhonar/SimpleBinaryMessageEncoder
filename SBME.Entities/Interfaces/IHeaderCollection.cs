namespace SBME.Entities.Interfaces;

public interface IHeaderCollection:IEnumerable<KeyValuePair<string, string>>
{
    string this[string key] { get; }
    
    int Count { get; }
    
    IReadOnlyDictionary<string, string> ReadOnlyDictionary { get; }
}