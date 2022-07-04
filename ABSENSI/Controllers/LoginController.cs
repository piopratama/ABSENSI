using ABSENSI.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABSENSI.Models;

namespace ABSENSI.Controllers
{
    using BCrypt.Net;

    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.header = "header";
            ViewBag.footer = "footer";
            ViewBag.customCss = "customCss";
            return View();
        }

        [HttpPost]
        //public ActionResult checkLogin(FormCollection collection)
        public ActionResult checkLogin(User collection)
        {
            bool isValid = false;
            var context = new AbsensiContext();
            var user=context.Users.Where(s => s.username == collection.username).FirstOrDefault<User>();
            if (user != null)
            {
                if (BCrypt.Verify(collection.password, user.password))
                {
                    isValid = true;
                }
            }

            if (isValid)
            {
                Session["user"] = user;
                if (user.is_admin)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["message"] = "Login failed, please check your username and password.";
            return RedirectToAction("Index", "Login");
        }

        public ActionResult logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}