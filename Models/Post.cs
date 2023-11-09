#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VoyageBlog.Models;
public class Post
{
    [Key]
    public int PostId {get;set;}
    [Required]
    public string Title {get;set;}
    public string ImageUrl {get;set;}
    [Required]
    public string Description{get;set;}
    [Required]
    public int UserId{get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public User? Creator{get;set;}
    public List<Comment_Post> CommentPost {get;set;} = new List<Comment_Post>();
}