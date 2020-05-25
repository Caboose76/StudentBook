using StudentBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace StudentBookApp.Controllers
{
    public class FeedController : Controller
    {
        private StudentBookAppDB db = new StudentBookAppDB (); //this makes the database connection a global variable

        //this code was adapted from https://stackoverflow.com/questions/927847/is-there-a-url-validator-on-net
        public static bool IsValidURI ( string uri )
        {
            if ( !Uri.IsWellFormedUriString (uri, UriKind.Absolute)) //this checks if the url is well formed 
                return false; //if it is well formed it'll return false
            Uri tmp;
            if ( !Uri.TryCreate (uri, UriKind.Absolute, out tmp) )
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps; //this checks if and returns it either as http or https
        }

        //this code was adapted from https://stackoverflow.com/questions/39777659/extract-the-video-id-from-youtube-url-in-net
        public static bool IsYouTubeVideo(string url)
        {
            string[] URLSplit = url.Split('.').ToArray(); //this adds the youtube link and splits the url up and adds it into an array
            if (URLSplit.Contains("youtube")) //this checks the url 
            {
                return true;
            }
            return false;
        }

        //this code was adapted from https://stackoverflow.com/questions/39129555/add-item-into-list-from-view-and-pass-to-controller-in-mvc5
        //this code was also adapted from https://docs.microsoft.com/en-us/aspnet/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/display_data_items_and_details
        public ActionResult Gaming()
        {
            // Listing tagged posts 
            string[] tagsForListing = new string[] { "game", "gaming" }; //this puts the tags to an array so they can assign the tags to any post when they are typed into the web input field 

            IQueryable<Post> postByTag = db.Posts.Include(p=>p.User).OrderByDescending(p=>p.Date); //this sorts the posts by tags and orders them by descending order in the database 
            List<Post> posts = new List<Post> ( ); //this lists posts from the database when they are created 
            foreach ( var tagText in tagsForListing ) //this create's a foreach loop by doing a tag text by searching for posts through the string array and match all posts to those tags so they can be displayed 
            {
                posts.AddRange (postByTag.Where (p => p.Tags.Select (t => t.Name).Contains (tagText)).ToList ( )); //this searchs the posts with the tags game or gaming tags in the database and they will be displayed in HTML 
            }

            //Check if image is existing
            List<bool> postsExist = new List<bool> (); //this checks if a post exists through a boolean 
            List<bool> isYoutubeVideo = new List<bool>();
            foreach(var post in posts)
            {
                if(IsValidURI(post.Body))
                {
                    postsExist.Add(true); //if the posts exist then it will add it on the webpage and display
                }
                else
                {
                    postsExist.Add(false); //if it returns false then it wont be displayed on the webpage
                }

                if ( IsYouTubeVideo (post.Body)) //this checks if the post is a youtube video by calling the method that checks it
                {
                    isYoutubeVideo.Add(true); //if its true then it will be displayed on the webpage
                }
                else
                {
                    isYoutubeVideo.Add(false);
                }
            }

            ViewBag.IsYoutubeVideo = isYoutubeVideo; //if it is a youtube then the viewbag will pass that data to the HTML and display it on the webpage
            ViewBag.PostExists = postsExist; //if a post is has data and the boolean is true it will pass the data from the database and display on the webpage 
            ViewBag.TaggedPosts = posts.Distinct();
            return View(posts.ToList());
        }

        public ActionResult Software()
        {
            // Listing tagged posts 

            string[] tagsForListing = new string[] { "wcs", "software", "college" };

            IQueryable<Post> postByTag = db.Posts.Include(p => p.User).OrderByDescending(p => p.Date);
            List<Post> posts = new List<Post>();
            foreach (var tagText in tagsForListing)
            {
                posts.AddRange(postByTag.Where(p => p.Tags.Select(t => t.Name).Contains(tagText)).ToList());
            }

            //Check if image is existing
            List<bool> postsExist = new List<bool>();
            List<bool> isYoutubeVideo = new List<bool>();
            foreach (var post in posts)
            {
                if (IsValidURI(post.Body))
                {
                    postsExist.Add(true);
                }
                else
                {
                    postsExist.Add(false);
                }

                if (IsYouTubeVideo(post.Body))
                {
                    isYoutubeVideo.Add(true);
                }
                else
                {
                    isYoutubeVideo.Add(false);
                }
            }

            ViewBag.IsYoutubeVideo = isYoutubeVideo;
            ViewBag.PostExists = postsExist;
            ViewBag.TaggedPosts = posts.Distinct();
            return View(posts.ToList());
        }

        public ActionResult Books()
        {
            // Listing tagged posts 

            string[] tagsForListing = new string[] { "Book", "books" };

            IQueryable<Post> postByTag = db.Posts.Include(p => p.User).OrderByDescending(p => p.Date);
            List<Post> posts = new List<Post>();
            foreach (var tagText in tagsForListing)
            {
                posts.AddRange(postByTag.Where(p => p.Tags.Select(t => t.Name).Contains(tagText)).ToList());
            }

            //Check if image is existing
            List<bool> postsExist = new List<bool>();
            List<bool> isYoutubeVideo = new List<bool>();
            foreach (var post in posts)
            {
                if (IsValidURI(post.Body))
                {
                    postsExist.Add(true);
                }
                else
                {
                    postsExist.Add(false);
                }

                if (IsYouTubeVideo(post.Body))
                {
                    isYoutubeVideo.Add(true);
                }
                else
                {
                    isYoutubeVideo.Add(false);
                }
            }

            ViewBag.IsYoutubeVideo = isYoutubeVideo;
            ViewBag.PostExists = postsExist;
            ViewBag.TaggedPosts = posts.Distinct();
            return View(posts.ToList());
        }
    }
}