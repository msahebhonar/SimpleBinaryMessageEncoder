using Microsoft.Extensions.Logging;
using Moq;
using SBME.Entities.Interfaces;
using SBME.Services.Interfaces;
using SBME.Tests.SharedSettings;

namespace SBME.Tests.SimpleBinaryMessageEncoder;

[Collection(nameof(SettingsCollection))]
public class SimpleBinaryMessageEncoderFixture
{
    public SimpleBinaryMessageEncoderFixture()
    {
        MockMessageFactory = new Mock<IMessageBuilder>();
        MockHeaderCollectionFactory = new Mock<IHeaderCollectionBuilder>();
        var settingsFixture = new SettingsFixture();
        var mockLogger = new Mock<ILogger<Services.SimpleBinaryMessageEncoder>>();
        Encoder = new Services.SimpleBinaryMessageEncoder(
            MockMessageFactory.Object, 
            MockHeaderCollectionFactory.Object, 
            settingsFixture.Settings, 
            mockLogger.Object);
    }

    public Mock<IMessageBuilder> MockMessageFactory { get; }
    
    public Mock<IHeaderCollectionBuilder> MockHeaderCollectionFactory { get; }
    
    public ISimpleBinaryMessageEncoder Encoder { get; }
}