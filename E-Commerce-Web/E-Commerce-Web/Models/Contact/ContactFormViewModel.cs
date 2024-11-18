using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Models.Contact
{
    public class ContactFormViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}