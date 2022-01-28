using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using EveListener.Model;

namespace EveListener.Service
{
    public class EveDataService
    {
        public const string DEFAULT_EVE_URI = "http://192.168.0.100";

        private string eveUri = "";
        private HttpClient client;

        public EveDataService(string uri = "")
        {
            if (string.IsNullOrEmpty(uri))
            {
                eveUri = DEFAULT_EVE_URI;
            }
            else
            {
                eveUri = uri;
            }

            client = new HttpClient();
        }

        public async Task<Measurement> GetMeasurement()
        {
            HttpResponseMessage response = await client.GetAsync(eveUri);

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Measurement data = JsonConvert.DeserializeObject<Measurement>(responseBody);
                return data;
            }
            else
            {
                Console.WriteLine("Unable to fetch data from Eve. Device may be offline.");
                Environment.Exit(1);
                return null;
            }
        }
    }
}