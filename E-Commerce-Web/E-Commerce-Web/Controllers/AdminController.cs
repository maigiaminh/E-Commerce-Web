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
        // GET: Admin
        public ActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            List<Product> products = _context.Products.ToList();
            return View(products);
        }

        public ActionResult ManagementProducts()
        {
            return View();
        }
    }
}