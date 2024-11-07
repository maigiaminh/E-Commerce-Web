using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace E_Commerce_Web.Utilities
{
    public class EmailHelper
    {
        public static void SendEmailConfirmation(string recipientEmail, string confirmationLink)
        {
            Debug.WriteLine("Email đang gửi");
            var message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["SMTPUser"]);
            message.To.Add(new MailAddress(recipientEmail));
            message.Subject = "Confirm your email";
            message.Body = $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.Host = ConfigurationManager.AppSettings["SMTPHost"];
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtp.Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["SMTPUser"],
                    ConfigurationManager.AppSettings["SMTPPassword"]);
                smtp.EnableSsl = true;
                Debug.WriteLine("gửi"); 
                Debug.WriteLine(ConfigurationManager.AppSettings["SMTPHost"]);
                Debug.WriteLine(ConfigurationManager.AppSettings["SMTPPort"]);
                Debug.WriteLine(ConfigurationManager.AppSettings["SMTPUser"]);
                Debug.WriteLine(ConfigurationManager.AppSettings["SMTPPassword"]);


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