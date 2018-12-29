using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogModel;
using MvcBlog.Filters;
using MvcBlog.Methods;
using MvcBlog.Models;

namespace MvcBlog.Controllers.Admin
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                ViewBag.Comments = db.Comments.ToList();
                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }
        [Filters.Authorize]
        public ActionResult Sort(string sortOrder)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                List<Comment> comments = db.Comments.ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        comments = new List<Comment>(comments.OrderByDescending(u => u.CommentId));
                        break;
                    case "content_desc":
                        comments = new List<Comment>(comments.OrderByDescending(u => u.Content));
                        break;
                    case "userId_desc":
                        comments = new List<Comment>(comments.OrderByDescending(u => u.UserId));
                        break;
                    case "postId_desc":
                        comments = new List<Comment>(comments.OrderByDescending(u => u.PostId));
                        break;
                    case "date_desc":
                        comments = new List<Comment>(comments.OrderByDescending(u => u.DateCreated));
                        break;
                    default:
                        comments = new List<Comment>(comments.OrderBy(u => u.CommentId));
                        break;
                }
                ViewBag.Comments = comments;
                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }
        [Filters.Authorize]
        public ActionResult Search(string searchVal,int filterVal=0)
        {
            using (var db = new BlogDbContext())
            {
                List<Comment> comments = db.Comments.ToList();
                switch (filterVal)
                {
                    case 1:
                        comments = comments.Where(c =>
                            c.CommentId.ToString().Contains(searchVal)).ToList();
                        break;
                    case 2:
                        comments = comments.Where(c =>
                            c.Content.Contains(searchVal)).ToList();
                        break;
                    case 3:
                        comments = comments.Where(c =>
                            c.UserId.ToString().Contains(searchVal)).ToList();
                        break;
                    case 4:
                        comments = comments.Where(c =>
                            c.PostId.ToString().Contains(searchVal)).ToList();
                        break;
                    case 5:
                        comments = comments.Where(c =>
                            c.DateCreated.Date.ToString(CultureInfo.CurrentCulture).Contains(searchVal)).ToList();
                        break;
                    default:
                        comments = comments.Where(c =>
                            c.CommentId.ToString().Contains(searchVal) || c.Content.Contains(searchVal) ||
                            c.DateCreated.ToString(CultureInfo.CurrentCulture).Contains(searchVal) || c.PostId.ToString().Contains(searchVal) ||
                            c.UserId.ToString().Contains(searchVal)).ToList();

                        break;
                }
                comments = comments.Where(c =>
                    c.CommentId.ToString().Contains(searchVal) || c.Content.Contains(searchVal) ||
                    c.DateCreated.ToString(CultureInfo.CurrentCulture).Contains(searchVal) || c.PostId.ToString().Contains(searchVal) ||
                    c.UserId.ToString().Contains(searchVal)).ToList();
                ViewBag.Comments = comments;
                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }
        [AdminAccess]
        public ActionResult OpenAddComment()
        {
                return View("~/Views/Admin/AddComment.cshtml");
        }
        [Filters.Authorize]
        public void AddComment(string content, int postId, DateTime dateCreated,string username)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                var userId = db.Users.Where(u => u.Username.Equals(username)).Select(u => u.UserId).FirstOrDefault();
                try
                {
                    Comment comment = new Comment();
                    comment.Content = content;
                    comment.UserId = userId;
                    comment.PostId = postId;
                    comment.DateCreated = dateCreated;

                    db.Comments.Add(comment);
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    
                }
               
            }
        }

        [AdminAccess]
        public ActionResult OpenUpdateComment(int commentId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                Comment comment = db.Comments.FirstOrDefault(c => c.CommentId == commentId);
                ViewBag.Comment = comment;
                return View("~/Views/Admin/AddComment.cshtml");
            }
        }
        [Filters.Authorize]
        public ActionResult UpdateComment(int commentId, string content, int userId, int postId, DateTime dateCreated)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                try
                {
                    Comment comment = db.Comments.FirstOrDefault(u => u.CommentId == commentId);
                    if (comment != null)
                    {
                        comment.Content = content;
                        comment.UserId = userId;
                        comment.PostId = postId;
                        comment.DateCreated = dateCreated;

                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                   
                }
                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }
        [Filters.Authorize]
        public void DeleteComment(int commentId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                try
                {
                    Comment comment = db.Comments.FirstOrDefault(p => p.CommentId == commentId);
                    if (comment != null)
                    {
                        db.Comments.Remove(comment);
                    }
                }
                catch (Exception e)
                {
                   
                }

                db.SaveChanges();
            }
        }

        public ActionResult UserAddComment(string content, int postId, DateTime dateCreated)
        {
            string username = User.Identity.Name;
            AddComment(content, postId, dateCreated, username);
            return RedirectToAction("ShowPost", "MainPage", new {postId = postId});
        }
        public ActionResult UserDeleteComment(int postId,int commentId)
        {
            string username = User.Identity.Name;
            var userId = GetMethods.GetUserId(username);
            if (CheckMethods.IsUsersComment(userId, commentId))
            {
                DeleteComment(commentId);
                return RedirectToAction("ShowPost", "MainPage", new { postId = postId });
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        public ActionResult UserUpdateComment(int postId, int commentId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                string username = User.Identity.Name;
                var userId = GetMethods.GetUserId(username);
                if (!CheckMethods.IsUsersComment(userId, commentId))
                {
                    return RedirectToAction("AccessDenied", "Error");
                }
                string commentToUpdate = db.Comments.Where(c => c.CommentId == commentId).Select(c=>c.Content).FirstOrDefault();
                ViewBag.CommentToUpdate = commentToUpdate;
                var post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                var user = db.Users.FirstOrDefault(u => u.UserId == post.UserId);
                var comments = db.Comments.Where(c => c.PostId == postId).ToList();
                comments = GetMethods.GetInitializedUserList(comments);
                ViewBag.User = user;

                ViewBag.Comments = comments;
                ViewData["Layout"] = GetMethods.GetLayout(User.Identity.Name);
                return View("~/Views/Main/ShowPost.cshtml",post);
            }
        }
    }
}