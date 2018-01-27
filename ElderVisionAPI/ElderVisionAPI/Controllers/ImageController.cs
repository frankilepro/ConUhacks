using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
