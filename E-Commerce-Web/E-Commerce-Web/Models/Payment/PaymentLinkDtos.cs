using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Models.Payment
{
    public class PaymentLinkDtos
    {
        public string PaymentId { get; set; } = string.Empty;
        public string PaymentUrl { get; set; } = string.Empty;
    }
}