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

    //
    // GET
    //

    [HttpGet]
    public async Task<List<PdfDocumentDTO>> GetAll()
    {
        return await _documentService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PdfDocumentDTO>> GetOne([FromRoute] string id)
    {
        PdfDocumentDTO? found = await _documentService.GetOne(id);
        if (found == null)
            return NotFound();
        
        return Ok(found);
    }
    
    [HttpGet("{id}/download")]
    public ActionResult Download([FromRoute] string id)
    {
        PdfDocument found = _documentService.Download(id);
        return new FileContentResult(found.Binary.AsByteArray, "application/pdf")
        {
            FileDownloadName = found.Name
        };
    }
    
    //
    // POST
    //
    
    [HttpPost]
    public async Task<PdfDocumentDTO> CreateNew([FromForm] ImageDocument imageDocument)
    {
        PdfDocument document = await _documentService.CreateOne(imageDocument);
        return new PdfDocumentDTO( document.Id, document.Name, document.Info, document.Tags, document.Created );
    }
    
    //
    // DELETE
    //

    [HttpDelete]
    public async Task DeleteAll()
    {
        await _documentService.DeleteAll();
    }
}