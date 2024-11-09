using E_Commerce_Web.Models;
using E_Commerce_Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace E_Commerce_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly EcommerceContext _context;
        public UserController()
        {
            _context = new EcommerceContext();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                if (!user.Active)
                {
                    ViewBag.ErrorMessage = "Your account is not activated. Please check your email.";
                    ViewBag.Email = email;
                    ViewBag.Password = password;
                    return View();
                }
                Session["UserID"] = user.UserID;
                Session["Avatar"] = user.Avatar;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                ViewBag.Email = email;
                ViewBag.Password = password;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string email, string password, string confirmPassword)
        {

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Email already exists. Please try again!";
                ViewBag.Username = username;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }
            
            else if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match. Please try again!";
                ViewBag.Username = username;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }

            else if (password.Length < 8)
            {
                ViewBag.ErrorMessage = "Passwords must greater or equals 8 characters. Please try again!";
                ViewBag.Username = username;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }

            var emailConfirmationToken = Guid.NewGuid().ToString();

            var newUser = new User
            {
                FullName = username,
                Email = email,
                PasswordHash = PasswordHasher.HashPassword(password),
                CreatedAt = DateTime.Now,
                Avatar = "default_avt.jpg",
                Active = false,
                VerificationToken = emailConfirmationToken,
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var confirmationLink = Url.Action("ConfirmEmail", "User", new { token = emailConfirmationToken }, Request.Url.Scheme);
            EmailHelper.SendEmailConfirmation(email, confirmationLink);
            ViewBag.SuccessMessage = "Registration successful, please verify your account via the email sent to you.";
            return View();
        }

        public ActionResult ConfirmEmail(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.VerificationToken == token);
            if (user == null)
            {
                return HttpNotFound("Invalid token.");
            }

            user.Active = true;
            user.VerificationToken = null;
            _context.SaveChanges();

            ViewBag.Message = "Your email has been confirmed!";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Login", "User");
        }
        [HttpGet]

        public ActionResult Profile()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int userID = Convert.ToInt32(Session["UserID"]);
            var user = _context.Users.FirstOrDefault(u => u.UserID == userID);

            return View("Profile", user);
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPass, string newPass, string confirmPass)
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            var user = _context.Users.FirstOrDefault(u => u.UserID == userID);

            if (newPass != confirmPass)
            {
                ViewBag.ErrorMessage = "Passwords do not match. Please try again!";
                ViewBag.OldPassword = oldPass;
                ViewBag.NewPassword = newPass;
                ViewBag.ConfirmPassword = confirmPass;
                return View("Profile", user);
            }

            else if (newPass.Length < 8)
            {
                ViewBag.ErrorMessage = "Passwords must greater or equals 8 characters. Please try again!";
                ViewBag.OldPassword = oldPass;
                ViewBag.NewPassword = newPass;
                ViewBag.ConfirmPassword = confirmPass;
                return View("Profile", user);
            }

            if (user != null)
            {
                if(!PasswordHasher.VerifyPassword(oldPass, user.PasswordHash))
                {
                    ViewBag.ErrorMessage = "Incorrect old password. Please try again!";
                    ViewBag.OldPassword = oldPass;
                    ViewBag.NewPassword = newPass;
                    ViewBag.ConfirmPassword = confirmPass;
                    return View("Profile", user);
                }
                else
                {
                    ViewBag.SuccessMessage = "Change password successfully!";
                    user.PasswordHash = PasswordHasher.HashPassword(newPass);
                    _context.SaveChanges();

                }
            }

            return View("Profile", user);
        }

        [HttpPost]
        public ActionResult UpdateInformation(string fullname, string phone, string email, string province, string district, string address, HttpPostedFileBase avatar)
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            var user = _context.Users.FirstOrDefault(u => u.UserID == userID);

            if (phone.Length < 10)
            {
                ViewBag.ErrorMessage = "Phone must greater or equals 10 number. Please try again!";
                return View("Profile", user);
            }

            if (user != null)
            {
                user.FullName = fullname;
                user.Phone = phone;
                user.Province = province;
                user.District = district;
                user.Address = address;

                if (avatar != null && avatar.ContentLength > 0)
                {
                    string path = Server.MapPath("~/Content/img/avt/");

                    string fileName = $"{userID}{System.IO.Path.GetExtension(avatar.FileName)}";
                    string fullPath = System.IO.Path.Combine(path, fileName);

                    avatar.SaveAs(fullPath);

                    user.Avatar = fileName;

                    Session["Avatar"] = user.Avatar;
                }
                ViewBag.SuccessMessage = "Update Information successfully!";
                _context.SaveChanges();
            }

            return View("Profile", user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}