using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Utilities.Momo.Config
{
    public class MomoConfig
    {
        public static string ConfigName => "Momo";
        public string PartnerCode { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string IpnUrl { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string PaymentUrl { get; set; } = string.Empty;
    }
}