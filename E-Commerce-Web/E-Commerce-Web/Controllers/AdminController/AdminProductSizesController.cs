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
    public class AdminProductSizesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: AdminProductSizes
        public ActionResult Index()
        {
            var productSizes = db.ProductSizes.Include(p => p.Product).Include(p => p.Size);
            return View(productSizes.ToList());
        }

        // GET: AdminProductSizes/Details/5

        public ActionResult Details(int? productId, int? sizeId)
        {
            if (productId == null || sizeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSize productSize = db.ProductSizes.Find(productId, sizeId);
            if (productSize == null)
            {
                return HttpNotFound();
            }
            return View(productSize);
        }


        // GET: AdminProductSizes/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "Name");
            return View();
        }

        // POST: AdminProductSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,SizeID,Stock")] ProductSize productSize)
        {
            if (ModelState.IsValid)
            {
                db.ProductSizes.Add(productSize);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productSize.ProductID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "Name", productSize.SizeID);
            return View(productSize);
        }

        // GET: AdminProductSizes/Edit/5
        public ActionResult Edit(int? productId, int? sizeId)
        {
            if (productId == null || sizeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSize productSize = db.ProductSizes.Find(productId, sizeId);
            if (productSize == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productSize.ProductID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "Name", productSize.SizeID);
            return View(productSize);
        }

        // POST: AdminProductSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,SizeID,Stock")] ProductSize productSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productSize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productSize.ProductID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "Name", productSize.SizeID);
            return View(productSize);
        }

        // GET: AdminProductSizes/Delete/5
        public ActionResult Delete(int? productId, int? sizeId)
        {
            if (productId == null || sizeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSize productSize = db.ProductSizes.Find(productId, sizeId);
            if (productSize == null)
            {
                return HttpNotFound();
            }
            return View(productSize);
        }

        // POST: AdminProductSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int productId, int sizeId)
        {
            ProductSize productSize = db.ProductSizes.Find(productId, sizeId);
            db.ProductSizes.Remove(productSize);
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
