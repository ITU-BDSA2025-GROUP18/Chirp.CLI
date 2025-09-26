using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade
{
    private const string sqlDBFilePath = "./Data/database.db";
    private readonly SqliteConnection _connection;
    private readonly SqliteCommand _command;

    public DBFacade()
    {
        _connection = new SqliteConnection($"Data Source={sqlDBFilePath}");
        _connection.Open();

        _command = _connection.CreateCommand();
    }

    public void CloseConnection() { _connection.Close(); }

    public List<CheepViewModel> GetCheeps()
    {


        return null!;
    }
}
