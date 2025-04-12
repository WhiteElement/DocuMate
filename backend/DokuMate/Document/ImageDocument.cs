namespace DokuMate.Document;

public record ImageDocument(string Name, string? Info, List<IFormFile>? Images, List<Tag.Tag>? Tags);