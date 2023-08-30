using Get_SWIFTy.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Get_SWIFTy.Controller;

[Route("api/swift")]
[ApiController]
public class SwiftController : ControllerBase
{
    private const string EmptyMessageError = "Please input a Swift MT799 message.";


    private readonly ISwiftServices swiftService;   

    public SwiftController(ISwiftServices swiftService)
    {
        this.swiftService = swiftService;        
    }

    [HttpPost("/text")]
    [Consumes("text/plain")]
    public IActionResult LogSwiftMessageText([FromBody] string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return StatusCode(StatusCodes.Status400BadRequest, EmptyMessageError);
        }
                
        string logMessage = swiftService.ParseMessage(message);
        return StatusCode(StatusCodes.Status200OK, logMessage);
    }


    [HttpPost("/file")]
    public IActionResult LogSwiftMessageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return StatusCode(StatusCodes.Status400BadRequest, EmptyMessageError);
        }

        using (var streamReader = new StreamReader(file.OpenReadStream()))
        {
            string message = streamReader.ReadToEnd();
            string logMessage = swiftService.ParseMessage(message);
            return StatusCode(StatusCodes.Status200OK, logMessage);
        }
    }

    [HttpGet()]    
    public IActionResult ReadSwiftMessages()
    {
        var messages = swiftService.ShowMessages();

        if (string.IsNullOrWhiteSpace(messages))
        {
            string noMessagesError = "No messages have been logged to the Database yet!";
            return StatusCode(StatusCodes.Status404NotFound, noMessagesError);
        }    
        
        return StatusCode(StatusCodes.Status200OK, messages);
    }
}
