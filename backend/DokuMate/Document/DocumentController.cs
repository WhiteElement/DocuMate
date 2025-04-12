using Microsoft.AspNetCore.Mvc;

namespace DokuMate.Document;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly DocumentService _documentService;

    public DocumentController(DocumentService documentService)
    {
        _documentService = documentService;
    }
    
    // public List<PdfDocument> GetAll()
    // {
    //     
    // }

    [HttpPost]
    public async Task<PdfDocument> CreateNew([FromForm] ImageDocument imageDocument)
    {
        return await _documentService.CreateOne(imageDocument);
    }
}