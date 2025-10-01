using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DbFacade
{
    private readonly string _sqlDatabasePath;
    private readonly SqliteConnection _connection;
    private readonly SqliteCommand _command;

    public DbFacade()
    {
        var databasePathEnvVar = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if (databasePathEnvVar is not null)
            _sqlDatabasePath = databasePathEnvVar;
        else
        {
            var chirpDatabaseDir = Path.Combine(Path.GetTempPath(), ".chirp");

            if (!Directory.Exists(chirpDatabaseDir))
                Directory.CreateDirectory(chirpDatabaseDir);

            var chirpDatabaseFile = Path.Combine(chirpDatabaseDir, "chirp.db");

            if (!File.Exists(chirpDatabaseFile))
            {
                var conn = new SqliteConnection($"Data Source={chirpDatabaseFile}");
                conn.Open();

                var comm = conn.CreateCommand();
                comm.CommandText =
                """
                CREATE TABLE user (
                user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                username STRING NOT NULL,
                email STRING NOT NULL
                );

                CREATE TABLE message (
                message_id INTEGER PRIMARY KEY AUTOINCREMENT,
                author_id INTEGER NOT NULL,
                text STRING NOT NULL,
                pub_date INTEGER
                );

                INSERT INTO user VALUES
                  (1,'Helge','ropf@itu.dk'),
                  (2,'Adrian','adho@itu.dk');

                INSERT INTO message VALUES
                  (1,1,'Hello, BDSA students!',1690892208),
                  (2,2,'Hej, velkommen til kurset.',1690895308);
                """;

                comm.ExecuteNonQuery();

                conn.Close();
            }

            _sqlDatabasePath = chirpDatabaseFile;
        }

        _connection = new SqliteConnection($"Data Source={_sqlDatabasePath}");
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
            ORDER by m.pub_date DESC
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

            var formattedTimestamp = FormatTimestamp(pubDate);

            cheeps.Add(new CheepViewModel(username, text, formattedTimestamp));
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
            ORDER BY m.pub_date DESC
            """;

        _command.CommandText = sqlQuery;
        using var reader = _command.ExecuteReader();

        while (reader.Read())
        {
            IDataRecord dataRecord = reader;

            var username = dataRecord[0].ToString()!;
            var text = dataRecord[1].ToString()!;
            var pubDate = long.Parse(dataRecord[2].ToString()!);

            var formattedTimestamp = FormatTimestamp(pubDate);

            cheeps.Add(new CheepViewModel(username, text, formattedTimestamp));
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
