using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SBME.Api.Dtos;
using SBME.Common;
using SBME.Entities.Interfaces;
using SBME.Services.Interfaces;

namespace SBME.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly ISimpleBinaryMessageEncoder _encoder;

        public MessageController(ISimpleBinaryMessageEncoder encoder, ILogger<MessageController> logger)
        {
            _encoder = encoder;
            _logger = logger;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult EncodeMessage(
            [FromServices] IMessageBuilder messageBuilder, 
            [FromServices] IHeaderCollectionBuilder headerCollectionBuilder, 
            [FromBody][Required] MessageDto messageDto)
        {
            try
            {
                // Validate input header
                if (messageDto.Headers.Count == 0)
                    return BadRequest(ErrorMessages.HeadersRequired);
                
                // Validate input payload
                if (messageDto.Payload is null)
                    return BadRequest(ErrorMessages.PayloadRequired);

                var message = messageBuilder.Create(
                    headerCollectionBuilder.Create(messageDto.Headers), 
                    Encoding.ASCII.GetBytes(messageDto.Payload));
                
                var result = _encoder.Encode(message);
                
                // Ensure there are encoded values before joining them
                if (result == null || result.Length == 0)
                    return BadRequest(ErrorMessages.EncodingFailed);

                // Join the encoded values with a space separator
                return Ok(string.Join(' ',result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessages.FormatErrorLog(nameof(EncodeMessage), ex.Message));
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InputDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DecodeMessage([FromBody][Required] InputDto message)
        {
            try
            {
                // Validate input message
                if (string.IsNullOrWhiteSpace(message.Message))
                    return BadRequest(ErrorMessages.MessageEmpty);
                    
                var array = message.Message.Split(' ');
                
                // Validate input array length
                if (array.Length == 0)
                    return BadRequest(ErrorMessages.InvalidMessage);
                
                var byteArray = new byte[array.Length];
                for (var i = 0; i < array.Length; i++)
                {
                    // Parse each byte value and validate it
                    if (!byte.TryParse(array[i], out byte byteValue))
                        return BadRequest(ErrorMessages.InvalidByte(i.ToString()));
                    
                    byteArray[i] = byteValue;
                }
                var result = _encoder.Decode(byteArray);
                var output = new MessageDto
                {
                    Headers = new Dictionary<string, string>(result.Headers.ReadOnlyDictionary),
                    Payload = Encoding.UTF8.GetString(result.Payload)
                };
                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessages.FormatErrorLog(nameof(DecodeMessage), ex.Message));
                return BadRequest(ex.Message);
            }
        }
    }
}
