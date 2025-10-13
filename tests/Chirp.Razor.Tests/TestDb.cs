using Microsoft.Data.Sqlite;

public static class TestDb
{
    public static string CreateWithSeed()
    {
        var path = Path.Combine(Path.GetTempPath(), $"chirp_test_{Guid.NewGuid():N}.db");
        using var conn = new SqliteConnection($"Data Source={path}");
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "";
        cmd.ExecuteNonQuery();
        return path;
    }
}
