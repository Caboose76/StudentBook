using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using StudentBookApp.Models;
using StudentBookApp.Models.Extended;
using System.IO;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace StudentBookApp.Controllers
{
    //this controllers code was adapted from https://www.youtube.com/watch?v=gSJFjuWFTdA&list=PLdUi92csWFpKWs_JMTaSv8_h7AfRMjJ4Z&index=14&t=1570s
    public class AccountController : Controller
    {
        private StudentBookAppDB dbModel = new StudentBookAppDB();  //this use's the database to get the user information and makes it a global variable

        //Login GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken] //this prevents cross site scripting
        public ActionResult Login(UserLogin login, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var v = dbModel.Users.Where(c => c.EmailID == login.EmailID).FirstOrDefault(); //this seeks the users info in the database
                if (v != null)
                {
                    if (string.Compare(Models.Extended.Crypto.getHash(login.Password), v.Password) == 0) //this compare's the user's password in the login web page to the one in the database
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout); //this creates the ticket
                        string encypted = FormsAuthentication.Encrypt(ticket); //this encrypts the user's ticket
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encypted); //this creates a cookie for the user and makes sure it is encrypted as well for the remember me function
                        cookie.Expires = DateTime.Now.AddMinutes(timeout); //once the cookie expires the user will have to re-login and re-enter their login credentials
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("NewsFeed", "Account"); //this redirects the user to their newsfeed on the website
                        }
                    }
                }
                ViewBag.ErrorMessage = "Email or Password is not valid!!!"; //this is to display an error message when a user types in an invalid email and password
            }
            return View();
        }

        //Logout function
        [Authorize] //this attribute's is only given to user's and they have to be given an role such as must be a registered user inorder to access this feature 
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); //this create's the logout function
            return RedirectToAction("Login", "Account"); //when the user signs out this redirects the user to the login page
        }

        [Authorize]
        public ActionResult NewsFeed()
        {
            return View();
        }
    }
}