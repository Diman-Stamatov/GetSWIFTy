using Get_SWIFTy.Model;

namespace Get_SWIFTy.Database.Interface;

public interface ISwiftDB
{
    string AddMessage(Message message);
}
