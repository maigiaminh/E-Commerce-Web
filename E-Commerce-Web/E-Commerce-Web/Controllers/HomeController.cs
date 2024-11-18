using E_Commerce_Web.Models;
using E_Commerce_Web.Models.Contact;
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
            var featuredProducts = _context.Products.Take(8).ToList(); 
            var newArrivals = _context.Products
                                      .OrderByDescending(p => p.CreatedAt)
                                      .Take(8)
                                      .ToList();

            ViewBag.FeaturedProducts = featuredProducts;
            ViewBag.NewArrivals = newArrivals; 
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

        public ActionResult ProductDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubmitContactForm(ContactFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmailHelper.SendEmailFromUser(model);

                    return Json(new { success = true, message = "Your message has been sent successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
                }
            }

            return Json(new { success = false, message = "Please fill all the required fields." });
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