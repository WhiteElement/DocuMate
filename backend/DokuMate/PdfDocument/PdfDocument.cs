namespace DokuMate.PdfDocument;

public class PdfDocument
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Info { get; set; }
    public List<Tag.Tag> Tags { get; set; }
    private FileInfo Binary { get; set; }
    private string? OcrContent { get; set; }
    private DateTime Created { get; set; }
}