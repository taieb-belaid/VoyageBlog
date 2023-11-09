#pragma warning disable

using Microsoft.EntityFrameworkCore;
namespace VoyageBlog.Models;

public class MyContext : DbContext 
{ 
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users {get;set;}
    public DbSet<Post> Posts {get;set;}
    public DbSet<Comment_Post> Comments{get;set;}
}