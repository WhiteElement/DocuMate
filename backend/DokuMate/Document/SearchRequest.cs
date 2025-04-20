namespace DokuMate.Document;

public record SearchRequest(string Name, List<Tag.Tag> Tags);