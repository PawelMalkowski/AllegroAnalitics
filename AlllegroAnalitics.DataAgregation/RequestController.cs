using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AllegroAnalitics.DataAgregation
{
    public class RequestController
    {
        CookieContainer cookies = new CookieContainer();
        HttpClientHandler handler = new HttpClientHandler();
        private readonly HttpClient client = null;
        public RequestController()
        {
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 0, 10);
            var timer = new System.Threading.Timer((e) =>
            {
               takeAllTimings();
            }, null, startTimeSpan, periodTimeSpan);
        }

        private async Task Login()
        {
            string s = "{\"UserName\":\"Admin\",\"Password\": \"Admin1234,\"}";
            HttpContent httpContent = new StringContent(s, Encoding.UTF8, "application/json");
            await client.PostAsync("http://localhost:8000/api/v1/user/Login", httpContent);
        }
        private async Task takeAllTimings()
        {
            try
            {
                HttpResponseMessage response = null;
                for (int i = 0; i < 5; ++i)
                {
                    response = await client.GetAsync("http://localhost:8000/api/v1/data/GetAllRequestTime");
                    if (response.StatusCode == HttpStatusCode.OK) break;
                    await Login();
                }
                string abc = await response.Content.ReadAsStringAsync();
                var timings = JsonSerializer.Deserialize<DataList>(await response.Content.ReadAsStringAsync());
                var a = 1;
                foreach (var time in timings.timingOrder)
                {
                    if (time.date < DateTime.Now.AddMinutes(-5))
                    {
                        UpdateData updateData = new UpdateData { id = time.orderid };
                        HttpContent httpContent = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");
                        client.PostAsync("http://localhost:8000/api/v1/data/UpdateData", httpContent);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
}