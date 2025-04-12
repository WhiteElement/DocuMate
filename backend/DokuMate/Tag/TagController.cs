using DokuMate.Database;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DokuMate.Tag;

[ApiController]
[Route("[controller]")]
public class TagController : ControllerBase
{
    private readonly IMongoCollection<Tag> _tagCollection;
    private const string CollectionName = "Tags";

    public TagController(MongoDatabase mongoDatabase)
    {
        _tagCollection = mongoDatabase.Db.GetCollection<Tag>(CollectionName);
    }
    
    [HttpGet]
    public async Task<List<Tag>> GetAll()
    {
        return await _tagCollection.Find(_ => true)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> CreateNew([FromBody] TagDTO dto)
    {
        Tag newTag = new Tag()
        {
            Name = dto.Name
        };

        if (await (await _tagCollection.FindAsync(x => x.Name == dto.Name)).AnyAsync())
            return Conflict($"Tag with Name '{dto.Name}' already exists");
        
        
        await _tagCollection.InsertOneAsync(newTag);
        return Created(String.Empty, newTag);
    }
}