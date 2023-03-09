using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeRaven.Models;

public class PostComment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Post Post { get; set; } 
    public Guid PostId { get; set; } 
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    [ForeignKey("CommentId")]
    public List<CommentMedia> CommentMedias { get; set; } = new List<CommentMedia>();
    [ForeignKey("CommentId")]
    public List<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
}