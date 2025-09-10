namespace SimpleDB;

public interface IDatabaseRepository<T>
{
    //Function that gets all cheeps from the crip_cli_db.csv file
    public IEnumerable<T> Read(int? limit = null); //For at gøre den generic (Fremgået også på billedet

    //Tilføjer en besked til chirp_cli_db.csv, inkl. user og tidspunkt
    public void Store(T message); //For at gøre den generic (Fremgår også på billedet)
}