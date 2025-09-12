namespace Chirp.CLI;

public record Cheep<T>(string Author, T Message, long Timestamp);