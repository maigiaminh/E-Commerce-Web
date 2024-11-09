using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web.Controllers
{
    public class CartController : Controller
    {
        private readonly EcommerceContext _context;

        public CartController(){
            _context = new EcommerceContext();
        }

        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity, string size)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return HttpNotFound();

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductID,
                    ProductName = product.ProductName,
                    Size = size,
                    Price = product.Price,
                    Quantity = quantity,
                    ImagePath = product.ImagePath
                });
            }

            SaveCart(cart);

            return RedirectToAction("Index", "Cart");
        }

        private List<CartItem> GetCart()
        {
            return Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            Session["Cart"] = cart;
        }
    }
}