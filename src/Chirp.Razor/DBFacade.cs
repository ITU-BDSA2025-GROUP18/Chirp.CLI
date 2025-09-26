using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DbFacade
{
    private const string SqlDbFilePath = "./Data/cheeps.db";
    private readonly SqliteConnection _connection;
    private readonly SqliteCommand _command;

    public DbFacade()
    {
        _connection = new SqliteConnection($"Data Source={SqlDbFilePath}");
        _connection.Open();

        _command = _connection.CreateCommand();
    }

    public void CloseConnection()
    {
        _connection.Close();
    }

    public List<CheepViewModel> GetCheeps(int? limit = null)
    {
        var cheeps = new List<CheepViewModel>();
        var sqlQuery =
            $"""
            SELECT u.username, m.text, m.pub_date
            FROM message m, user u
            WHERE m.author_id = u.user_id
            ORDER by m.pub_date DESC;
            """;

        if (limit != null) sqlQuery += " LIMIT " + limit;

        _command.CommandText = sqlQuery;
        using var reader = _command.ExecuteReader();
        while (reader.Read())
        {
            IDataRecord dataRecord = reader;

            var username = dataRecord[0].ToString()!;
            var text = dataRecord[1].ToString()!;
            var pubDate = long.Parse(dataRecord[2].ToString()!);

            var formattedTimeStamp = FormatTimestamp(pubDate);

            cheeps.Add(new CheepViewModel(username, text, formattedTimeStamp));
        }

        return cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        var cheeps = new List<CheepViewModel>();
        var sqlQuery =
            $"""
            SELECT u.username, m.text, m.pub_date
            FROM message m JOIN user u
            ON m.author_id = u.user_id
            WHERE u.username = '{author}'
            ORDER BY m.pub_date DESC;
            """;

        _command.CommandText = sqlQuery;
        using var reader = _command.ExecuteReader();

        while (reader.Read())
        {
            IDataRecord dataRecord = reader;

            var username = dataRecord[0].ToString()!;
            var text = dataRecord[1].ToString()!;
            var pubDate = long.Parse(dataRecord[2].ToString()!);

            var formattedTimeStamp = FormatTimestamp(pubDate);

            cheeps.Add(new CheepViewModel(username, text, formattedTimeStamp));
        }

        return cheeps;
    }

    private static string FormatTimestamp(long pubDate)
    {
        var formattedTimeStamp = DateTimeOffset
            .FromUnixTimeSeconds(pubDate)
            .LocalDateTime
            .ToString(CultureInfo.InvariantCulture);
        return formattedTimeStamp;
    }
}
