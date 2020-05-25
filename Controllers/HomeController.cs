using StudentBookApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentBookApp.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View(); //this displays the home page for the website
		}

        //this code was adapted from https://stackoverflow.com/questions/927847/is-there-a-url-validator-on-net
        public static bool IsValidURI(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute)) //this checks if the url is well formed 
                return false; //if it is well formed it'll return false
            Uri tmp;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps; //this checks if and returns it either as http or https
        }
    }
}