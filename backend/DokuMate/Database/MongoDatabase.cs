using DokuMate.Helpers;
using MongoDB.Driver;

namespace DokuMate.Database;

public class MongoDatabase
{
    public IMongoDatabase Db { get; }
    
    public MongoDatabase()
    {
        var mongoClient = new MongoClient($"mongodb://{DotEnv.GetVar("CONNECTION_STRING")}");
        Db = mongoClient.GetDatabase(DotEnv.GetVar("DATABASE_NAME"));
    }
}