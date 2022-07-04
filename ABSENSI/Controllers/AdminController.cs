using ABSENSI.Context;
using ABSENSI.Models;
using ABSENSI.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ABSENSI.Controllers
{
    using ABSENSI.Class;
    using BCrypt.Net;
    using OfficeOpenXml;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Net;
    using System.Xml.Linq;

    public class AdminController : Controller
    {
        // GET: Admin
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
            var userList = context.Users.ToList<User>();

            var userViewModel = new UserViewModel
            {
                user=new User(),
                userList = userList
            };

            return View(userViewModel);
        }

        [HttpPost]
        public ActionResult insertEmployee(ViewModel.UserViewModel collection)
        {
            var context = new AbsensiContext();
            context.Users.Add(new User
            {
                username = collection.user.username,
                fullname=collection.user.fullname,
                password = BCrypt.HashPassword(collection.user.password),
                is_admin=collection.user.is_admin,
                inserted_at=DateTime.UtcNow
            });
            context.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public JsonResult insertEvent(int user_id, DateTime date)
        {
            MyResult e = new MyResult();
            try
            {
                if (date.ToUniversalTime().Date.Subtract(DateTime.UtcNow.Date).TotalDays >= 0)
                {
                    var context = new AbsensiContext();

                    date = date.ToUniversalTime();
                    int countData = context.UserSchedules.Where(s => s.user_id == user_id && s.schedule_start <= date && s.schedule_end >= date).Count();

                    if (countData == 0)
                    {
                        context.UserSchedules.Add(new UserSchedule
                        {
                            user_id = user_id,
                            schedule_start = date,
                            schedule_end = date,
                            inserted_at = DateTime.UtcNow

                        });

                        context.SaveChanges();
                        e.code = 0;
                        e.desc = "";
                    }
                    else
                    {
                        e.code = -1;
                        e.desc = "This user has been added to this schedule";
                    }
                }
                else
                {
                    e.code = -1;
                    e.desc = "Cannot set schedule for the past";
                }
            }
            catch(Exception ex)
            {
                e.code = -1;
                e.desc = ex.Message;
            }
            return Json(e);
        }

        [HttpPost]
        public JsonResult updateEvent(int user_schedule_id, DateTime date_start, DateTime date_end)
        {
            MyResult e = new MyResult();
            try
            {
                var context = new AbsensiContext();

                var userScheduleToUpdate = context.UserSchedules.SingleOrDefault(s => s.user_schedule_id == user_schedule_id);
                userScheduleToUpdate.schedule_start = date_start.ToUniversalTime();
                userScheduleToUpdate.schedule_end = date_end.ToUniversalTime();

                context.SaveChanges();
                e.code = 0;
                e.desc = "";
            }
            catch (Exception ex)
            {
                e.code = -1;
                e.desc = ex.Message;
            }
            return Json(e);
        }

        [HttpGet]
        public JsonResult getEvent()
        {
            var context = new AbsensiContext();

            var userSchedulesList = context.UserSchedules.Where(s=>s.user_id!=10 && s.user_id!=11) .Join(
                context.Users,
                p=>p.user_id,
                e=>e.user_id,
                (p, e)=>new
                {
                    user_schedule_id=p.user_schedule_id,
                    user_id=p.user_id,
                    username=e.username,
                    fullname=e.fullname,
                    start=p.schedule_start,
                    end=p.schedule_end,
                    attendance = p.attendance
                }).ToList();

            //var list = new[]
            //{
            //    new { title = "Event 1", start = "2022-06-01", end="2022-06-01" },
            //    new { title = "Event 2", start = "2022-06-02", end="2022-06-02" }
            //}.ToList();
            List<CalendarEvent> calendarEvent = new List<CalendarEvent>();
            foreach(var u in userSchedulesList)
            {
                calendarEvent.Add(new CalendarEvent
                {
                    id = u.user_schedule_id.ToString(),
                    title = u.fullname,
                    start = u.start.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    end = u.end.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    attendance = u.attendance,
                    is_past = (DateTime.UtcNow.Subtract(u.start).TotalHours > 24 || u.attendance ? true : false)
                });
            }

            return Json(calendarEvent, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult deleteEvent(int user_schedule_id)
        {
            MyResult e = new MyResult();

            try
            {
                var context = new AbsensiContext();
                context.Entry(new UserSchedule
                {
                    user_schedule_id = user_schedule_id
                }).State = EntityState.Deleted;
                context.SaveChanges();
                e.code = 0;
                e.desc = "";
            }
            catch (Exception ex)
            {
                e.code = -1;
                e.desc = ex.Message;
            }
            return Json(e);
        }
        [HttpPost]
        public ActionResult deleteUser(ViewModel.UserViewModel collection)
        {
            var context = new AbsensiContext();
            context.Entry(new User
            {
                user_id = collection.user.user_id
            }).State = EntityState.Deleted;
            context.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult printReport(ViewModel.UserViewModel collection)
        {
            DateTime dTStart = ((DateTime)collection.user.inserted_at).ToUniversalTime().Date;
            DateTime dTEnd = dTStart.AddDays(1);

            var context = new AbsensiContext();

            var userSchedulesList = context.UserSchedules.Where(s=>s.schedule_start>=dTStart && s.schedule_end<dTEnd).Join(
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
                    attendance = p.attendance,
                    lat=p.lat,
                    lng=p.lng
                }).ToList();

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Nama";
            Sheet.Cells["B1"].Value = "Kehadiran";
            Sheet.Cells["C1"].Value = "Lokasi";

            int row = 2;
            foreach (var u in userSchedulesList)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = u.fullname;
                Sheet.Cells[string.Format("B{0}", row)].Value = (u.attendance == true ? "Hadir" : "Alfa");

                if (!String.IsNullOrEmpty(u.lat) && !String.IsNullOrEmpty(u.lng))
                {
                    RetrieveFormatedAddress(u.lat, u.lng);
                }

                Sheet.Cells[string.Format("C{0}", row)].Value = "";

                row++;
            }

            //int row = 2;
            //foreach (var item in collection)
            //{

            //    Sheet.Cells[string.Format("A{0}", row)].Value = item.Name;
            //    Sheet.Cells[string.Format("B{0}", row)].Value = item.Department;
            //    Sheet.Cells[string.Format("C{0}", row)].Value = item.Address;
            //    Sheet.Cells[string.Format("D{0}", row)].Value = item.City;
            //    Sheet.Cells[string.Format("E{0}", row)].Value = item.Country;
            //    row++;
            //}


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

            return Content("");
        }

        public static void RetrieveFormatedAddress(string lat, string lng)
        {
            string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";

            string location = string.Empty;
            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants()
                              where elm.Name == "status"
                              select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants()
                               where elm.Name == "formatted_address"
                               select elm).FirstOrDefault();
                    requestUri = res.Value;
                }
            }
        }
    }
}