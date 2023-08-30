using Get_SWIFTy.Model;

namespace Get_SWIFTy.Database.Interface;

public interface ISwiftDbAccess
{
    string RecordMessage(Message message);

    List<Message> ReadMessages();
}
