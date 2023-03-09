using System.ComponentModel.DataAnnotations;
using WeRaven.Tools;

namespace WeRaven.Models;

public class Auth
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public User User { get; set; }
    public Guid UserId { get; set; }
    public int Code { get; set; } = MathTool.GenerateCode();
    public DateTime ExpiryAt { get; set; } = DateTime.UtcNow.AddHours(1);
}