namespace DokuMate.Document;

public record PdfDocumentDTO(string Id, string Name, string? Info, List<Tag.Tag> Tags, DateTime Created);
