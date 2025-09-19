using System.Reflection;

namespace Chirp.CLI.Data;

public static class DataLoader
{
    public static string GetCsvPath()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Chirp"
        );

        string dataFolder = Path.Combine(folder, "data");
        Directory.CreateDirectory(dataFolder);

        string dataPath = Path.Combine(dataFolder, "chirp_cli_db.csv");

        if (!File.Exists(dataPath))
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "Chirp.CLI.data.chirp_cli_db.csv";

            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Could not find embedded resource: {resourceName}");

            using FileStream fileStream = File.Create(dataPath);
            stream.CopyTo(fileStream);
        }

        return dataPath;
    }
}