using DoAnLapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnLapTrinhWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
            ViewBag.Candidate = (Candidate)Session["Candidate"];
            if (ViewBag.Candidate != null)
                ViewBag.Posts = db.Posts.SqlQuery("SELECT * FROM Posts WHERE Id_Specialization = @id", new SqlParameter("@id", ViewBag.Candidate.Id_Specialization)).ToList();
            ViewBag.Employers = db.Employers.ToList();
            return View();
        }

        public ActionResult About()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}