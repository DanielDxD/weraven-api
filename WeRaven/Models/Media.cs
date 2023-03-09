using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeRaven.Models;

public class Media
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } 
    public string MimeType { get; set; }
    [ForeignKey("MediaId")]
    public List<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
    [ForeignKey("MediaId")]
    public List<CommentMedia> CommentMedias { get; set; } = new List<CommentMedia>();
}