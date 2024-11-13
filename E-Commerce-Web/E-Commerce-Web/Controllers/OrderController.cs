using E_Commerce_Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly EcommerceContext _context;
        public OrderController()
        {
            _context = new EcommerceContext();
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            else if (order.UserID != Convert.ToInt32(Session["UserID"])) {
                return HttpNotFound();
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult OrderCreate(string paymentMethod, string RecipientName, string RecipientPhone, string Address, string Note, decimal Discount, decimal Shipping, decimal Total, decimal Subtotal)
        {
            Debug.WriteLine("OK");
            if (string.IsNullOrEmpty(paymentMethod))
            {
                ModelState.AddModelError("", "Please select a payment method.");
                return RedirectToAction("Index", "Cart");
            }

            List<CartItem> cartItems = Session["Cart"] as List<CartItem>;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var newOrder = new Order
                    {
                        UserID = Convert.ToInt32(Session["UserID"]),
                        OrderDate = DateTime.Now,
                        Discount = Discount,
                        ShippingFee = Shipping,
                        TotalAmount = Total,
                        Subtotal = Subtotal,
                        PaymentMethod = paymentMethod,
                        Status = paymentMethod == "Cash On Delivery (COD)" ? "Delivering" : "Pending",
                        RecipientName = RecipientName,
                        RecipientPhone = RecipientPhone,
                        RecipientAddress = Address,
                        Note = Note
                    };

                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();

                    foreach (var item in cartItems)
                    {
                        var product = _context.Products.Find(item.ProductId);
                        var productSize = _context.ProductSizes.FirstOrDefault(ps => ps.ProductID == item.ProductId && ps.SizeID == item.SizeID);

                        if (product != null && product.Stock >= item.Quantity)
                        {
                            product.Stock -= item.Quantity;
                        }
                        else
                        {
                            throw new Exception("Insufficient stock for product: " + item.ProductName);
                        }

                        if (productSize != null && productSize.Stock >= item.Quantity)
                        {
                            productSize.Stock -= item.Quantity;
                        }
                        else
                        {
                            throw new Exception("Insufficient stock for product size: " + item.ProductName + " - " + item.Size);
                        }

                        var orderDetail = new OrderDetail
                        {
                            OrderID = newOrder.OrderID,
                            ProductID = item.ProductId,
                            SizeID = item.SizeID,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price
                        };

                        _context.OrderDetails.Add(orderDetail);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    return RedirectToAction("OrderDetails", new { id = newOrder.OrderID });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Error processing your order: " + ex.Message);
                    Debug.WriteLine("error" + ex.Message);

                    return RedirectToAction("Index", "Cart");
                }
            }
        }
    }
}