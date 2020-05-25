using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using StudentBookApp.Models;

namespace StudentBookApp.Controllers
{
    public class PostsController : Controller
    {
        private StudentBookAppDB db = new StudentBookAppDB(); //this makes the database connection a global variable

        static public List<Post> UserPosts(User currentUser, List<Post> allPosts)
        {
            //exclude posts without authors to evade null reference error
            List<Post> postsWithAuthors = allPosts.Where(p => p.User != null).ToList();

            //check remaining posts
            List<Post> userPosts = allPosts.Where(p => p.User.EmailID == currentUser.EmailID).OrderByDescending(p => p.Date).ToList();
            return (userPosts);
        }

        // GET: Posts
        [Authorize]
        public ActionResult Index()
        {
            var postsWithAuthors = db.Posts.Include(p => p.User).ToList(); //this organises the posts in to a list on a webpage
            return View(postsWithAuthors); //this returns the post list
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) //this checks to see if the user id is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //if the user id is null it displays an error page
            }
            Post post = db.Posts.Include(p => p.User).Include(p => p.Comments).SingleOrDefault(x => x.Id == id); //this organises the posts and adds the comments to each post
            var comments = db.Comments.Include(p => p.User).Where(cm => cm.PostId == post.Id).OrderByDescending(cm => cm.Date).ToList(); //this orders the comments by descending order
            post.Comments = comments; //this equals the post.Comments to the comments table to retrieve the information from the database

            if (post == null) //this checks if the posts exists
            {
                return HttpNotFound(); //if the post does not exist it'll return a http not found error page
            }
            bool postExists = FeedController.IsValidURI(post.Body); //if the post exists it was calling a method through the feedcontroller to check if it was a valid url and check for posts that are associated with the tags in the array
            ViewBag.PostExists = postExists; //this displays the posts from the feedcontroller with the tags
            return View(post); //this displays the posts on the webpage

        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to 
        // this code was adapted from https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,TagString,Tags")] Post post)
        {
            if (ModelState.IsValid) //this checks if the modelstate is valid
            {
                List<string> taglist = post.TagString.Split(' ').ToList(); //this organises the taglist by splitting it so the tags are all one line
                foreach (var tagl in taglist)
                {
                    post.Tags.Add(new Tag() { Name = tagl }); //this attachs the tag's to the users post
                }
                post.User = db.Users.FirstOrDefault(user => user.EmailID == User.Identity.Name); //this checks the user email in the database to the current signed in user to see if it was valid
                post.Date = DateTime.Now; //this displays the current time to the post when it is being posted
                db.Posts.Add(post); //this adds the users post to the database table
                db.SaveChanges(); //this saves the changes in the database
                return RedirectToAction("NewsFeed", "Account"); //this redirects the user to the newsfeed
            }

            return View(post); //this returns the post to the webpage
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null) //this checks to see if the user id is null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //if the user id is null it displays an error page
            }
            Post post = db.Posts.Find(id); //this finds the original post the user is updating
            if (post == null)  //if the posts the user is trying to edit does not exit
            {
                return HttpNotFound(); //this will display an error message when the post does not exist
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        // this code was adapted from https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,TagString,Tags")] Post post)
        {
            Post thisPost = this.db.Posts.Include(p => p.Tags).FirstOrDefault(p => p.Id == post.Id);

            if (ModelState.IsValid)
            {
                thisPost.Tags.Clear(); //this clears the tags when the user is updating the post

                db.SaveChanges(); //this saves the database when changes are made

                List<string> taglist = post.TagString.Split(' ').ToList(); //this organises the taglist by splitting it so the tags are all one line 

                foreach (var tagl in taglist)
                {
                    thisPost.Tags.Add(new Tag() { Name = tagl }); //this attachs the tag's to the users post
                }
                thisPost.Title = post.Title; //this updates the post title
                thisPost.Body = post.Body; //this updates the posts body
                thisPost.Date = DateTime.Now; //this updates the comments date when the user edits their comments
                thisPost.TagString = post.TagString; //this updates the tag string
                thisPost.User = db.Users.FirstOrDefault(user => user.EmailID == User.Identity.Name);
                db.Entry(thisPost).State = EntityState.Modified; //this updates the database
                db.SaveChanges(); //this saves the database when changes are made
                return RedirectToAction("Index", "Posts");
            }
            return View(post); //this displays the new post when it was being updated
        }

        // this code was adapted from https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}