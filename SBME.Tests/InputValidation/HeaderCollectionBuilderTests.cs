using SBME.Common;
using SBME.Entities.Builders;
using SBME.Entities.Interfaces;
using SBME.Tests.SharedSettings;

namespace SBME.Tests.InputValidation;

[Collection(nameof(SettingsCollection))]
public class HeaderCollectionBuilderTests
{
    private readonly SettingsFixture _settingsFixture;

    public HeaderCollectionBuilderTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;
    }
    
    [Fact]
    public void Create_HeaderCountIsInLimit_ReturnObject()
    {
        // Arrange
        var headers = new Dictionary<string, string>();
        for (var i = 0; i < 63; i++)
            headers.Add(i.ToString(), i.ToString());
        
        // Act
        var headerCollectionFactory = new HeaderCollectionBuilder(_settingsFixture.Settings);
        var result = headerCollectionFactory.Create(headers);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IHeaderCollection>(result);
    }
    
    [Fact]
    public void Create_HeaderCountOutsideLimit_ThrowException()
    {
        // Arrange
        var headers = new Dictionary<string, string>();
        for (var i = 0; i < 64; i++)
            headers.Add(i.ToString(), i.ToString());
        
        // Act
        var headerCollectionFactory = new HeaderCollectionBuilder(_settingsFixture.Settings);
        void Act() => headerCollectionFactory.Create(headers);

        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.ExceededHeaderCount, ex.Message);
    }
    
    [Fact]
    public void Create_HeaderNameIsWhitespace_ThrowException()
    {
        // Arrange
        var headers = new Dictionary<string, string> { { "", "value" } };

        // Act
        var headerCollectionFactory = new HeaderCollectionBuilder(_settingsFixture.Settings);
        void Act() => headerCollectionFactory.Create(headers);

        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.InvalidHeaderEntry, ex.Message);
    }
    
    [Fact]
    public void Create_HeaderValueIsWhitespace_ThrowException()
    {
        // Arrange
        var headers = new Dictionary<string, string> { { "name", "" } };

        // Act
        var headerCollectionFactory = new HeaderCollectionBuilder(_settingsFixture.Settings);
        void Act() => headerCollectionFactory.Create(headers);

        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.InvalidHeaderEntry, ex.Message);
    }
    
    [Fact]
    public void Create_NonASCIIEncodedValues_ThrowException()
    {
        // Arrange
        var headers = new Dictionary<string, string> { { "name", "åäö" } };

        // Act
        var headerCollectionFactory = new HeaderCollectionBuilder(_settingsFixture.Settings);
        void Act() => headerCollectionFactory.Create(headers);

        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.InvalidHeaderEntryFormat, ex.Message);
    }
    
    [Fact]
    public void Create_HeaderNameOutsideLimit_ThrowException()
    {
        // Arrange
        var headers = new Dictionary<string, string> { { new string('x', 1024), "value" } };

        // Act
        var headerCollectionFactory = new HeaderCollectionBuilder(_settingsFixture.Settings);
        void Act() => headerCollectionFactory.Create(headers);

        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.InvalidHeaderEntrySize, ex.Message);
    }
    
    [Fact]
    public void Create_HeaderValueOutsideLimit_ThrowException()
    {
        // Arrange
        var headers = new Dictionary<string, string> { { "name", new string('x', 1024) } };

        // Act
        var headerCollectionFactory = new HeaderCollectionBuilder(_settingsFixture.Settings);
        void Act() => headerCollectionFactory.Create(headers);

        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.InvalidHeaderEntrySize, ex.Message);
    }
}