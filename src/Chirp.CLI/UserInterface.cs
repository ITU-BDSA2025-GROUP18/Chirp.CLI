namespace Chirp.CLI;

public abstract class UserInterface<T> : Program
{
    // Function that writes out all cheeps in console
    public static void PrintCheeps(IEnumerable<T> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.ToString());
        }
    }
}