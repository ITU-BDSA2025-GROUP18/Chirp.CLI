using System.Globalization;

namespace Chirp.CLI.Tests
{
    public class ProgramAndUiTests : IDisposable
    {
        private readonly string _tmpDir;
        private readonly string _dbPath;
        private readonly TextWriter _origOut;
        private readonly TextWriter _origErr;

        public ProgramAndUiTests()
        {
            // Kør tests i et midlertidigt arbejdskatalog, så pilles ikke ved rigtig CSV-fil
            _tmpDir = Directory.CreateTempSubdirectory("chirp-tests-").FullName;
            _dbPath = Path.Combine(_tmpDir, "chirp_cli_db.csv");
            Directory.SetCurrentDirectory(_tmpDir);

            _origOut = Console.Out;
            _origErr = Console.Error;
        }

        public void Dispose()
        {
            Console.SetOut(_origOut);
            Console.SetError(_origErr);

            try { Directory.Delete(_tmpDir, recursive: true); }
            catch { /* ignorer oprydningsfejl på CI/Windows */ }
        }

        [Fact]
        public void Read_PrintsFormattedLinesFromCsv()
        {
            // Arrange: CsvHelper forventer header-række
            var ts = 0L;
            var expectedTime = DateTimeOffset
                .FromUnixTimeSeconds(ts).LocalDateTime
                .ToString(CultureInfo.InvariantCulture);

            File.WriteAllText(_dbPath,
                "Author,Message,Timestamp\n" +
                $"alice,hej med dig,{ts}\n");

            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            UserInterface.PrintCheeps();

            // Assert
            var output = sw.ToString();
            Assert.Contains($"alice @ {expectedTime}: hej med dig", output);
        }

        [Fact]
        public void Cheep_MissingArgument_ReturnsNonZeroAndWritesError()
        {
            using var swErr = new StringWriter();
            Console.SetError(swErr);

            // Act
            var code = Program.Main(new[] { "cheep" });

            // Assert
            Assert.NotEqual(0, code);
            Assert.Contains("required", swErr.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Cheep_WritesLineToCsvAndReturnsZero()
        {
            // Arrange
            var msg = $"hello-{Guid.NewGuid()}";
            var user = Environment.UserName;

            // Act
            var code = Program.Main(new[] { "cheep", msg });

            // Assert
            Assert.Equal(0, code);
            Assert.True(File.Exists(_dbPath));

            var text = File.ReadAllText(_dbPath);
            Assert.Contains(user, text);
            Assert.Contains(msg, text);
        }
    }
}

