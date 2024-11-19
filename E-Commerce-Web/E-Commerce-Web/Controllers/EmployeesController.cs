using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce_Web.Models;
using E_Commerce_Web.Utilities;

namespace E_Commerce_Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EcommerceContext db;

        public EmployeesController()
        {
            db = new EcommerceContext();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["EmployeeID"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var employee = db.Employees.FirstOrDefault(u => u.Email == email);
            if (employee != null && HashHelper.VerifyPassword(password, employee.PasswordHash))
            {
                Session["EmployeeID"] = employee.EmployeeID;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                ViewBag.Email = email;
                ViewBag.Password = password;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["EmployeeID"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string email, string password, string confirmPassword)
        {

            var existingEmployee = db.Employees.FirstOrDefault(u => u.Email == email);
            if (existingEmployee != null)
            {
                ViewBag.ErrorMessage = "Email already exists. Please try again!";
                ViewBag.EmployeeName = username;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }

            else if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match. Please try again!";
                ViewBag.EmployeeName = username;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }

            else if (password.Length < 8)
            {
                ViewBag.ErrorMessage = "Passwords must greater or equals 8 characters. Please try again!";
                ViewBag.EmployeeName = username;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.ConfirmPassword = confirmPassword;
                return View();
            }

            var newEmployee = new Employee
            {
                FullName = username,
                Email = email,
                PasswordHash = HashHelper.HashPassword(password),
                CreatedAt = DateTime.Now,
            };

            db.Employees.Add(newEmployee);
            db.SaveChanges();

            ViewBag.SuccessMessage = "Registration successful";
            return View();
        }

        /*// GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FullName,Email,PasswordHash,Phone,Role,CreatedAt")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FullName,Email,PasswordHash,Phone,Role,CreatedAt")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        public ActionResult ConfirmEmail(string token)
        {
            var employee = db.Users.FirstOrDefault(u => u.VerificationToken == token);
            if (employee == null)
            {
                return HttpNotFound("Invalid token.");
            }

            employee.Active = true;
            employee.VerificationToken = null;
            db.SaveChanges();

            ViewBag.Message = "Your email has been confirmed!";
            return View();
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
