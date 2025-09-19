namespace Chirp.CLI;

public abstract class UserInterface<T> : Program
{
    // Function that writes out all cheeps in console
    public static async Task PrintCheeps(IAsyncEnumerable<Cheep<string>> cheeps)
    {
        await foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.ToString());
        }
    }
}