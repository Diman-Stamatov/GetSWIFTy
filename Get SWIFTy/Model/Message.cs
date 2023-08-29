namespace Get_SWIFTy.Model;

public class Message
{
    public static int NextId = 1;

    public int Id;
    public string SenderReference;
    public string MessageType;
    public string TransactionReference;
    public string RelatedReference;
    public string Narrative;
    public string AuthenticationCode;
}
