using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace E_Commerce_Web.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Subtotal { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } = "Pending";
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public string Note { get; set; }
        public virtual User User { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}