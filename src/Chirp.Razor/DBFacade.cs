using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade
{
    private const string sqlDBFilePath = "./Data/database.db";
    private readonly SqliteConnection connection;

    public DBFacade()
    {
        connection = new SqliteConnection($"Data Source={sqlDBFilePath}");
        connection.Open();
    }

    public void CloseConnection() { connection.Close(); }

}
