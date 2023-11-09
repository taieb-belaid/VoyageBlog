#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VoyageBlog.Models;
public class Comment_Post
{
    [Key]
    public int CommentId {get;set;}
    [Required]
    public string Content{get;set;}
    [Required]
    public int UserId{get;set;}
    [Required]
    public int PostId{get;set;}
    public User? User {get;set;}
    public Post? Post {get;set;}
}