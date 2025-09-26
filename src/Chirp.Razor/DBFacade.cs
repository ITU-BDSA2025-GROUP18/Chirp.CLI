using System.Data;
using System.Globalization;
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

    public List<CheepViewModel> GetCheeps(int? limit = null)
    {
        var sqlQuery =
            @"SELECT m.*, u.* FROM message m, user u
              WHERE m.author_id = u.user_id
              ORDER by m.pub_date desc";

        if (limit != null) sqlQuery += " LIMIT " + limit;

        _command.CommandText = sqlQuery;

        var cheeps = new List<CheepViewModel>() { };

        using var reader = _command.ExecuteReader();
        while (reader.Read())
        {
            var dataRecord = (IDataRecord)reader;

            var username = dataRecord[5].ToString()!;
            var text = dataRecord[2].ToString()!;
            var pub_date = long.Parse(dataRecord[3].ToString()!);

            var formattedTimeStamp = DateTimeOffset
                .FromUnixTimeSeconds(pub_date)
                .LocalDateTime
                .ToString(CultureInfo.InvariantCulture);

            cheeps.Add(new CheepViewModel(username, text, formattedTimeStamp));
        }

        return cheeps;
    }
}
