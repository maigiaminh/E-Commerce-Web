using System;
using System.ComponentModel.DataAnnotations;


namespace E_Commerce_Web.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } = "Staff";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}