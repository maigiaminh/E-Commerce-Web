using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
            decimal cartSubtotal = cart.Sum(item => item.Subtotal);
            decimal shipping = 0;
            decimal discount = 0;
            decimal total = 0;


            if (Session["UserID"] != null)
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                var user = _context.Users.FirstOrDefault(u => u.UserID == userID);

                if (user.City != "Ho Chi Minh" && cartSubtotal > 0)
                {
                    shipping = 30;
                }
            }
            
            total = cartSubtotal + shipping - discount;

            ViewBag.CartSubtotal = cartSubtotal;
            ViewBag.Shipping = shipping;
            ViewBag.Discount = discount;
            ViewBag.Total = total;

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

        [HttpPost]
        public ActionResult RemoveFromCart(int productId, string size)
        {
            var cart = GetCart();
            var itemToRemove = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCart(cart);
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}