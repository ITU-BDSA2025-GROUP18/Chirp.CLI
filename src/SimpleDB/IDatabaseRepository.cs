namespace SimpleDB;

public interface IDatabaseRepository<T>
{
    // Function that gets all cheeps from the crip_cli_db.csv file
    public IEnumerable<T> Read(int? limit = null);

    // Adds a message to chirp_cli_db.csv
    public void Store(T message);
}