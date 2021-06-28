using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AllegroAnalitics.Common.Object;
using System.Linq;

namespace AllegroAnalitics.Common
{
    public class RequestProduct
    {

        private readonly HttpClient client = new HttpClient();
        public static AllegroData clientData;

        public RequestProduct()
        {
            clientData = JsonSerializer.Deserialize<AllegroData>(File.ReadAllText("allegroData.json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.allegro.public.v1+json"));
            client.Timeout = new TimeSpan(0, 0, 10);
        }

        public string ConstrucyRequest(RequestData requestData)
        {
            StringBuilder request = new StringBuilder();
            request.Append("https://api.allegro.pl/offers/listing?");
            request.Append($"phrase=\"{requestData.name}\"&include=-categories&include=-filters&include=-sort&sort=-startTime");
            foreach(var category in requestData.cattegoriesId)
            {
                request.Append($"&category.id={category}");
            }
            foreach (var parameter in requestData.parametrLists)
            {
                request.Append($"&{parameter.Key}={parameter.Value}");
            }
            return request.ToString();
        }

        public async Task<Items> SendRequest(string requestString,uint offset)
        {
            HttpResponseMessage response;
            Products products;
            int numberOfTrials = 0;
            do
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientData.token);
                response = await client.GetAsync($"{requestString}&offset={offset}");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) clientData.token = await GetAllegroToken.SetTokenAsync();
                ++numberOfTrials;
            } while (response.StatusCode != System.Net.HttpStatusCode.OK && numberOfTrials < 5);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception("Cannot download data");
            products = JsonSerializer.Deserialize<Products>(await response.Content.ReadAsStringAsync());
            return products.items;
        }

        public async Task<uint> FindFirstRegular(string requestString,uint start)
        {
            Items items;
            if (start > 30) 
            {
                start -= 30;
                items = await SendRequest(requestString, start);
            }
            else items = await SendRequest(requestString, 0);
            if (items.regular.Count > 0 && items.regular.Count < 60) return (uint)(start + items.promoted.Count);
            if (items.regular.Count == 0) 
            {
                for (uint i = 1; i < 50; ++i)
                {
                    items = await SendRequest(requestString, start + (i * 60));
                    if (items.regular.Count > 0) return (uint)(start + (i * 60) + items.promoted.Count);
                }
            }
            else
            {
                for (uint i = 1; i < 50; ++i)
                {
                    items = await SendRequest(requestString, start - (i * 60));
                    if (items.regular.Count > 0) return (uint)(start - (i * 60) + items.promoted.Count);
                }
            }
            return 0;
        }

        public async Task<RequestsResult> MakeRequest(RequestData requestData)
        {
            string requestString = ConstrucyRequest(requestData);
            RequestsResult requestsResult = new RequestsResult();
            bool isNewPromotedIteams = true;
            if (requestData.firstRegularId == null)
            {
                for (uint i = 0; i < 50; ++i)
                {
                    Items items = await SendRequest(requestString, i * 60);
                    
                    if (requestsResult.firstPromotedId == null && items.promoted.Count > 0) requestsResult.firstPromotedId = items.promoted[0].id;
                    if (requestsResult.firstRegularId == null && items.regular.Count > 0)
                    {
                        requestsResult.firstRegularId = items.regular[0].id;
                        requestsResult.firstRegularOffset = (uint)((i * 60) + items.promoted.Count);
                    }
                    if (items.promoted.Count + items.regular.Count < 59) break;
                    if (requestData.firstPromotedId != null && isNewPromotedIteams)
                    {
                        int Exist = items.promoted.FindIndex(x => x.id.Equals(requestData.firstPromotedId));
                        if (Exist != -1)
                        {
                            requestsResult.iteamList.AddRange(items.promoted.GetRange(0,Exist));
                            isNewPromotedIteams = false;
                        }
                    }
                    if(isNewPromotedIteams) requestsResult.iteamList.AddRange(items.promoted);
                    requestsResult.iteamList.AddRange(items.regular);
                }
            }
            else
            {
                uint i;
                for (i = 0; i < 25; ++i)
                {
                    Items items = await SendRequest(requestString, i * 60);

                    if (requestsResult.firstPromotedId == null && items.promoted.Count > 0) requestsResult.firstPromotedId = items.promoted[0].id;
                    if (requestsResult.firstRegularId == null && items.regular.Count > 0)
                    {
                        requestsResult.firstRegularId = items.regular[0].id;
                        requestsResult.firstRegularOffset = (uint)((i * 60) + items.promoted.Count);
                    }
                    if (requestData.firstPromotedId != null)
                    {
                        int Exist = items.promoted.FindIndex(x => x.id.Equals(requestData.firstPromotedId));
                        if (Exist != -1)
                        {
                            requestsResult.iteamList.AddRange(items.promoted.GetRange(0, Exist));
                            break;
                        }
                    }
                    if (items.promoted.Count<59 ) break;
                    requestsResult.iteamList.AddRange(items.promoted);
                }
                uint started = await FindFirstRegular(requestString,requestData.firstRegularOffset);
                requestsResult.firstRegularOffset = started;
                for (uint j = 0; j < 50 - i; ++j)
                {
                    Items items = await SendRequest(requestString,started + j * 60);

                    if (requestsResult.firstRegularId == null && items.regular.Count > 0) requestsResult.firstRegularId = items.regular[0].id;
                    if (items.regular.Count < 59 ) break;
                    if (requestData.firstRegularId != null)
                    {
                        int Exist = items.regular.FindIndex(x => x.id.Equals(requestData.firstRegularId));
                        if (Exist != -1)
                        {
                            requestsResult.iteamList.AddRange(items.regular.GetRange(0, Exist));
                            break;
                        }
                    }
                    requestsResult.iteamList.AddRange(items.regular);
                }
            }
            return requestsResult;
        }
    }
}