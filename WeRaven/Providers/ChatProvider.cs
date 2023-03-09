using MongoDB.Driver;
using WeRaven.Models;
using WeRaven.Models.Mongo;

namespace WeRaven.Providers;

public class ChatProvider : ProviderBase<Chat>
{
    public ChatProvider() : base("Chat")
    {}
    
    public async Task<List<Chat>> GetAsync() => await _mongoCollection.Find(_ => true).ToListAsync();

    public async Task<Chat?> GetAsync(string id) =>
        await _mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Chat?> GetAsync(Guid id) =>
        await _mongoCollection.Find(x => x.UserA == id || x.UserB == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Chat chat) => await _mongoCollection.InsertOneAsync(chat);

    public async Task UpdateAsync(Chat chat) =>
        await _mongoCollection.ReplaceOneAsync(x => x.Id == chat.Id, chat);

    public async Task RemoveAsync(string id) => await _mongoCollection.DeleteOneAsync(x => x.Id == id);
}