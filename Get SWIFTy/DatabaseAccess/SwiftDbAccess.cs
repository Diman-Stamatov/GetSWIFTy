using System.Data;
using Get_SWIFTy.Database.Interface;
using Get_SWIFTy.Model;
using Microsoft.Data.Sqlite;

namespace Get_SWIFTy.Database;
public class SwiftDbAccess : ISwiftDbAccess
{
    static string databaseFileName = "SwiftDB.db";
    static string databasePath = Path.Combine(Directory.GetCurrentDirectory(), databaseFileName);
    static string connectionString = $"Data Source={databasePath}";

    public string RecordMessage(Message message)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = 
                "INSERT INTO Messages (SenderReference, MessageType, TransactionReference, " +
                "RelatedReference, Narrative, AuthenticationCode)" +
                $"VALUES (@senderReference, @messageType, @transactionReference, " +
                $"@relatedReference, @narrative, @authenticationCode)";

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

    public List<Message> ReadMessages()
    {
        var messages = new List<Message>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, SenderReference, MessageType, TransactionReference, " +
                "RelatedReference, Narrative, AuthenticationCode FROM Messages";
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string senderReference = reader.GetString(1);
                    string messageType = reader.GetString(2);
                    string transactionReference = reader.GetString(3);
                    string relatedReference = reader.GetString(4);
                    string narrative = reader.GetString(5);
                    string authenticationCode = reader.GetString(6);

                    messages.Add(new Message 
                    { 
                        Id = id,
                        SenderReference = senderReference,
                        MessageType = messageType,
                        TransactionReference = transactionReference,
                        RelatedReference = relatedReference,
                        Narrative = narrative,
                        AuthenticationCode = authenticationCode
                    });
                }
            }
        }

        return messages;
    }
}
