#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Transactions;
namespace VoyageBlog.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    [MinLength(2)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    public string LastName { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "Name User Name much be between 3 and 15 characters.")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    public string Password { get; set; }

    [NotMapped]
    [Compare("Password", ErrorMessage = "The password do not match.")]
    public string ConfirmPassword { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Post> CreatedPosts {get;set;} = new List<Post>();
    public List<Comment_Post> MakeComments{get;set; } = new List<Comment_Post>(); 
}