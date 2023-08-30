using Get_SWIFTy.Model;

namespace Get_SWIFTy.Service.Interface;

public interface ISwiftServices
{
    string ParseMessage(string message);

    string ShowMessages();
}
