using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ActionResult Shop(int page = 1, string sortOption = "default", int? categoryId = null)
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;

            var products = _context.Products.OrderBy(p => p.ProductID).ToList();

            if (categoryId != null)
            {
                products = products.Where(p => p.CategoryID == categoryId).ToList();
            }

            switch (sortOption)
            {
                case "default":
                    break;
                case "priceAsc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "priceDesc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "nameAsc":
                    products = products.OrderBy(p => p.ProductName).ToList();
                    break;
                case "nameDesc":
                    products = products.OrderByDescending(p => p.ProductName).ToList();
                    break;
                case "dateAsc":
                    products = products.OrderBy(p => p.CreatedAt).ToList();
                    break;
                case "dateDesc":
                    products = products.OrderByDescending(p => p.CreatedAt).ToList();
                    break;

                default:
                    break;
            }

            int pageSize = 8; 
            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var paginatedProducts = products
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.SortOption = sortOption;

            return View(paginatedProducts);
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