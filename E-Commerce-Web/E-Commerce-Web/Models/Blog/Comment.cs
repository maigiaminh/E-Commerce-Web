using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}