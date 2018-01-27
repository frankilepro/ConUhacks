using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ElderVisionAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Image")]
    public class ImageController : Controller
    {
        // GET: api/Image
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Image/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Image
        [HttpPost]
        public void Post([FromBody]string value)
        {
            string filePath = @"C:\Users\granf\Documents\GitHub\ConUhack\tmp\" + DateTime.Now.Ticks + ".png";
            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(value));
            Process.Start(new ProcessStartInfo("cmd", $"/c start {filePath}") { CreateNoWindow = true });

            string accounSid = "AC74db4dcc60f91702206b697c0ef368e0";
            string authToken = "1c7051f8bb5fdda9d0e55c053f8fd62a";

            TwilioClient.Init(accounSid, authToken);
            var message = MessageResource.Create(
               to: new PhoneNumber("+15146059990"),
               from: new PhoneNumber("+15146127729"),
               mediaUrl: new List<Uri> { new Uri("file:///" + filePath) },
               body: "ElderVision: Your elder might be in danger!! See picture to confirm");
        }

        // PUT: api/Image/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
