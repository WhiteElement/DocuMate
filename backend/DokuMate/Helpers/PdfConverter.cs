using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;

namespace DokuMate.Helpers;

public class PdfConverter
{
    private List<FileInfo> _images;
    private List<FileInfo> _tracker = new();
    private readonly string _name;
    private readonly bool _keepTempFiles;
    private FileInfo _pdf;

    private readonly string _tempFolder;

    public PdfConverter(string name, List<IFormFile> images, bool keepTempFiles = false)
    {
        _name = name.EndsWith(".pdf") ? name : $"{name}.pdf";
        _keepTempFiles = keepTempFiles;
        _tempFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        if (!Directory.Exists(_tempFolder))
            Directory.CreateDirectory(_tempFolder);
        
        _images = images.Select(image =>
        {
            string filePath = Path.Combine(_tempFolder, image.FileName);
            using Stream fs = new FileStream(filePath, FileMode.Create);
            image.CopyTo(fs);
            return new FileInfo(filePath);
        }).ToList();
        
        _images.ForEach(image => _tracker.Add(image));
    }

    public string ToPdf()
    {
        _pdf = new FileInfo(Path.Combine(_tempFolder, _name));
        _tracker.Add(_pdf);
        PdfDocument pdfDocument = new PdfDocument(new PdfWriter(_pdf));
        iText.Layout.Document document = new iText.Layout.Document(pdfDocument);

        try
        {
            _images.ForEach(imageInfo =>
            {
                ImageData imageData = ImageDataFactory.Create(imageInfo.FullName);
                Image image = new Image(imageData);
                image.SetWidth(pdfDocument.GetDefaultPageSize().GetWidth() - 50);
                image.SetAutoScaleHeight(true);
                document.Add(image);
            });
        }
        finally
        {
            pdfDocument.Close();
        }

        return _pdf.FullName;
    }

    public void CleanUp()
    {
        _tracker.ForEach(file =>
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        );
        
        if (!Directory.GetFiles(_tempFolder).Any())
            Directory.Delete(_tempFolder);
    }

    public void DocumentScan()
    {
        ImageProcessor imageProcessor = new ImageProcessor()
        {
            GenerateIntermediateFiles = _keepTempFiles,
            InputFiles = _images.ToArray(),
            OutputFilePath = new DirectoryInfo(_tempFolder)
        };
        string[] outputFiles = imageProcessor.DocumentScan();
        ReplaceInputWithScans(outputFiles);
    }

    private void ReplaceInputWithScans(string[] scans)
    {
        _images = scans
            .Select(scan => new FileInfo(scan))
            .ToList();
        
        _images.ForEach(image => _tracker.Add(image));
    }
}