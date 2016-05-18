using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace viewPatientServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public ActionResult Index(string deviceID, string pushMessage)
        {
            try
            {
                Notification.pushMessage pushMessageObj = new Notification.pushMessage();
                pushMessageObj.SendIPhoneNotification(deviceID, pushMessage);
                return View();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}
