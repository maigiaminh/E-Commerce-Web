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