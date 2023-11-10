#pragma warning disable
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using VoyageBlog.Models;

namespace VoyageBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;
    private MyContext _context;
    private User LoggedInUser
    {
        get { return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId")); }
    }
    public HomeController(ILogger<HomeController> logger, MyContext context, IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _context = context;
        this._hostEnvironment = hostEnvironment;
    }

    public IActionResult Index() 
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpGet("signup")]
    public IActionResult SignUp(){
        return View();
    }
            //_____Register________
    [HttpPost("/user/register")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {   
            //Unique Email
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email already taken");
                return View("Signup");
            }
            //Unique UserName
            if (_context.Users.Any(u => u.UserName == newUser.UserName))
            {
                ModelState.AddModelError("UserName", "User Name already exist");
                return View("Signup");
            }
            //Hashing password
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);
            //Save to db
            _context.Add(newUser);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("signup");
    }
    //_____Login_________
    [HttpPost("/user/login")]
    public IActionResult Login(Login logUser)
    {
        if (ModelState.IsValid)
        {
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == logUser.LoginEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Incorrect Validation");
                return View("Index");
            }
            //comparing pass
            PasswordHasher<Login> hasher = new PasswordHasher<Login>();
            var result = hasher.VerifyHashedPassword(logUser, userInDb.Password, logUser.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Incorrect Validation");
                return View("Index");
            }
            HttpContext.Session.SetInt32("userId", userInDb.UserId);
            HttpContext.Session.SetString("userName", userInDb.UserName);
            return RedirectToAction("Main");
        }
        return View("Index");
    }
    //__________Main_Page________
    [HttpGet("main")] 
    public IActionResult Main()
    {
        return View();
    }
    //_________Post_View_____
    [HttpGet("post")]
    public IActionResult Post()
    {
        ViewBag.User = LoggedInUser;
        return View();
    }
    //________Post_method______
    [HttpPost("/user/post")]
        public IActionResult Post(Post newPost)
    {

            _context.Posts.Add(newPost);
            _context.SaveChanges();
            return RedirectToAction("Main");
    }

    //_______Explore_View______
    [HttpGet("explore")]
    public IActionResult Explore(){
        ViewBag.Posts = _context.Posts.Include(c=>c.CommentPost).ToList();
        return View();
    }
    //______One_Post_View__
    [HttpGet("/onepost/{postId}")]
    public IActionResult OnePost(int postId)
    {
        ViewBag.User = LoggedInUser;
        ViewBag.OnePost = _context.Posts.Include(p=>p.Creator)
                            .FirstOrDefault(p=>p.PostId == postId);
        // _____comment_____
        ViewBag.Comments = _context.Posts.Include(p => p.CommentPost)
                                        .ThenInclude(c=>c.User)
                                        .FirstOrDefault(p=>p.PostId == postId);

        return View();
    }

    //_________Comment_Action__
    [HttpPost("/message/new")]
    public IActionResult Comment(Comment_Post newCom)
    {
        _context.Comments.Add(newCom);
        _context.SaveChanges();
        return RedirectToAction("explore");
    }

    //________delete_one_post__
    [HttpGet("/delete/{postId}")]
    public IActionResult Delete(int postId)
    {
        Post deletePost = _context.Posts.SingleOrDefault(p=>p.PostId == postId);
        _context.Posts.Remove(deletePost);
        _context.SaveChanges();
        return RedirectToAction("Explore");
    } 
    //__________Edit_one_post____
    [HttpGet("/edit/{postId}")]
    public IActionResult EditPost(int postId)
    {
        Post editPost = _context.Posts.SingleOrDefault(p=>p.PostId == postId);
        ViewBag.User = LoggedInUser;
        return View(editPost);
    }
    //__________Update_One_Post
    [HttpPost("/update/{postId}")]
    public IActionResult Update(int postId, Post newPost)
    {
        Post oldPost = _context.Posts.SingleOrDefault(p=>p.PostId == postId);
        oldPost.Title = newPost.Title;
        oldPost.ImageUrl = newPost.ImageUrl;
        oldPost.Description = newPost.Description;
        oldPost.UpdatedAt = newPost.UpdatedAt;
        _context.SaveChanges();
        return RedirectToAction("Explore");
    }
    //[____Log_Out____
    [HttpGet("logout")]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
