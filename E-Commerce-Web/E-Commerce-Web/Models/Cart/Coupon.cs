using System;
using System.ComponentModel.DataAnnotations;


namespace E_Commerce_Web.Models
{
    public class Coupon
    {
        [Key]
        public int CouponID { get; set; }
        [Required, MaxLength(50)]
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}