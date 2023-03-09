using System.ComponentModel.DataAnnotations;

namespace WeRaven.Models;

public class PostLike
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Post Post { get; set; } 
    public Guid PostId { get; set; } 
    public User User { get; set; } 
    public Guid UserId { get; set; }
}