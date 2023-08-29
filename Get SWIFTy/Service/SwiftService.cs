using Get_SWIFTy.Service.Interface;

namespace Get_SWIFTy.Service;

public class SwiftService : ISwiftService
{
    public string LogMessage(string message)
    {
        return $"The message was logged successfully: {message}";
    }
}
