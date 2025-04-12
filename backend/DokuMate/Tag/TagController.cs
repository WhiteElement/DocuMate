using DokuMate.Database;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DokuMate.Tag;

[ApiController]
[Route("[controller]")]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;

    public TagController(TagService tagService)
    {
        _tagService = tagService;
    }
    
    [HttpGet]
    public async Task<List<Tag>> GetAll()
    {
        return await _tagService.GetAll();
    }

    [HttpPost]
    public async Task<ActionResult> CreateNew([FromBody] TagDTO dto)
    {
        if (await _tagService.NameAlreadyTaken(dto.Name))
            return Conflict($"Tag with Name '{dto.Name}' already exists");
        
        return Created(String.Empty, await _tagService.CreateNew(dto));
    }
}