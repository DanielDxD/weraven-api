namespace WeRaven.Models.Mongo.Helpers;

public class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; } = "";
    public Guid UserId { get; set; }
    public DateTime SendAt { get; set; } = DateTime.Now;
    public List<ChatMedia> Medias { get; set; } = new List<ChatMedia>();
    public bool Edited { get; set; } = false;
    public bool Erased { get; set; } = false;
}