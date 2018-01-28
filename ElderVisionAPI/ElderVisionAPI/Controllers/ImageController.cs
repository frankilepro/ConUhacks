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
using IBM.WatsonDeveloperCloud.VisualRecognition.v3;
using ElderVisionAPI.Model;
using System.Threading;

namespace ElderVisionAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Image")]
    public class ImageController : Controller
    {
        static int Counter = 0;
        static Queue<string> CurrTasks = new Queue<string>();
        static bool IsInProcess = false;
        static bool TwilioSent = false;

        // GET: api/Image
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{FilePath}")]
        public IActionResult Get(string filePath)
        {
            Byte[] b = System.IO.File.ReadAllBytes(@"C:\Users\alexi\Desktop\conuhacks\tmp\" + filePath);
            return File(b, "image/png");
        }

        //// GET: api/Image/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Image
        [HttpPost]
        public void Post([FromBody]ImageString value)
        {
            Debug.WriteLine("POST COUNT = " + value.base64.Count);
            foreach (var item in value.base64)
            {
                CurrTasks.Enqueue(item);
                //Task.Run(() => handleAsyncCalls(item));
            }
            if (!IsInProcess) ContinueOrStartProcess();
            //return Redirect("Photo");
        }

        private async void ContinueOrStartProcess()
        {
            while (CurrTasks.Count != 0)
            {
                IsInProcess = true;
                string first = CurrTasks.Dequeue();
                await Task.Run(() => handleAsyncCalls(GenerateId(first)));
            }
            IsInProcess = false;
        }

        private string GenerateId(string base64)
        {
            //var fileId = Guid.NewGuid() + "__" + Counter++;
            if (Counter == 10) Counter = 0;
            var fileId = Counter++.ToString();
            //var date = DateTime.Now.Ticks + "__" + Counter++;
            string filePath = @"C:\Users\alexi\Desktop\conuhacks\tmp\" + fileId + ".png";
            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64));
            //Process.Start(new ProcessStartInfo("cmd", $"/c start {filePath}") { CreateNoWindow = true });
            Debug.WriteLine("CURR COUNT = " + Counter);
            return fileId;
        }

        private void handleAsyncCalls(string fileId)
        {
            string accounSid = "AC74db4dcc60f91702206b697c0ef368e0";
            string authToken = "1c7051f8bb5fdda9d0e55c053f8fd62a";

            VisualRecognitionService vr = new VisualRecognitionService();
            vr.SetCredential("8f93d643451fc90c6047cdb1074cd2369bd1cbd8");
            string[] classifierIds = { "FallingPerson_739654467" };
            var uri = "http://9c2d7edd.ngrok.io/api/image/" + fileId + ".png";
            IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.ClassifyTopLevelMultiple result = null;
            try
            {
                result = vr.Classify(uri, classifierIds);
                //result = await Task.Run(() => vr.Classify(uri, classifierIds));
                //Thread.Sleep(1000);
            }
            catch(AggregateException e)
            {
                var allo = e.Message;
            }

            if (result == null)
            {
                Debug.WriteLine("NULL");
                return;
            }
            System.Diagnostics.Debug.WriteLine("Falling: {0}, fell {1}",
                result.Images.First().Classifiers.First().Classes[0].Score,
                result.Images.First().Classifiers.First().Classes[1].Score);

            if (!TwilioSent && result.Images.First().Classifiers.First().Classes[1].Score >= 0.5)
            {
                TwilioSent = !TwilioSent;
                TwilioClient.Init(accounSid, authToken);
                var message = MessageResource.Create(
                   to: new PhoneNumber("+15146059990"),
                   from: new PhoneNumber("+15146127729"),
                   mediaUrl: new List<Uri> { new Uri(uri) },
                   body: "ElderVision: Your elder might be in danger!! See picture to confirm");
            }
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
