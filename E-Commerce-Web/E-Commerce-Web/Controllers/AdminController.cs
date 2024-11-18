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
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManagementProducts()
        {
            return View();
        }
    }
}