using System.Data;
using Microsoft.Data.Sqlite;

var sqlDBFilePath = "./tmp/database.db";
var sqlQuery = @"SELECT m.*, u.* FROM message m, user u
                 WHERE m.author_id = u.user_id
                 ORDER by m.pub_date desc";

using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
{
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = sqlQuery;

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        var dataRecord = (IDataRecord)reader;

        var username = dataRecord[5];
        var text = dataRecord[2];
        var pub_date = dataRecord[3];

        Console.WriteLine($"{username} @ {pub_date}: {text}");
    }
}
