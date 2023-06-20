namespace SBME.Entities.Interfaces;

public interface IHeaderCollectionBuilder
{
    IHeaderCollection Create(Dictionary<string, string> headers);
}