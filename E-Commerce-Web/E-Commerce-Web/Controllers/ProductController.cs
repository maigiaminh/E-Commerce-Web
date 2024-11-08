using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace E_Commerce_Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceContext _context;

        public ProductController()
        {
            _context = new EcommerceContext();
        }
        public ActionResult Shop(int page = 1)
        {
            int pageSize = 8; 
            int totalProducts = _context.Products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var products = _context.Products
                                    .OrderBy(p => p.ProductID) 
                                    .Skip((page - 1) * pageSize) 
                                    .Take(pageSize) 
                                    .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(products);
        }

        public ActionResult ProductDetails(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return HttpNotFound("Product not found.");
            }

            var relatedProducts = _context.Products
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != productId)
                .OrderByDescending(p => p.CreatedAt)
                .Take(4)
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }
    }
}