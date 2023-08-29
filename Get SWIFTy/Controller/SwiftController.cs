using Get_SWIFTy.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Get_SWIFTy.Controller;

[Route("api/swift")]
[ApiController]
public class SwiftController : ControllerBase
{
    private const string EmptyMessageError = "Please input a Swift MT799 message.";


    private readonly ISwiftService swiftService;   

    public SwiftController(ISwiftService swiftService)
    {
        this.swiftService = swiftService;        
    }
        
    [HttpPost()]
    [Consumes("text/plain")]
    public IActionResult LogSwiftMessage([FromBody] string message)
    {        
        if (string.IsNullOrWhiteSpace(message))
        {
            return StatusCode(StatusCodes.Status400BadRequest, EmptyMessageError);
        }    

        string logMessage = swiftService.LogMessage(message);
        return StatusCode(StatusCodes.Status200OK, logMessage);
    }
}
