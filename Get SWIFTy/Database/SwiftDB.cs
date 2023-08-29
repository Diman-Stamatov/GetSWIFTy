using System.Data;
using Get_SWIFTy.Database.Interface;
using Get_SWIFTy.Model;
using Microsoft.Data.Sqlite;

namespace Get_SWIFTy.Database;
public class SwiftDB : ISwiftDB
{
    public string AddMessage(Message message)
    {

        string databaseFileName = "SwiftDB.db";
        Directory.GetCurrentDirectory();
        string databasePath = Path.Combine(Directory.GetCurrentDirectory(), databaseFileName);
        string connectionString = $"Data Source={databasePath}";        

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = 
                "INSERT INTO Messages (SenderReference, MessageType, TransactionReference, RelatedReference, Narrative, AuthenticationCode)" +
                $"VALUES (@senderReference, @messageType, @transactionReference, @relatedReference, @narrative, @authenticationCode)";

            command.Parameters.AddWithValue("@senderReference", message.SenderReference);
            command.Parameters.AddWithValue("@messageType", message.MessageType);
            command.Parameters.AddWithValue("@transactionReference", message.TransactionReference);
            command.Parameters.AddWithValue("@relatedReference", message.RelatedReference);
            command.Parameters.AddWithValue("@narrative", message.Narrative);
            command.Parameters.AddWithValue("@authenticationCode", message.AuthenticationCode);

            command.ExecuteNonQuery();
        }

        return "Message logged successfully.";
    }
}
