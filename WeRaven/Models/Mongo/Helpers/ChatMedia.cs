namespace WeRaven.Models.Mongo.Helpers;

public class ChatMedia
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Url { get; set; } 
    public string MimeType { get; set; } 
}