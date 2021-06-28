using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Net.Http.Headers;
using AllegroAnalitics.Common.Object;

namespace AllegroAnalitics.Common
{
     public static class GetAllegroToken
    {
        private static readonly HttpClient client = new HttpClient();
        
        public static async Task<string> SetTokenAsync()
        {
            AllegroData clientData = JsonSerializer.Deserialize<AllegroData>(File.ReadAllText("allegroData.json"));
            var authToken = Encoding.ASCII.GetBytes($"{clientData.clientId}:{clientData.clientSecret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            var response = await client.GetAsync("https://allegro.pl/auth/oauth/token?grant_type=client_credentials");
            clientData.token = JsonSerializer.Deserialize<Token>(await response.Content.ReadAsStringAsync()).access_token;
            File.WriteAllText("allegroData.json", JsonSerializer.Serialize(clientData));
            return clientData.token;       
        }
    }
}

