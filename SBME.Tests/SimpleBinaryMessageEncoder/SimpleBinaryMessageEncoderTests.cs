using System.Collections.ObjectModel;
using System.Text;
using Moq;
using SBME.Common;
using SBME.Entities.Interfaces;

namespace SBME.Tests.SimpleBinaryMessageEncoder;

public class SimpleBinaryMessageEncoderTests:IClassFixture<SimpleBinaryMessageEncoderFixture>
{
    private readonly SimpleBinaryMessageEncoderFixture _fixture;
    public SimpleBinaryMessageEncoderTests(SimpleBinaryMessageEncoderFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Encode_ValidInput_EncodeMessage()
    {
        // Arrange
        var mockMessage = new Mock<IMessage>();
        var mockHeaderCollection = new Mock<IHeaderCollection>();

        mockMessage
            .Setup(x => x.Payload)
            .Returns(new byte[] { 0 });
        mockMessage
            .Setup(x => x.Headers.ReadOnlyDictionary)
            .Returns(new ReadOnlyDictionary<string, string>(new Dictionary<string, string>{{new string('x', 1023),"y"}}));

        _fixture
            .MockHeaderCollectionFactory
            .Setup(x => x.Create(It.IsAny<Dictionary<string, string>>()))
            .Returns(mockHeaderCollection.Object);

        _fixture
            .MockMessageFactory
            .Setup(x => x.Create(It.IsAny<IHeaderCollection>(), It.IsAny<byte[]>()))
            .Returns(mockMessage.Object);
        
        // Act
        var result = _fixture
            .Encoder
            .Encode(mockMessage.Object);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
        Assert.Equal(1034, result.Length);
    }
    
    [Fact]
    public void Decode_ValidInput_DecodeHeader()
    {
        // Arrange
        var message = new byte[] { 1, 1, 4, 110, 97, 109, 101, 1, 5, 118, 97, 108, 117, 101, 72, 105, 33 };
        var outputHeaders = new Dictionary<string, string>();
        
        var mockMessage = new Mock<IMessage>();
        var mockHeaderCollection = new Mock<IHeaderCollection>();
        
        _fixture
            .MockMessageFactory
            .Setup(x => x.Create(It.IsAny<IHeaderCollection>(), It.IsAny<byte[]>()))
            .Returns(mockMessage.Object);
        
        _fixture
            .MockHeaderCollectionFactory
            .Setup(x => x.Create(It.IsAny<Dictionary<string, string>>()))
            .Callback<Dictionary<string,string>>(headers => outputHeaders = headers)
            .Returns(mockHeaderCollection.Object);
        
        // Act
        _fixture
            .Encoder
            .Decode(message);
        
        // Assert
        Assert.Single(outputHeaders);
        Assert.Equal("value", outputHeaders["name"]);
    }
    
    [Fact]
    public void Decode_ValidInput_DecodePayload()
    {
        // Arrange
        var message = new byte[] { 1, 1, 4, 110, 97, 109, 101, 1, 5, 118, 97, 108, 117, 101, 72, 105, 33 };
        var outputPayload = Array.Empty<byte>();
        
        var mockMessage = new Mock<IMessage>();
        var mockHeaderCollection = new Mock<IHeaderCollection>();

        _fixture
            .MockMessageFactory.Setup(x => x.Create(It.IsAny<IHeaderCollection>(), It.IsAny<byte[]>()))
            .Callback<IHeaderCollection, byte[]>((headers, payload) => outputPayload = payload)
            .Returns(mockMessage.Object);
        
        _fixture
            .MockHeaderCollectionFactory
            .Setup(x => x.Create(It.IsAny<Dictionary<string, string>>()))
            .Returns(mockHeaderCollection.Object);
        
        // Act
        _fixture
            .Encoder
            .Decode(message);
        
        // Assert
        Assert.Equal("Hi!", Encoding.ASCII.GetString(outputPayload));
    }
    
    [Fact]
    public void Decode_HeaderCountExceeds_ThrowException()
    {
        // Arrange
        var message = new byte[] { 64, 1 };
        
        var mockMessage = new Mock<IMessage>();
        var mockHeaderCollection = new Mock<IHeaderCollection>();

        _fixture
            .MockMessageFactory.Setup(x => x.Create(It.IsAny<IHeaderCollection>(), It.IsAny<byte[]>()))
            .Returns(mockMessage.Object);
        
        _fixture
            .MockHeaderCollectionFactory
            .Setup(x => x.Create(It.IsAny<Dictionary<string, string>>()))
            .Returns(mockHeaderCollection.Object);
        
        // Act
        void Act() => _fixture
            .Encoder
            .Decode(message);
        
        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.ExceededHeaderCount, ex.Message);
    }
    
    [Fact]
    public void Decode_InvalidPayloadSize_ThrowException()
    {
        // Arrange
        var message = new byte[(256 * 1024) + 5];
        message[0] = 1;
        message[1] = 1;
        message[2] = 1;
        message[3] = 1;
        message[4] = 1;

        var mockMessage = new Mock<IMessage>();
        var mockHeaderCollection = new Mock<IHeaderCollection>();

        _fixture
            .MockMessageFactory.Setup(x => x.Create(It.IsAny<IHeaderCollection>(), It.IsAny<byte[]>()))
            .Returns(mockMessage.Object);
        
        _fixture
            .MockHeaderCollectionFactory
            .Setup(x => x.Create(It.IsAny<Dictionary<string, string>>()))
            .Returns(mockHeaderCollection.Object);
        
        // Act
        void Act() => _fixture
            .Encoder
            .Decode(message);
        
        // Assert
        var ex = Assert.Throws<ArgumentException>((Action) Act);
        Assert.Equal(ErrorMessages.InvalidPayloadSize, ex.Message);
    }
}