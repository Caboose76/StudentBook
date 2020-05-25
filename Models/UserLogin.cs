using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StudentBookApp.Models
{
    public class UserLogin
    {
        //this models code was adapted from https://www.youtube.com/watch?v=gSJFjuWFTdA&list=PLdUi92csWFpKWs_JMTaSv8_h7AfRMjJ4Z&index=14&t=1570s

        [Display(Name = "E-Mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="E-Mail Address is Required")] //this displays an error message when the input field is null
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)] //this makes sure that this is a password datatype
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")] //this displays an error when the input field is null
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe  { get; set; }

    }
}