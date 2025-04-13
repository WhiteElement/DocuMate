using Microsoft.AspNetCore.Mvc;

namespace DokuMate.Tag;

[ApiController]
[Route("api/[controller]")]
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag([FromRoute] string? id)
    {
        if (id == null)
            throw new ArgumentException("No Id for Tag to delete provided");
        
        long deleted = await _tagService.DeleteTag(id);

        if (deleted == 0)
            return NotFound($"No Tag with Id: '{id}' found");

        return Accepted($"Deleted '{id}'");
    }
    
}