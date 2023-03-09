using System.ComponentModel.DataAnnotations;

namespace WeRaven.Models;

public class PostMedia
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Post Post { get; set; }
    public Guid PostId { get; set; } 
    public Media Media { get; set; } 
    public Guid MediaId { get; set; }
}