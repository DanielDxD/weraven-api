using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeRaven.Models;

public class Post
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public User User { get; set; } 
    public Guid UserId { get; set; }
    public string Content { get; set; } = "";
    public bool IsPrivate { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    [ForeignKey("PostId")]
    public List<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
    [ForeignKey("PostId")]
    public List<PostLike> PostLikes { get; set; } = new List<PostLike>();
    [ForeignKey("PostId")]
    public List<PostComment> PostComments { get; set; } = new List<PostComment>();
}