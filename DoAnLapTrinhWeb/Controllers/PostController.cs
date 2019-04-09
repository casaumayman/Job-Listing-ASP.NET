using DoAnLapTrinhWeb.Models;
using DoAnLapTrinhWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;

namespace DoAnLapTrinhWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly DatabaseContext db = new DatabaseContext();
        // GET: Post
        public ActionResult Index()
        {
            var dtPosts = db.Posts.SqlQuery("SELECT * FROM Posts WHERE Posts.Id_Employer = @Id", new SqlParameter("@Id", ((Employer)Session["Employer"]).Id_Employer)).ToList();
            List<ViewPost> viewPosts = new List<ViewPost>();
            var sp = db.Specializations.SqlQuery("SELECT * FROM Specializations").ToList();
            foreach(var item in dtPosts)
            {
                ViewPost v = new ViewPost()
                {
                    Id_Post = item.Id_Post,
                    DatePost = item.DatePost,
                    Title = item.Title
                };
                foreach(var item2 in sp)
                {
                    if (item2.Id_Specialization == item.Id_Specialization)
                    {
                        v.Name_Specialization = item2.Name;
                        break;
                    }
                }
                viewPosts.Add(v);
            }
            ViewBag.List = viewPosts;
            return View();
        }
        public ActionResult Create()
        {
            List<Specialization> list = new List<Specialization>(); 
            list = db.Specializations.SqlQuery("select * from Specializations").ToList();
            ViewBag.list = list;
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            Post post = new Post()
            {
                Title = form["Title"],
                Description = form["Description"],
                Require = form["Require"],
                Benefit = form["Benefit"],
            };
            post.Salary = Convert.ToInt64(form["Salary"]);
            post.Id_Specialization = Convert.ToInt32(form["Id_Specialization"]);
            post.DatePost = new DateTime();
            post.DatePost = DateTime.Now;
            Employer employer = new Employer();
            employer = (Employer)Session["Employer"];
            post.Id_Employer = employer.Id_Employer;
            db.Posts.Add(post);
            db.SaveChanges();
            var Followings = db.Followings.Where(follow => follow.Id_Employer == employer.Id_Employer).ToList();
            MailMessage mail = new MailMessage();
            foreach (var follow in Followings)
            {
                Candidate candidate = new Candidate();
                candidate = db.Candidates.Find(follow.Id_Candidate);
                mail.Bcc.Add(candidate.Email);
            }
            //Mail Setting
            StringBuilder Body = new StringBuilder();
            Body.Append("<p>" + employer.CompanyName + " vừa đăng một tin mới!</p>");
            string url = $"http://Localhost:25951/Post/Details/{post.Id_Post}";
            string link = @"<a href=""" + url + @""">Xem chi tiết</a>";
            Body.Append(link);
            mail.From = new MailAddress("casaumayman321@gmail.com");
            mail.Subject = employer.CompanyName + " vừa đăng một tin mới!";
            mail.Body = Body.ToString();// phần thân của mail ở trên
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("casaumayman321@gmail.com", "anhyeuem123");// tài khoản Gmail của bạn
            smtp.EnableSsl = true;
            if (mail.Bcc.Count > 0) smtp.Send(mail);
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Post post = new Post();
            post = db.Posts.SqlQuery("SELECT * FROM Posts WHERE Id_Post=@id", new SqlParameter("@id", id)).SingleOrDefault();
            Employer employer = new Employer();
            employer = db.Employers.SqlQuery("SELECT * FROM Employers WHERE Id_Employer=@id", new SqlParameter("@id", post.Id_Employer)).SingleOrDefault();
            ViewBag.Err = true;
            if (Session["Candidate"] != null)
            {
                Candidate candidate = new Candidate();
                candidate = (Candidate)Session["Candidate"];
                var job_app = db.JobApplications.SqlQuery("SELECT * FROM JobApplications WHERE Id_Candidate=@candi AND Id_Employer=@employ"
                , new SqlParameter("@candi", candidate.Id_Candidate)
                , new SqlParameter("@employ", employer.Id_Employer))
                .SingleOrDefault();
                ViewBag.Err = (job_app != null);
            }
            ViewBag.Post = post;
            ViewBag.Employer = employer;
            var sp = db.Specializations.SqlQuery("SELECT * FROM Specializations").ToList();
            ViewPost v = new ViewPost()
            {
                Id_Post = post.Id_Post,
                DatePost = post.DatePost,
                Title = post.Title
            };
            foreach (var item2 in sp)
            {
                if (item2.Id_Specialization == post.Id_Specialization)
                {
                    v.Name_Specialization = item2.Name;
                    break;
                }
            }
            ViewBag.ViewPost = v;
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.list = db.Specializations.SqlQuery("SELECT * FROM Specializations").ToList();
            Post post = db.Posts.Find(id);
            return View(post);
        }
        [HttpPost]
        public ActionResult Edit(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(db.Posts.Find(id));
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}