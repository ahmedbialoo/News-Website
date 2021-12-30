using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day3_Lab.Models;
using PagedList;

namespace Day3_Lab.Controllers
{
    
    public class HomeController : Controller
    {
        userDBContext db = new userDBContext();
        public ActionResult users(int? pageno , string sort,string search)
        {
            if (Session["userid"]!=null)
            {

            int no = pageno == null ? 1 : pageno.Value;
            ViewBag.sort = sort;
            ViewBag.search = search;
            var users = db.user_data.OrderBy(n => n.user_id);
            if (!String.IsNullOrEmpty(search))
            {
            
                users = db.user_data.Where(n => n.user_name.Contains(search)).OrderBy(n => n.user_id);
                
            }
            switch (sort)
            {
                case "name":
                    users = db.user_data.OrderBy(n => n.user_name);
                    break;
                case "email":
                    users = db.user_data.OrderBy(n => n.email);
                    break;
                case "age":
                    users = db.user_data.OrderByDescending(n => n.age);
                    break;
                default:
                    break;
            }

            return View(users.ToPagedList(no,3));
            }
            else
            {
                return RedirectToAction("login");
            }
            
        } 
        public ActionResult download (user_data e)
        {
            return File($"/attach/{e.cv}","application/pdf");
        }
        public ActionResult create ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(user_data e,HttpPostedFileBase photo,HttpPostedFileBase cv)
        {

            
                cv.SaveAs(Server.MapPath($"~/attach/{cv.FileName}"));
                e.cv = cv.FileName;
                photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
                e.photo = photo.FileName;
                db.user_data.Add(e);
                db.SaveChanges();
                return RedirectToAction("users");
          
        } 
        public ActionResult login ()
        {
            if (Request.Cookies["mvc"]!=null)
            {
                Session.Add("userid",Request.Cookies["mvc"].Values["userid"]);
                return RedirectToAction("profile");
            }
            return View();
        }
        [HttpPost]
        public ActionResult login (user_data e, bool rememberme)
        {
            user_data emp = db.user_data.Where(n => n.email == e.email && n.password == e.password).FirstOrDefault();
            if (emp!= null)
            {
                Session.Add("userid", emp.user_id);
                if (rememberme)
                {
                    HttpCookie co = new HttpCookie("mvc");
                    co.Values.Add("userid", emp.user_id.ToString());
                    co.Expires= DateTime.Now.AddMonths(2);
                    Response.Cookies.Add(co);
                }
                return RedirectToAction("profile");
            }
            else
            {
                ViewBag.error = "invalid";
                return View();
            }
        }
        public ActionResult profile ()
        {
            int no = int.Parse(Session["userid"].ToString());
            return View(db.user_data.Find(no));
        } 

        public ActionResult logout()
        {
            Session["userid"] = null;
            HttpCookie co = new HttpCookie("mvc");
            co.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(co);
            return RedirectToAction("login");
        } 
        public ActionResult edit (int id)
        {
            user_data emp = db.user_data.Find(id);
            return View(emp);
        }
        [HttpPost]
        public ActionResult edit(user_data e, HttpPostedFileBase photo)
        {
            user_data emp = db.user_data.Find(e.user_id);
            if (photo !=null)
            {
                photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
                emp.photo = photo.FileName;
            }
            emp.confirm_password = emp.password;
            emp.user_name = e.user_name;
            emp.adress = e.adress;
            emp.age = e.age;
            emp.email = e.email;
            db.SaveChanges();
            return RedirectToAction("users");
        } 
        public ActionResult delete (int id)
        {
            db.user_data.Remove(db.user_data.Find(id));
            db.SaveChanges();
            return RedirectToAction("users");
        }
        public ActionResult create_new ()
        {
            ViewBag.cats = new SelectList(db.catalogs.ToList(), "cat_id", "cat_name");
            return View();
        }
        [HttpPost]
        public ActionResult create_new(news n,HttpPostedFileBase photo)
        {
            int uid = int.Parse(Session["userid"].ToString());
            n.user_id = uid;
            photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
            n.photo = photo.FileName;
            n.datetime = DateTime.Now;
            db.news.Add(n);
            db.SaveChanges();
            return RedirectToAction("profile");
        }
        public ActionResult user_news (int id, int? pageno , string sort, string search)
        {
            var ns = db.news.Where(n => n.user_id == id).OrderBy(n => n.datetime);
            ViewBag.sort = sort;
            ViewBag.search = search;
            switch(sort)
            {
                case "title":
                    ns = db.news.Where(n => n.user_id == id).OrderByDescending(n => n.title);
                    break;
                case "time":
                    ns = db.news.Where(n => n.user_id == id).OrderByDescending(n => n.datetime);
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(search))
            {
                ns = db.news.Where(n => n.user_id == id && n.title.Contains(search)).OrderBy(n => n.datetime);
            }
            int no = pageno == null ? 1 : pageno.Value;
            return View(ns.ToPagedList(no,3));
        }
        public ActionResult edit_profile(int id)
        {
            user_data user = db.user_data.Find(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult edit_profile(user_data e, HttpPostedFileBase photo)
        {
            user_data emp = db.user_data.Find(e.user_id);
            if (photo != null)
            {
                photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
                emp.photo = photo.FileName;
            }
            emp.confirm_password = emp.password;
            emp.user_name = e.user_name;
            emp.adress = e.adress;
            emp.age = e.age;
            emp.email = e.email;
            
            db.SaveChanges();
            return RedirectToAction("profile");
        }
        public ActionResult change_password(int id)
        {
            var user = db.user_data.Find(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult change_password(int old, int password, int confirm_password)
        {

            var user = db.user_data.Find(int.Parse(Session["userid"].ToString()));
            if (old == user.password)
            {
                if (password == confirm_password)
                {

                    user.password = password;
                    user.confirm_password = user.password;
                    db.SaveChanges();
                    ViewBag.ok = "you have successfuly changed your password";
                    return View();
                }
                else
                {
                    ViewBag.retry = "password not matched";
                    return View();
                }

            }
            else
            {
                ViewBag.error = "enter valid password";
                return View();

            }
        }

        public ActionResult details (int id)
        {
            news news_details = db.news.Find(id);
            ViewBag.details = news_details;
            return PartialView();
        }

        public ActionResult check(string email)
        {
            user_data user = db.user_data.Where(n => n.email == email).FirstOrDefault();
            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}