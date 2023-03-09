using Microsoft.EntityFrameworkCore;
using WeRaven.Models;

namespace WeRaven.Data;

public class AppDbContext : DbContext
{
    public  DbSet<Auth> Auths { get; set; } 
    public DbSet<CommentLike> CommentLikes { get; set; } 
    public DbSet<CommentMedia> CommentMedias { get; set; } 
    public DbSet<Media> Medias { get; set; } 
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostComment> PostComments { get; set; } 
    public DbSet<PostLike> PostLikes { get; set; } 
    public DbSet<PostMedia> PostMedias { get; set; }
    public DbSet<User> Users { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
    { }
}