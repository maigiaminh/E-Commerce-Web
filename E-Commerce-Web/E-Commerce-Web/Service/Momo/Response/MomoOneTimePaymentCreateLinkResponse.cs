using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Service.Momo.Response
{
    public class MomoOneTimePaymentCreateLinkResponse
    {
        public string partnerCode { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public long amount { get; set; }
        public long responseTime { get; set; }
        public string message { get; set; } = string.Empty;
        public string resultCode { get; set; } = string.Empty;
        public string payUrl { get; set; } = string.Empty;
        public string deeplink { get; set; } = string.Empty;
        public string qrCodeUrl { get; set; } = string.Empty;
    }
}