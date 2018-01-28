using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;
using Newtonsoft.Json;

namespace CustomRenderer.Droid
{
    static class RestAPI
    {
        static List<string> encoded = new List<string>();

        public static async Task StartCall(Bitmap image)
        {
            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                await image.CompressAsync(Bitmap.CompressFormat.Png, 5, stream);
                bitmapData = stream.ToArray();
            }
            System.Diagnostics.Debug.WriteLine("FRANK: compressed");
            image.Recycle();
            image.Dispose();

            encoded.Add(Convert.ToBase64String(bitmapData));
            if (encoded.Count == 5)
            {
                await Call();
            }
        }

        private static async Task Call()
        {
            HttpClient client = new HttpClient();
            var apiAdress = "https://9c2d7edd.ngrok.io/api/image";
            var json = new StringContent(JsonConvert.SerializeObject(new ImageString
            {
                base64 = encoded

            }), Encoding.UTF8, "application/json");
            encoded.Clear();
            var res = await client.PostAsync(apiAdress, json);
            System.Diagnostics.Debug.WriteLine("FRANK: posted " + res.ReasonPhrase);
        }
    }
}