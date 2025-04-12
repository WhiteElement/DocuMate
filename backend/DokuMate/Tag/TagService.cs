using DokuMate.Database;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DokuMate.Tag;

public class TagService
{
    private readonly IMongoCollection<Tag> _tagCollection;
    private const string CollectionName = "Tags";

    public TagService(MongoDatabase mongoDatabase)
    {
        _tagCollection = mongoDatabase.Db.GetCollection<Tag>(CollectionName);
    }

    public async Task<List<Tag>> GetAll()
    {
        return await _tagCollection
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<Tag> CreateNew(TagDTO dto)
    {
        Tag newTag = new Tag()
        {
            Name = dto.Name
        };

        await _tagCollection.InsertOneAsync(newTag);
        return newTag;
    }

    public async Task<bool> NameAlreadyTaken(string dtoName)
    {
        var found = await _tagCollection.FindAsync(x => x.Name == dtoName);
        return await found.AnyAsync();
    }
}