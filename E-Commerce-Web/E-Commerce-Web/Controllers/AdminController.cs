using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly EcommerceContext _context;

        public AdminController()
        {
            _context = new EcommerceContext(); 
        }
        public ActionResult Index()
        {

            return View("~/Views/Admin/Dashboard.cshtml");
        }

        public ActionResult Dashboard()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }

        public ActionResult Products()
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            List<Product> products = _context.Products.ToList();

            return View("~/Views/Admin/Products/Products.cshtml", products);
        }

        public ActionResult Orders()
        {
            return View("~/Views/Admin/Orders.cshtml");
        }

        public ActionResult Coupons()
        {
            return View("~/Views/Admin/Coupons.cshtml");
        }

        public ActionResult Users()
        {
            return View("~/Views/Admin/Users.cshtml");
        }

        public ActionResult Blogs()
        {
            return View("~/Views/Admin/Blogs.cshtml");
        }

    }
}