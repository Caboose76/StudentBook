using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using StudentBookApp.Models;
using StudentBookApp.Models.Extended;

namespace StudentBookApp.Controllers
{
    //this controllers code was adpated from https://www.youtube.com/watch?v=xBS9FMF2NFM&list=PLdUi92csWFpKWs_JMTaSv8_h7AfRMjJ4Z&index=6
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Register(int id = 0) //this starts the User ID at zero
        {
            User userModel = new User(); //this calls the user class as a variable
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //this prevents cross site scripting
        public ActionResult Register(User userModel)
        {
            if (ModelState.IsValid)
            {
                using (StudentBookAppDB dbModel = new StudentBookAppDB())
                {
                    if (dbModel.Users.Any(x => x.EmailID == userModel.EmailID)) //this checks if the user is using an email address the same as another user in the user database
                    {
                        ViewBag.DuplicateMessage = "E-Mail Already Exists, Please Choose another"; //this displays the error message to the user
                        return View("Register", userModel); //this returns the view for the register page
                    }

                    userModel.Password = Models.Extended.Crypto.getHash(userModel.Password); //this converts the password and passe's it to the Encrpt Class to convert the password
                    userModel.ConfirmPassword = Models.Extended.Crypto.getHash(userModel.ConfirmPassword); //this also hashe's the confirmed password
                    dbModel.Users.Add(userModel); //this adds the user to the database when they register for an account
                    dbModel.SaveChanges(); //this saves the changes in the database
                }
                ModelState.Clear();
                ViewBag.SuccessMessage = "Registration has been successful for " + userModel.EmailID; //this lets the user know that the registration for their account has been successful
            }
            return View("Register", new User()); //this returns to the view with the new user object
        }
    }
}