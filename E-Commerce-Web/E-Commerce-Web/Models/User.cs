using System;
using System.ComponentModel.DataAnnotations;


namespace E_Commerce_Web.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public bool Active { get; set; }
        public string VerificationToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Avatar { get; set; }
    }
}