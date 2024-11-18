using E_Commerce_Web.Models.Contact;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace E_Commerce_Web.Utilities
{
    public class EmailHelper
    {
        private static readonly string SmtpHost = ConfigurationManager.AppSettings["SMTPHost"];
        private static readonly int SmtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
        private static readonly string SmtpUser = ConfigurationManager.AppSettings["SMTPUser"];
        private static readonly string SmtpAdmin = ConfigurationManager.AppSettings["SMTPAdmin"];

        private static readonly string SmtpPassword = ConfigurationManager.AppSettings["SMTPPassword"];

        public static void SendEmailConfirmation(string recipientEmail, string confirmationLink)
        {
            Debug.WriteLine("Email đang gửi");
            var message = new MailMessage();
            message.From = new MailAddress(SmtpUser);
            message.To.Add(new MailAddress(recipientEmail));
            message.Subject = "Confirm your email";
            message.Body = $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.Host = SmtpHost;
                smtp.Port = SmtpPort;
                smtp.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(message);
                    Debug.WriteLine("Email đã gửi");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Email không thể gửi: " + ex.Message);
                }
            }
        }

        public static bool SendEmailSignUpNewLetters(string recipientEmail)
        {
            Debug.WriteLine("Email đang gửi");
            var message = new MailMessage();
            message.From = new MailAddress(SmtpUser);
            message.To.Add(new MailAddress(recipientEmail));
            message.Subject = "Newsletter Subscription Confirmation";
            message.Body = @"
            <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            margin: 0;
                            padding: 0;
                        }
                        .container {
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            border: 1px solid #ddd;
                            border-radius: 10px;
                            background-color: #f9f9f9;
                        }
                        .header {
                            text-align: center;
                            color: #007BFF;
                            margin-bottom: 20px;
                        }
                        .content {
                            margin-bottom: 20px;
                        }
                        .coupon {
                            background-color: #f0f8ff;
                            padding: 10px;
                            border-radius: 5px;
                            border: 1px dashed #007BFF;
                            margin-bottom: 10px;
                            font-weight: bold;
                            text-align: center;
                        }
                        .footer {
                            text-align: center;
                            font-size: 0.9em;
                            color: #666;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2 class='header'>Thank You for Subscribing!</h2>
                        <p class='content'>Dear Subscriber,</p>
                        <p class='content'>
                            Thank you for subscribing to our newsletter! We will keep you updated with our latest news and exclusive offers.
                        </p>
                        <h3>Here are some exclusive coupon codes for you:</h3>
                        <div class='coupon'>SUMMER10</div>
                        <div class='coupon'>NEWYEAR20</div>
                        <div class='coupon'>WINTER15</div>
                        <div class='coupon'>SPRING5</div>
                        <div class='coupon'>CLEARANCE30</div>
                        <div class='coupon'>FREESHIP</div>
                        <p class='content'>Enjoy shopping with us!</p>
                        <p class='footer'>This email was sent from our newsletter subscription service. If you did not subscribe, please disregard this email.</p>
                    </div>
                </body>
            </html>";

            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.Host = SmtpHost;
                smtp.Port = SmtpPort;
                smtp.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(message);
                    Debug.WriteLine("Email đã gửi");
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Email không thể gửi: " + ex.Message);
                    return false;
                }
            }
        }
        
        public static void SendEmailFromUser(ContactFormViewModel model)
        {
            Debug.WriteLine("Email đang gửi");
            var message = new MailMessage();
            message.From = new MailAddress(SmtpUser);
            message.To.Add(new MailAddress(SmtpAdmin));
            message.Subject = $"New Contact Message from {model.Name}";
            message.Body = $@"
                <h3>Contact Details:</h3>
                <p><strong>Name:</strong> {model.Name}</p>
                <p><strong>Email:</strong> {model.Email}</p>
                <p><strong>Subject:</strong> {model.Subject}</p>
                <p><strong>Message:</strong><br>{model.Message}</p>";
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.Host = SmtpHost;
                smtp.Port = SmtpPort;
                smtp.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(message);
                    Debug.WriteLine("Email đã gửi");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Email không thể gửi: " + ex.Message);
                }
            }
        }
    }
}