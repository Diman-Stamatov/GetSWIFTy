using Get_SWIFTy.Service.Interface;
using System.Text.RegularExpressions;

namespace Get_SWIFTy.Service;

public class SwiftService : ISwiftService
{
    public string LogMessage(string message)
    {
        string senderReference = ParseField(message, "1:");
        string messageType = ParseField(message, "2:");
        string sequenceNumber = ParseField(message, "3:");
        string transactionReference = ParseField(message, "20:");
        string relatedReference = ParseField(message, "21:");
        string narrative = ParseField(message, "79:");
        return $"The message was logged successfully: {message}";
    }

    static string ParseField(string message, string tag)
    {
        string pattern = $@"{tag}([^\n]+)";
        Match match = Regex.Match(message, pattern);
        return match.Success ? match.Groups[1].Value : null;
    }

    
}
