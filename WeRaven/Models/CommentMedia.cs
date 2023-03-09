namespace WeRaven.Models;

public class CommentMedia
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public PostComment PostComment { get; set; } 
    public Guid CommentId { get; set; } 
    public Media Media { get; set; } 
    public Guid MediaId { get; set; }
}