using Get_SWIFTy.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Get_SWIFTy.Controller;

[Route("api/swift")]
[ApiController]
public class SwiftController : ControllerBase
{
    private const string EmptyMessageError = "Please input a Swift MT799 message.";

    private readonly ILogger Logger;
    private readonly ISwiftServices SwiftService;   

    public SwiftController(ISwiftServices swiftService, ILogger<SwiftController> logger)
    {
        this.SwiftService = swiftService; 
        this.Logger = logger;
    }

    [HttpPost("/text")]
    [Consumes("text/plain")]
    public IActionResult LogSwiftMessageText([FromBody] string message)
    {
        Logger.LogInformation("Message parsing from plain text started.");

        if (string.IsNullOrWhiteSpace(message))
        {
            Logger.LogWarning($"Message parsing from plain text failed: {EmptyMessageError}");
            return StatusCode(StatusCodes.Status400BadRequest, EmptyMessageError);
        }
                
        string logMessage = SwiftService.ParseMessage(message);

        Logger.LogInformation("Message parsing from plain text successful.");
        return StatusCode(StatusCodes.Status200OK, logMessage);
    }


    [HttpPost("/file")]
    public IActionResult LogSwiftMessageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            Logger.LogWarning($"Message parsing from .txt file failed: {EmptyMessageError}");
            return StatusCode(StatusCodes.Status400BadRequest, EmptyMessageError);
        }

        using (var streamReader = new StreamReader(file.OpenReadStream()))
        {
            string message = streamReader.ReadToEnd();
            string logMessage = SwiftService.ParseMessage(message);

            Logger.LogInformation("Message parsing from .txt file successful.");
            return StatusCode(StatusCodes.Status200OK, logMessage);
        }
    }

    [HttpGet()]    
    public IActionResult ReadSwiftMessages()
    {
        Logger.LogInformation("Message reading from database started");
        var messages = SwiftService.ShowMessages();

        if (string.IsNullOrWhiteSpace(messages))
        {
            string noMessagesError = "No messages have been logged to the Database yet!";

            Logger.LogWarning($"Message reading from database failed: {noMessagesError}");
            return StatusCode(StatusCodes.Status404NotFound, noMessagesError);
        }

        Logger.LogInformation("Message reading from database successful");
        return StatusCode(StatusCodes.Status200OK, messages);
    }
}
