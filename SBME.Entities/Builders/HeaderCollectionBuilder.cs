using Microsoft.Extensions.Options;
using SBME.Common;
using SBME.Entities.Interfaces;

namespace SBME.Entities.Builders;

public class HeaderCollectionBuilder:IHeaderCollectionBuilder
{
    private readonly IOptions<AppSettings> _appSettings;

    public HeaderCollectionBuilder(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    public IHeaderCollection Create(Dictionary<string, string> headers)
    {
        return new HeaderCollection(headers, _appSettings);
    }
}