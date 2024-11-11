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
            
            if(cart.Count > 0)
            {
                //shipping = GetShippingCost();

                if (Session["Discount"] != null)
                {
                    discount = (decimal)Session["Discount"] * cartSubtotal / 100;

                    if (Session["CouponCode"].ToString().ToUpper() == "FREESHIP")
                        shipping = 0;
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

        [HttpPost]
        public ActionResult ApplyCoupon(string couponCode)
        {
            var cartItems = GetCart();
            decimal cartSubtotal = cartItems.Sum(item => item.Subtotal);
            decimal shipping = 0;
            decimal discount = 0;

            if(cartItems.Count > 0)
            {
                var coupon = _context.Coupons
                        .FirstOrDefault(c =>
                        c.Code.ToUpper() == couponCode.ToUpper() &&
                        c.IsActive &&
                        (c.ExpirationDate == null || c.ExpirationDate >= DateTime.Now));

                //shipping = GetShippingCost();

                if (coupon != null)
                {
                    if(coupon.Code == "FREESHIP")
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
            
            decimal total = cartSubtotal + shipping - discount;

            ViewBag.CartSubtotal = cartSubtotal;
            ViewBag.Shipping = shipping;
            ViewBag.Discount = discount;
            ViewBag.Total = total;

            return View("Index", cartItems);
        }

        public ActionResult RemoveCoupon()
        {
            Session.Remove("Discount");
            Session.Remove("CouponCode");
            return RedirectToAction("Index");
        }

        /*
        private decimal GetShippingCost()
        {
            if (Session["UserID"] != null)
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                var user = _context.Users.FirstOrDefault(u => u.UserID == userID);

                if (user.City != "Ho Chi Minh")
                {
                    return 30;
                }
            }

            return 0;
        }
        */

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

        public ActionResult Payment()
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