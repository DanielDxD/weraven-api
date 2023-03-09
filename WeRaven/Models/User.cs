using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WeRaven.Models;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Birthdate { get; set; }
    public bool Private { get; set; } = false;
    public string Username { get; set; }
    public string Password { get; set; }
    public string Bio { get; set; } = "";
    public string ProfilePhoto { get; set; } = "none.png";
    public string CanvaPhoto { get; set; } = "none";
    public bool Confirmed { get; set; } = false;
    public bool Verified { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    [ForeignKey("UserId")]
    public List<Auth> Auths { get; set; } = new List<Auth>();
    [ForeignKey("UserId")]
    public List<Post> Posts { get; set; } = new List<Post>();
    [ForeignKey("UserId")]
    public List<PostLike> PostLikes { get; set; } = new List<PostLike>();
    [ForeignKey("UserId")]
    public List<PostComment> PostComments { get; set; } = new List<PostComment>();
    [ForeignKey("UserId")]
    public List<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
}