using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnLapTrinhWeb.Models;
using DoAnLapTrinhWeb.ViewModel;

namespace DoAnLapTrinhWeb.Controllers
{
    public class CandidateController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult SignUp()
        {
            ViewBag.List = db.Schools.ToList();
            ViewBag.List2 = db.Specializations.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(FormCandidate form)
        {
            HttpPostedFileBase file = form.CV;
            ViewBag.List = db.Schools.ToList();
            ViewBag.List2 = db.Specializations.ToList();
            Candidate e = new Candidate()
            {
                Name = form.Name,
                PhoneNumber = form.PhoneNumber,
                Email = form.Email,
                UserName = form.UserName,
                Password = form.Password,
                YearBorn = form.YearBorn,
                Id_School = Convert.ToInt32(form.Id_School),
                Id_Specialization = Convert.ToInt32(form.Id_Specialization)
            };
            e.Gender = form.Gender == "male" ? true : false;
            if (form.Password != form.RePassword)
            {
                ViewBag.Err = "Mật khẩu không trùng khớp!";
                return View();
            }
            if (file != null && file.ContentLength > 0)
            {
                string exten = Path.GetExtension(file.FileName);
                string _FileName = e.UserName + exten;
                string _path = Path.Combine(Server.MapPath("/img/CV"), _FileName);
                e.CVLink = "/img/CV/" + _FileName;
                file.SaveAs(_path);
            }
            else
            {
                e.CVLink = null;
            }
            if (ModelState.IsValid)
            {
                var find = db.Employers.SqlQuery("SELECT * From Employers WHERE UserName=@UserName", new SqlParameter("@UserName", e.UserName)).SingleOrDefault();
                var find2 = db.Candidates.SqlQuery("SELECT * From Candidates WHERE UserName=@UserName", new SqlParameter("@UserName", e.UserName)).SingleOrDefault();
                if (find != null || find2 != null)
                {
                    ViewBag.Err = "Tên tài khoản đã tồn tại!";
                    return View();
                }
                try
                {
                    db.Candidates.Add(e);
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    ViewBag.Err = err.ToString();
                    return View();
                }
                return RedirectToAction("Index", new { Controller = "Home" });
            }
            return View(form);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            ViewBag.candidate = db.Candidates.SqlQuery("SELECT * FROM Candidates WHERE Id_Candidate = @id", new SqlParameter("@id",id)).SingleOrDefault();
            var Specializations = db.Specializations.ToList();
            foreach (var item in Specializations)
            {
                if (item.Id_Specialization == ViewBag.candidate.Id_Specialization)
                {
                    ViewBag.specialization = item.Name;
                    break;
                }
            }
            var School = db.Schools.ToList();
            foreach (var item in School)
            {
                if (item.Id == ViewBag.candidate.Id_School)
                {
                    ViewBag.school = item.Name;
                    break;
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Candidate candidate = new Candidate();
            candidate = db.Candidates.SqlQuery("SELECT * FROM Candidates WHERE Id_Candidate=@id", new SqlParameter("@id",id)).SingleOrDefault();
            FormCandidate formCandidate = new FormCandidate()
            {
                Id_Candidate = candidate.Id_Candidate,
                Name = candidate.Name,
                PhoneNumber = candidate.PhoneNumber,
                Gender = candidate.Gender?"male":"female",
                YearBorn = candidate.YearBorn,
                Email = candidate.Email,
                Id_School = candidate.Id_School.ToString(),
                Id_Specialization = candidate.Id_Specialization.ToString(),
            };
            ViewBag.candidate = formCandidate;
            ViewBag.Specializations = db.Specializations.ToList();
            ViewBag.Schools = db.Schools.SqlQuery("SELECT * FROM Schools").ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCandidate form)
        {
            Candidate candidate = db.Candidates.Find(form.Id_Candidate);
            candidate.Name = form.Name;
            candidate.PhoneNumber = form.PhoneNumber;
            candidate.Gender = (form.Gender == "male") ? true : false;
            candidate.YearBorn = form.YearBorn;
            candidate.Email = form.Email;
            candidate.Id_School = Convert.ToInt32(form.Id_School);
            candidate.Id_Specialization = Convert.ToInt32(form.Id_Specialization);
            HttpPostedFileBase file = form.CV;
            if (file != null && file.ContentLength > 0)
            {
                string exten = Path.GetExtension(file.FileName);
                string _FileName = candidate.UserName + exten;
                string _path = Path.Combine(Server.MapPath("/img/CV"), _FileName);
                candidate.CVLink = "/img/CV/" + _FileName;
                file.SaveAs(_path);
            }
            db.SaveChanges();
            return RedirectToAction("Details", new {id = candidate.Id_Candidate});
        }
        public ActionResult SearchEmployer(string name)
        {
            if (name == "")
            {
                ViewBag.Employers = db.Employers.SqlQuery("SELECT * FROM Employers").ToList();
            } else
            { 
                ViewBag.Employers = db.Employers.Where(s => s.CompanyName.Contains(name)).ToList();
            }
            Candidate candidate = new Candidate();
            candidate = (Candidate)Session["Candidate"];
            List<bool> Err = new List<bool>();
            foreach(var Employer in ViewBag.Employers)
            {
                var JobApplication = db.Followings.SqlQuery("SELECT * FROM Followings WHERE Id_Candidate=@can AND Id_Employer=@emp"
                    ,new SqlParameter("@can", candidate.Id_Candidate)
                    ,new SqlParameter("@emp", Employer.Id_Employer)).SingleOrDefault();
                Err.Add(JobApplication == null);
            }
            ViewBag.Err = Err;
            return View();
        }
        public ActionResult SearchPost(FormCollection form)
        {
            int salary;
            if (form["salary"] == null) salary = 999999999;
            else if (form["salary"] == "") salary = (int)0;
            else salary = Convert.ToInt32(form["salary"]);
            ViewBag.Specializations = db.Specializations.ToList();
            if (form["Id_Specialization"] == "All" || form["Id_Specialization"] == null)
            {
                ViewBag.Posts = db.Posts.SqlQuery("SELECT * FROM Posts WHERE Salary >= @num",new SqlParameter("@num",salary)).ToList();
            }
            else
            {
                ViewBag.Posts = db.Posts
                    .SqlQuery("SELECT * FROM Posts WHERE Salary>=@num AND Id_Specialization=@Id"
                    , new SqlParameter("@num",salary)
                    , new SqlParameter("@Id",Convert.ToInt32(form["Id_Specialization"])))
                    .ToList();
            }
            return View();
        }
        [HttpGet]
        public ActionResult JobApplication(int id)
        {
            Candidate candidate = new Candidate();
            candidate = (Candidate)Session["Candidate"];
            Employer employer = new Employer();
            employer = db.Employers.Find(id);
            JobApplication job = new JobApplication() {
                Id_Candidate = candidate.Id_Candidate,
                Id_Employer = id,
                DateOfApplication = DateTime.Now
            };
            db.JobApplications.Add(job);
            db.SaveChanges();
            return View();
        }
        public ActionResult Follow(int id)
        {
            Candidate candidate = new Candidate();
            candidate = (Candidate)Session["Candidate"];
            Employer employer = new Employer();
            employer = db.Employers.Find(id);
            Following following = new Following()
            {
                Id_Candidate = candidate.Id_Candidate,
                Id_Employer = id
            };
            db.Followings.Add(following);
            db.SaveChanges();
            return View();
        }
        public ActionResult UnFollow(int id)
        {
            Candidate candidate = new Candidate();
            candidate = (Candidate)Session["Candidate"];
            Employer employer = new Employer();
            employer = db.Employers.Find(id);
            Following following = new Following()
            {
                Id_Candidate = candidate.Id_Candidate,
                Id_Employer = id
            };
            db.Database.ExecuteSqlCommand("DELETE FROM Followings WHERE Id_Candidate = @can AND Id_Employer = @emp"
                , new SqlParameter("@can",candidate.Id_Candidate)
                , new SqlParameter("@emp",employer.Id_Employer));
            db.SaveChanges();
            return View();
        }
    }
}
