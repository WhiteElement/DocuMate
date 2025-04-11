using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DokuMate.Tag;

public class Tag
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
}