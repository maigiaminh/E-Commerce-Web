using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly EcommerceContext _context;
        public BlogController()
        {
            _context = new EcommerceContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BlogDetail(int id = 0)
        {
            //var blog = _context.Include("Comments").FirstOrDefault(b => b.Id == id);

           // if (blog == null)
            //{
                //return HttpNotFound();
           // }

            return View();
        }
    }
}