using MongoDB.Bson.Serialization.Attributes;
using WeRaven.Models.Mongo.Helpers;

namespace WeRaven.Models.Mongo;

public class Account
{
    [BsonId]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Guid UserId { get; set; }
    public List<string> Followers { get; set; } = new List<string>();
    public List<string> Following { get; set; } = new List<string>();
    public List<string> Blocklist { get; set; } = new List<string>();
    public List<string> BestFriends { get; set; } = new List<string>();
    public Settings Settings { get; set; } = new();
}