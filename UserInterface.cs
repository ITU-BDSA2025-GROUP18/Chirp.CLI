public class UserInterface : Program
{
    public static void Main(string[] args)
    {
        if (args.Length< 1) Console.WriteLine("Invalid argument(s)");
        if (args[0] == "read") read();
        else if (args[0] == "cheep") cheep(args);
    }
}