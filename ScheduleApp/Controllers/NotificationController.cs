using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScheduleData.Services;

namespace ScheduleApp.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            var notificationService = new NotificationService();
            notificationService.SendEmail("dmpyatin@gmail.com", "subj", "<h3>body</h3>");
            return View();
        }
    }
}