using ABSENSI.Class;
using ABSENSI.Context;
using ABSENSI.Models;
using ABSENSI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABSENSI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.header = "header";
            ViewBag.footer = "footer";
            ViewBag.customCss = "customCss";

            var context = new AbsensiContext();
            int user_id = ((User)Session["user"]).user_id;

            List<User> userObj = (from t in context.Users
                           where t.user_id == user_id
                           select t).ToList<User>();

            List<User> userList = new List<User>();
            foreach(User u in userObj)
            {
                userList.Add(u);
            }

            var userViewModel = new UserViewModel
            {
                user = new User(),
                userList = userList
            };

            return View("Home",userViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public JsonResult getEvent()
        {
            var context = new AbsensiContext();
            int user_id = ((User)Session["user"]).user_id;

            var userSchedulesList = context.UserSchedules.Join(
            context.Users,
            p => p.user_id,
            e => e.user_id,
            (p, e) => new
            {
                user_schedule_id = p.user_schedule_id,
                user_id = p.user_id,
                username = e.username,
                fullname = e.fullname,
                start = p.schedule_start,
                end = p.schedule_end,
                attendance = p.attendance
            }).Where(s => s.user_id == user_id).ToList(); //&& s.start.Date==DateTime.Now.Date

            if (user_id == 11)
            {
                userSchedulesList = context.UserSchedules.Join(
                context.Users,
                p => p.user_id,
                e => e.user_id,
                (p, e) => new
                {
                    user_schedule_id = p.user_schedule_id,
                    user_id = p.user_id,
                    username = e.username,
                    fullname = e.fullname,
                    start = p.schedule_start,
                    end = p.schedule_end,
                    attendance = p.attendance
                }).ToList(); //&& s.start.Date==DateTime.Now.Date
            }

            //var list = new[]
            //{
            //    new { title = "Event 1", start = "2022-06-01", end="2022-06-01" },
            //    new { title = "Event 2", start = "2022-06-02", end="2022-06-02" }
            //}.ToList();
            List<CalendarEvent> calendarEvent = new List<CalendarEvent>();
            foreach (var u in userSchedulesList)
            {
                calendarEvent.Add(new CalendarEvent
                {
                    id = u.user_schedule_id.ToString(),
                    title = u.fullname,
                    start = u.start.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    end = u.end.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    attendance=u.attendance,
                    is_past=(DateTime.UtcNow.Subtract(u.start).TotalHours > 24 || u.attendance ? true : false),
                    constraint= "businessHours"
                });
            }

            return Json(calendarEvent, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult insertAttendance(ViewModel.UserViewModel collection)
        {
            var context = new AbsensiContext();
            //context.Users.Add(new User
            //{
            //    username = collection.user.username,
            //    fullname = collection.user.fullname,
            //    password = BCrypt.HashPassword(collection.user.password),
            //    is_admin = collection.user.is_admin,
            //    inserted_at = DateTime.Now
            //});
            int id_user_schedule = collection.userSchedule.user_schedule_id;
            var result = context.UserSchedules.SingleOrDefault(b => b.user_schedule_id == id_user_schedule 
            && b.schedule_start<=DateTime.UtcNow && b.schedule_end>=DateTime.UtcNow);
            if (result != null)
            {
                if (!String.IsNullOrWhiteSpace(collection.userSchedule.lat) && !String.IsNullOrWhiteSpace(collection.userSchedule.lng))
                {
                    result.lat = collection.userSchedule.lat;
                    result.lng = collection.userSchedule.lng;
                    result.attendance = true;
                    result.updated_at = DateTime.UtcNow;
                    context.SaveChanges();
                }
                else
                {
                    TempData["message"] = "Failed to submit because position is not found";
                }
            }
            else
            {
                TempData["message"] = "Failed to submit because the datetime is out of the allowed range";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}