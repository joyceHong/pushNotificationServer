using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using viewPatientServer;
using viewPatientServer.Controllers;
using viewPatientServer.Models;
using System.Collections.Generic;
using System.Web.Http.Cors;

namespace viewPatientServer.Tests.Controllers
{
     [EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            PublicInfo.CheckClientSide();
            entityPatient tempPatient = new entityPatient();

            IList<entityPatient> liAllPatient = tempPatient.getAllPatients();

              //tempPatient.getAllPatients();

            //// 排列
            //HomeController controller = new HomeController();

            //// 作用
            //ViewResult result = controller.Index() as ViewResult;

            //// 判斷提示
            //Assert.IsNotNull(result);
            //Assert.AreEqual("Home Page", result.ViewBag.Title);

            
        }
    }
}
