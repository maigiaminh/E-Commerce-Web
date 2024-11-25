using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce_Web.Models;

namespace E_Commerce_Web.Controllers.AdminController
{
    public class AdminOrdersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: AdminOrders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: AdminOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: AdminOrders/Create
        public ActionResult Create()
        {
            // List of status options
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Paid", Text = "Paid" },
                new { Value = "Delivering", Text = "Delivering" },
                new { Value = "Delivered", Text = "Delivered" },
                new { Value = "Confirmed", Text = "Confirmed" }
            }, "Value", "Text");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");
            return View();
        }

        // POST: AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserID,OrderDate,Discount,ShippingFee,TotalAmount,Subtotal,PaymentMethod,Status,RecipientName,RecipientPhone,RecipientAddress,Note")] Order order)
        {
            if (ModelState.IsValid)
            {
                // Add the new order to the database
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If validation fails, pass the status list again
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Paid", Text = "Paid" },
                new { Value = "Delivering", Text = "Delivering" },
                new { Value = "Delivered", Text = "Delivered" },
                new { Value = "Confirmed", Text = "Confirmed" }
            }, "Value", "Text", order.Status);

            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", order.UserID);
            return View(order);
        }

        // GET: AdminOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            // List of status options
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Paid", Text = "Paid" },
                new { Value = "Delivering", Text = "Delivering" },
                new { Value = "Delivered", Text = "Delivered" },
                new { Value = "Confirmed", Text = "Confirmed" }
            }, "Value", "Text", order.Status);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", order.UserID);
            return View(order);
        }

        // POST: AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserID,OrderDate,Discount,ShippingFee,TotalAmount,Subtotal,PaymentMethod,Status,RecipientName,RecipientPhone,RecipientAddress,Note")] Order order)
        {
            if (ModelState.IsValid)
            {
                // Check if the order exists
                var existingOrder = db.Orders.Find(order.OrderID);
                if (existingOrder != null)
                {
                    // Update the order status
                    existingOrder.Status = order.Status;  // Update the status from the form

                    // Update other fields as necessary
                    existingOrder.OrderDate = order.OrderDate;
                    existingOrder.Discount = order.Discount;
                    existingOrder.ShippingFee = order.ShippingFee;
                    existingOrder.TotalAmount = order.TotalAmount;
                    existingOrder.Subtotal = order.Subtotal;
                    existingOrder.PaymentMethod = order.PaymentMethod;
                    existingOrder.RecipientName = order.RecipientName;
                    existingOrder.RecipientPhone = order.RecipientPhone;
                    existingOrder.RecipientAddress = order.RecipientAddress;
                    existingOrder.Note = order.Note;

                    db.Entry(existingOrder).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            // If validation fails, pass the status list again
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Paid", Text = "Paid" },
                new { Value = "Delivering", Text = "Delivering" },
                new { Value = "Delivered", Text = "Delivered" },
                new { Value = "Confirmed", Text = "Confirmed" }
            }, "Value", "Text", order.Status);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", order.UserID);
            return View(order);
        }

        // GET: AdminOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
