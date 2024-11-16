using E_Commerce_Web.Models;
using iText.StyledXmlParser.Jsoup.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly EcommerceContext _context;
        public BlogController()
        {
            _context = new EcommerceContext();
        }

        public ActionResult Index(int page = 1)
        {
            var blogs = _context.Blogs.OrderBy(p => p.Id).ToList();
            int pageSize = 4;
            int totalBlogs = blogs.Count();
            int totalPages = (int)Math.Ceiling((double)totalBlogs / pageSize);

            var paginatedBlogs = blogs
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(paginatedBlogs);
        }

        public ActionResult BlogDetail(int id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);

            if (blog == null)
            {
                return HttpNotFound();
            }

            var recentBlogs = _context.Blogs
                .Where(b => b.Id != id)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToList();

            ViewBag.RecentBlogs = recentBlogs;
            return View(blog);
        }

        [HttpPost]
        public ActionResult AddComment(int BlogId, string Content)
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                return RedirectToAction("Detail", new { id = BlogId });
            }

            if (Session["UserID"] != null)
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                var comment = new E_Commerce_Web.Models.Comment
                {
                    BlogId = BlogId,
                    UserId = userID,
                    Content = Content,
                    CreatedAt = DateTime.Now
                };
                _context.Comments.Add(comment);
                _context.SaveChanges();
            }

            return RedirectToAction("Detail", new { id = BlogId });
        }
    }
}