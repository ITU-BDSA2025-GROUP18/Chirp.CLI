namespace Chirp.CLI;

public abstract class UserInterface : Program
{
    // Function that writes out all cheeps in console
    public static void PrintCheeps(IEnumerable<Cheep<string>> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.ToString());
        }
    }
}