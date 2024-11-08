using System;
using System.ComponentModel.DataAnnotations;


namespace E_Commerce_Web.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        [Required, MaxLength(100)]
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [MaxLength(255)]
        public string ImagePath { get; set; }
        public virtual Category Category { get; set; }
    }
}