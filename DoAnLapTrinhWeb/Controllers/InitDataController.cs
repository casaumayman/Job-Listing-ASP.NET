using DoAnLapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnLapTrinhWeb.Controllers
{
    public class InitDataController : Controller
    {
        // GET: InitData
        private readonly DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Specialization()
        {
            db.Specializations.Add(new Specialization() { Name = "Web" });
            db.Specializations.Add(new Specialization() { Name = "Application Mobile" });
            db.Specializations.Add(new Specialization() { Name = "Application Window" });
            db.Specializations.Add(new Specialization() { Name = "Testing" });
            db.Specializations.Add(new Specialization() { Name = "Security" });
            db.SaveChanges();
            return View();
        }
        public ActionResult School()
        {
            db.Schools.Add(new School() { Name = "HUTECT" });
            db.Schools.Add(new School() { Name = "Bách Khoa" });
            db.Schools.Add(new School() { Name = "Khoa học tự nhiên" });
            db.Schools.Add(new School() { Name = "Tôn Đức Thắng" });
            db.Schools.Add(new School() { Name = "Kinh tế tài chính" });
            db.SaveChanges();
            return View();
        }
    }
}