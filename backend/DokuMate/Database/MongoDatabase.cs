using MongoDB.Driver;

namespace DokuMate.Database;

public class MongoDatabase
{
    public IMongoDatabase Db { get; set; }
    
    public MongoDatabase()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        Db = mongoClient.GetDatabase("TestDatabase");
    }
}