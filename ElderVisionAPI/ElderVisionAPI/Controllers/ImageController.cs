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
        // GET: api/Image/FilePath
        [HttpGet("{FilePath}")]
        public IActionResult Get(string filePath)
        {
            Byte[] b = System.IO.File.ReadAllBytes(@"C:\Users\granf\Documents\GitHub\ConUhack\tmp\" + filePath);   // You can use your own method over here.         
            return File(b, "image/png");
        }

        // GET: api/Image/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Image
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var date = DateTime.Now.Ticks;
            string filePath = @"C:\Users\granf\Documents\GitHub\ConUhack\tmp\" + date + ".png";
            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(value));
            Process.Start(new ProcessStartInfo("cmd", $"/c start {filePath}") { CreateNoWindow = true });

            string accounSid = "AC74db4dcc60f91702206b697c0ef368e0";
            string authToken = "1c7051f8bb5fdda9d0e55c053f8fd62a";

            var uri = "https://8d339617.ngrok.io/api/image/" + date + ".png";

            TwilioClient.Init(accounSid, authToken);
            var message = MessageResource.Create(
               to: new PhoneNumber("+14388883108"),
               from: new PhoneNumber("+15146127729"),
               body: "ElderVision: Your elder might be in danger!! See picture to confirm",
               mediaUrl: new List<Uri> { new Uri(uri) });
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
