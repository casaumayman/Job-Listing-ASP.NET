using DoAnLapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnLapTrinhWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext db = new DatabaseContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string UserName = form["UserName"];
            string Password = form["Password"];
            Candidate candidate = new Candidate();
            candidate = db.Candidates.SqlQuery("select * from Candidates where UserName=@UserName And Password=@Password", new SqlParameter("UserName",@UserName), new SqlParameter("Password",@Password)).SingleOrDefault();
            if (candidate != null)
            {
                // Login Success Candidates
                Session.Add("Candidate", candidate);
                return RedirectToAction("Index",new { Controller = "Home" });
            }
            else
            {
                Employer employer = new Employer();
                employer = db.Employers.SqlQuery("select * from Employers where UserName=@UserName And Password=@Password", new SqlParameter("UserName", @UserName), new SqlParameter("Password", @Password)).SingleOrDefault();
                if (employer == null)
                {
                    ViewBag.Err = "Tên tài khoản hoặc mật khẩu không chính xác!";
                    return View();
                }
                // Login Success Employer
                Session.Add("Employer", employer);
                return RedirectToAction("Index", new { Controller = "Home" });
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", new { Controller = "Home" });
        }
    }
}