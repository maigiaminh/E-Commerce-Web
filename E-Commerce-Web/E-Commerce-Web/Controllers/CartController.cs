using E_Commerce_Web.Migrations;
using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

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
        
        private decimal GetShippingCost(string address)
        {
            return address.Contains("Ho Chi Minh") ? 0 : 2;
        }
        

        [HttpPost]
        public ActionResult Checkout()
        {
            if (Session["UserID"] != null)
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                var user = _context.Users.FirstOrDefault(u => u.UserID == userID);

                ViewBag.Name = user.FullName;
                ViewBag.Email = user.Email;
                ViewBag.Phone = user.Phone;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

            var cartItems = GetCart();
            decimal cartSubtotal = cartItems.Sum(item => item.Subtotal);

            ViewBag.CartItems = cartItems;
            ViewBag.Total = cartSubtotal;

            return View();
        }

        [HttpPost]
        public ActionResult Payment(string name, string recipientPhone, string note, string fullAddress, string couponCode = "", bool removeCoupon = false)
        {
            if (Session["UserID"] != null)
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                var user = _context.Users.FirstOrDefault(u => u.UserID == userID);

                ViewBag.Name = user.FullName;
                ViewBag.Email = user.Email;
                ViewBag.Phone = user.Phone;
            }

            if (removeCoupon)
            {
                Session.Remove("Discount");
                Session.Remove("CouponCode");
            }

            var cart = GetCart();
            decimal cartSubtotal = cart.Sum(item => item.Subtotal);
            decimal shipping = GetShippingCost(fullAddress);
            decimal discount = 0;
            decimal total = 0;

            if(couponCode != "" && removeCoupon == false)
            {
                var coupon = _context.Coupons
                                        .FirstOrDefault(c =>
                                        c.Code.ToUpper() == couponCode.ToUpper() &&
                                        c.IsActive &&
                                        (c.ExpirationDate == null || c.ExpirationDate >= DateTime.Now));

                if (coupon != null)
                {
                    if (coupon.Code == "FREESHIP")
                    {
                        shipping = 0;
                    }
                    discount = coupon.Discount * cartSubtotal / 100;
                    Session["Discount"] = coupon.Discount;
                    Session["CouponCode"] = coupon.Code;
                }
                else
                {
                    Session.Remove("Discount");
                    Session.Remove("CouponCode");
                    ViewBag.Message = "Coupon is invalid or expired.";
                }
            }

            total = cartSubtotal + shipping - discount;

            ViewBag.RecipientName = name;
            ViewBag.RecipientPhone = recipientPhone;
            ViewBag.Note = note;
            ViewBag.Address = fullAddress;

            ViewBag.CartSubtotal = cartSubtotal;
            ViewBag.Shipping = shipping;
            ViewBag.Discount = discount;
            ViewBag.Total = total;

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