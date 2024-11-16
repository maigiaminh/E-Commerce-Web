using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Web.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; } 
        public string Title { get; set; } 
        public string Content { get; set; } 
        public string ImageUrl { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Author { get; set; } 

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}