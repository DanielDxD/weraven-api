using MongoDB.Driver;
using WeRaven.Tools;

namespace WeRaven.Providers;

public class ProviderBase<T>
{
    protected readonly IMongoCollection<T> _mongoCollection;
    private readonly string _dbName = "WeRaven";

    public ProviderBase(string collectionName)
    {
        var mongoClient = new MongoClient(EnvTool.MongoConnection);
        var mongoDatabase = mongoClient.GetDatabase(_dbName);
        _mongoCollection = mongoDatabase.GetCollection<T>(collectionName);
    }
}