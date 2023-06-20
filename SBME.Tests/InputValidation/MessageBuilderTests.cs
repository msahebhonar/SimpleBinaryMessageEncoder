using Moq;
using SBME.Common;
using SBME.Entities.Builders;
using SBME.Entities.Interfaces;
using SBME.Tests.SharedSettings;

namespace SBME.Tests.InputValidation;

[Collection(nameof(SettingsCollection))]
public class MessageBuilderTests
{
    private readonly SettingsFixture _settingsFixture;

    public MessageBuilderTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;
    }

    [Fact]
    public void Create_ValidPayloadSize_ReturnObject()
    {
        // Arrange
        var message = new MessageBuilder(_settingsFixture.Settings);
        var mockHeaderCollection = new Mock<IHeaderCollection>();

        // Act
        var result = message.Create(mockHeaderCollection.Object, new byte[256]);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IMessage>(result);
    }
    
    [Fact]
    public void Create_PayloadSizeOutsideLimit_ThrowException()
    {
        // Arrange
        var message = new MessageBuilder(_settingsFixture.Settings);
        var mockHeaderCollection = new Mock<IHeaderCollection>();

        // Act
        void Act() => message.Create(mockHeaderCollection.Object, new byte[(256 * 1024) + 1]);
        
        // Assert
        var ex = Assert.Throws<ArgumentException>((Action)Act);
        Assert.Equal(ErrorMessages.InvalidPayloadSize,ex.Message);
    }
}