using MongoDB.Bson.Serialization.Attributes;
using WeRaven.Models.Mongo.Helpers;

namespace WeRaven.Models.Mongo;

public class Chat
{
    [BsonId]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Guid UserA { get; set; } 
    public Guid UserB { get; set; } 
    public List<Message> Messages { get; set; } = new List<Message>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}