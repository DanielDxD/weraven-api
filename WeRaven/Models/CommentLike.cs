namespace WeRaven.Models;

public class CommentLike
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public PostComment PostComment { get; set; } 
    public Guid CommentId { get; set; } 
    public User User { get; set; } 
    public Guid UserId { get; set; }
}