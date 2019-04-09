using DoAnLapTrinhWeb.Models;
using DoAnLapTrinhWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnLapTrinhWeb.Controllers
{
    [AllowCrossSiteJson]
    public class EmployerController : Controller
    {
        private readonly DatabaseContext db = new DatabaseContext();
        // GET: Employer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(FormEmployer form)
        {
            HttpPostedFileBase file = form.ImgLogo;
            Employer e = new Employer()
            {
                CompanyName = form.CompanyName,
                Address = form.Address,
                Email = form.Email,
                UserName = form.UserName,
                Password = form.Password
            };
            if (form.Password != form.RePassword)
            {
                ViewBag.Err = "Mật khẩu không trùng khớp!";
                return View();
            }
            if (file != null && file.ContentLength > 0)
            {
                string exten = Path.GetExtension(file.FileName);
                string _FileName = e.UserName + exten;
                string _path = Path.Combine(Server.MapPath("/img/Employer"), _FileName);
                e.ImgLogo = "/img/Employer/" + _FileName;
                file.SaveAs(_path);
            }
            else
            {
                e.ImgLogo = "~/img/Employer/Empty.jpg";
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
                    db.Employers.Add(e);
                    db.SaveChanges();
                } catch(Exception err)
                {
                    ViewBag.Err = err.ToString();
                    return View();
                }
                return RedirectToAction("Index", new { Controller = "Home" });
            }
            return View(form);
        }
        public ActionResult SearchCandidate(FormCollection form)
        {
            ViewBag.Schools = db.Schools.ToList();
            ViewBag.Specializations = db.Specializations.ToList();
            if (form["Id_School"] == null) form["Id_School"] = "-1";
            if (form["Id_Specialization"] == null) form["Id_Specialization"] = "-1";
            if (form["Id_School"] != null && form["Id_Specialization"] != null)
            {
                if (form["Id_School"] == "All")
                {
                    if (form["Id_Specialization"] == "All")
                    {
                        ViewBag.Candidates = db.Candidates.SqlQuery("SELECT * FROM Candidates").ToList();
                    }
                    else
                    {
                        ViewBag.Candidates = db.Candidates.SqlQuery("SELECT * FROM Candidates WHERE Id_Specialization = @Id_Specialization", new SqlParameter("@Id_Specialization", Convert.ToInt32(form["Id_Specialization"]))).ToList();
                    }
                }
                else
                {
                    if (form["Id_Specialization"] == "All")
                    {
                        ViewBag.Candidates = db.Candidates
                            .SqlQuery("SELECT * FROM Candidates WHERE Id_School = @Id_School", new SqlParameter("@Id_School", Convert.ToInt32(form["Id_School"])))
                            .ToList();
                    }
                    else
                    {
                        ViewBag.Candidates = db.Candidates
                            .SqlQuery("SELECT * FROM Candidates WHERE Id_Specialization = @Id_Specialization AND Id_School = @Id_School"
                            , new SqlParameter("@Id_Specialization", Convert.ToInt32(form["Id_Specialization"]))
                            , new SqlParameter("@Id_School", Convert.ToInt32(form["Id_School"])))
                            .ToList();
                    }
                }
                ViewBag.Id_School = form["Id_School"];
                ViewBag.Id_Specialization = form["Id_Specialization"];
            }
            return View();
        }
        public ActionResult ListApplication()
        {
            Employer employer = new Employer();
            employer = (Employer)Session["Employer"];
            ViewBag.JobApplications = db.JobApplications.Where(m => m.Id_Employer == employer.Id_Employer).ToList();
            ViewBag.Candidates = db.Candidates.ToList();
            ViewBag.Schools = db.Schools.ToList();
            ViewBag.Specializations = db.Specializations.ToList();
            return View();
        }
        public ActionResult Report(FormCollection form)
        {
            int year;
            Employer employer = new Employer();
            employer = (Employer)Session["Employer"];
            if (form["year"] == null || form["year"]=="") year = DateTime.Now.Year;
            else year = Convert.ToInt32(form["year"]);
            ViewBag.Year = year;
            List<int> Arr = new List<int>();
            int tong = 0;
            for (int i = 0; i < 12; i++)
            {
                Arr.Add(db.JobApplications.Count(m => m.Id_Employer == employer.Id_Employer && (m.DateOfApplication.Month == i+1) && (m.DateOfApplication.Year == year)));
                tong += Arr[i];
            }
            ViewBag.Arr = Arr;
            ViewBag.Total = tong;
            return View();
        }
    }
}