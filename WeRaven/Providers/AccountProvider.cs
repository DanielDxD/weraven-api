using MongoDB.Driver;
using WeRaven.Models;
using WeRaven.Models.Mongo;

namespace WeRaven.Providers;

public class AccountProvider : ProviderBase<Account>
{
    public AccountProvider() : base("Account")
    {}

    public async Task<List<Account>> GetAsync() => await _mongoCollection.Find(_ => true).ToListAsync();

    public async Task<Account?> GetAsync(string id) =>
        await _mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Account?> GetAsync(Guid id) =>
        await _mongoCollection.Find(x => x.UserId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Account account) => await _mongoCollection.InsertOneAsync(account);

    public async Task UpdateAsync(Account account) =>
        await _mongoCollection.ReplaceOneAsync(x => x.Id == account.Id, account);

    public async Task RemoveAsync(string id) => await _mongoCollection.DeleteOneAsync(x => x.Id == id);
}