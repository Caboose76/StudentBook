//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentBookApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual Post Post { get; set; } //this is foreign key class in the database and links it to the post table
    }
}
