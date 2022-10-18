using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VACIMB_Service.Models;

namespace VACIMB_Service.Controllers
{
    public class InquiryController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post([FromBody] InquiryRequest values)
        {
            InquiryRequest res = values;

            //string hashKey = ConfigurationManager.AppSettings["HashKey"];

            VACIMB_Service.Models.InquiryVA r = InquiryVA.GetResult(res);
            return Request.CreateResponse(HttpStatusCode.OK, r);
        }

        public string Get()
        {
            return "Welcome To Web API MNC GUI";
        }
    }
}