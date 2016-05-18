using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using viewPatientServer.Models;

namespace viewPatientServer
{
    public class apiPatientController : ApiController
    {
        // GET api/<controller>
        [EnableCors(           
            origins: "*",
            headers: "accept,content-type,origin",
            methods: "get,post")]
        public IEnumerable<entityPatient> Get()
        {
            PublicInfo.CheckClientSide();
            entityPatient tempPatient = new entityPatient();
            return tempPatient.getAllPatients();
        }

        // GET api/<controller>/5
        [EnableCors(
            origins: "*",
            headers: "accept,content-type,origin",
            methods: "get,post")]
        [Route("api/apiPatient/registID/{registID}/platform/{platform}")]
        public bool Get(string registID, string platform)
        {
            PublicInfo.CheckClientSide();
            entryAppPushMessage entryAppPushMessageObj = new entryAppPushMessage();
            entryAppPushMessageObj.strName = "joyce";
            entryAppPushMessageObj.strRegistID = registID;
            entryAppPushMessageObj.strPlatform = platform;
            return entryAppPushMessageObj.registedApp(); //儲存註冊的app id
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}