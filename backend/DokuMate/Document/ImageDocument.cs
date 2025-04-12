namespace DokuMate.PdfDocument;

public record ImageDocument(string Name, string? Info, List<FileInfo>? Images, List<Tag.Tag> Tags);