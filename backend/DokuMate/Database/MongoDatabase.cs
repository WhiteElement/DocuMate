using DokuMate.Helpers;
using MongoDB.Driver;

namespace DokuMate.Database;

public class MongoDatabase
{
    public IMongoDatabase Db { get; }
    
    public MongoDatabase()
    {
        if (!DotEnv.TryGetVar("CONNECTION_STRING", out string connString) ||
            !DotEnv.TryGetVar("DATABASE_NAME", out string dbName))
            throw new ArgumentException("ConnectionString or DatabaseName not provided");
                
        var mongoClient = new MongoClient($"mongodb://{connString}");
        Db = mongoClient.GetDatabase(dbName);
    }
}