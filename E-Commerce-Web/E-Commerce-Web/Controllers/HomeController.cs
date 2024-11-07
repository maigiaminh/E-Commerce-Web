using E_Commerce_Web.Models;
using E_Commerce_Web.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceContext _context;
        public HomeController()
        {
            _context = new EcommerceContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Shop()
        {
            return View();
        }

        public ActionResult ProductDetails()
        {
            return View();
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
                Session["UserID"] = user.UserID;
                Session["Username"] = user.FullName;
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

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email || u.FullName == username);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Username or Email already exists.";
                ViewBag.Username = username; 
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                ViewBag.Username = username;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }

            var newUser = new User
            {
                FullName = username,
                Email = email,
                PasswordHash = PasswordHasher.HashPassword(password),
                CreatedAt = DateTime.Now,
                Avatar = "default_avt.jpg" 
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login", "Home");
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