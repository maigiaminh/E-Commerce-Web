using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce_Web.Utilities;

namespace E_Commerce_Web.Controllers
{
    public class NewsletterController : Controller
    {
        [HttpPost]
        public JsonResult SignUpAjax(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Json(new { success = false, message = "Email cannot be empty." });
            }

            bool emailSent = EmailHelper.SendEmailSignUpNewLetters(email);

            if (emailSent)
            {
                return Json(new { success = true, message = "Subscription successful! A confirmation email has been sent." });
            }
            else
            {
                return Json(new { success = false, message = "An error occurred while sending the confirmation email. Please try again later." });
            }
        }

    }
}