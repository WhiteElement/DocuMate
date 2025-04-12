using Microsoft.AspNetCore.Mvc;

namespace DokuMate.PdfDocument;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    public List<PdfDocument> GetAll()
    {
        
    }
}