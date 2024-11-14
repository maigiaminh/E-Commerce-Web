using System;
using System.ComponentModel.DataAnnotations;


namespace E_Commerce_Web.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required, MaxLength(100)]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}