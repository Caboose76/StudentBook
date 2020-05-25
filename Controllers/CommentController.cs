using StudentBookApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentBookApp.Controllers
{
    public class CommentController : Controller
    {
        private StudentBookAppDB db = new StudentBookAppDB(); //this makes the database connection a global variable

        // GET: Comments/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            Comment cm =
            new Comment
            {
                PostId = id, //this makes post id equal to the comments id so the comment the user created associated to the one post
            };
            
            return View(cm); //this returns the comment to the webpage 
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        // this code was adapted from https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] Comment comment)
        {
            if (ModelState.IsValid) //this checks if the modelstate is valid
            {
                comment.Date = DateTime.Now; //this displays the current time to the comment when it is being posted
                comment.User = db.Users.FirstOrDefault(user => user.EmailID == User.Identity.Name); //this checks the user email in the database to the current signed in user to see if it was valid
                db.Comments.Add(comment); //this adds the users comment to the database table
                db.SaveChanges(); //this saves the changes in the database
                return RedirectToAction("Details", "Posts", new { id = comment.PostId }); //this redirects the user to the post the user commentated in
            }

            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId); //this shows all the posts there is on the current page
            return View(comment); //this returns the comment to the webpage
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