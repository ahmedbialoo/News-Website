using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day3_Lab.Models;

namespace Day3_Lab.Controllers
{
    public class userController : Controller
    {
        userDBContext db = new userDBContext(); 
        public ActionResult Index()
        {
            ViewBag.cats = new SelectList(db.catalogs.ToList(), "cat_id", "cat_name");
            return View();
        }
        public ActionResult show_news(int? droplist)
        {
            List<news> ns = db.news.Where(n => n.cat_id == droplist).ToList();
            if (droplist==null)
            {
                ns = db.news.ToList();
                return PartialView(ns);
            }
            else
            {
            return PartialView(ns);

            }
        }
        public ActionResult cat_news()
        {
            ViewBag.cats = db.catalogs.ToList();
            return PartialView();
        }
        public ActionResult newsByCat(int catid)
        {
            List<news> ns = db.news.Where(n => n.cat_id == catid).ToList();
            return View(ns);
        }
    }
}