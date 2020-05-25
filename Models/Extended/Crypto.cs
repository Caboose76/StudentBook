using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNet.Identity;

namespace StudentBookApp.Models.Extended
{
    public class Crypto
    {
        public static string getHash(string input)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input))); //this hashe's the password for the user and returns the value to them 
        }
    }
}