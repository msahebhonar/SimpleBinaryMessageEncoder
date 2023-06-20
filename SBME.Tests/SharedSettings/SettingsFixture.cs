using Microsoft.Extensions.Options;
using SBME.Common;

namespace SBME.Tests.SharedSettings;

public class SettingsFixture
{
    public SettingsFixture()
    {
        var appSettings = new AppSettings
        {
            MaxHeaderEntrySize = 1023,
            MaxNumOfHeaders = 63,
            MaxPayloadSize = 256
        };
        Settings = Options.Create(appSettings);
    }

    public IOptions<AppSettings> Settings { get; }
}