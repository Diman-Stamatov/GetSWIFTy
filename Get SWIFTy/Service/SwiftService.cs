using Get_SWIFTy.Database.Interface;
using Get_SWIFTy.Model;
using Get_SWIFTy.Service.Interface;
using System.Text.RegularExpressions;

namespace Get_SWIFTy.Service;

public class SwiftService : ISwiftService
{
    private const string FieldPatternEnding = "(.*?)}";
    private const string FreeformFieldPatternEnding = @"([^\n]+)";

    private readonly ISwiftDB swiftDB;

    public SwiftService(ISwiftDB swiftDB)
    {
        this.swiftDB = swiftDB;
    }
    public string LogMessage(string message)
    {
         

        string senderReference = ParseField(message, "1:", FieldPatternEnding);
        string messageType = ParseField(message, "2:", FieldPatternEnding);
        string transactionReference = ParseField(message, ":20:", FreeformFieldPatternEnding);
        string relatedReference = ParseField(message, ":21:", FreeformFieldPatternEnding);
        string narrative = ParseField(message, ":79:", FieldPatternEnding);
        string authenticationCode = ParseField(message, "MAC:", FieldPatternEnding);

        var newMessage = new Message
        {
            SenderReference = senderReference,
            MessageType = messageType,
            TransactionReference = transactionReference,
            RelatedReference = relatedReference,
            Narrative = narrative,
            AuthenticationCode = authenticationCode
        };

        string databaseResponse = swiftDB.AddMessage(newMessage);

        return databaseResponse;
    }

    static string ParseField(string message, string fieldCode, string patternEnding)
    {
        string pattern = $@"{fieldCode}{patternEnding}";
        Match match = Regex.Match(message, pattern, RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value : null;
    }

    
}
