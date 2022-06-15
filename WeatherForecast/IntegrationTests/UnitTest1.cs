using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        public static async Task<double> GetWeatherApi(string city)
        {
            if (string.IsNullOrWhiteSpace(city) || city.Length == 1)
            {
                throw new ArgumentNullException(nameof(city));
            }

            //needs to be moved to appsettings
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new();
            client.BaseAddress = new Uri("https://api.openweathermap.org");

            var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={apikey}");
            var stringResult = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<dynamic>(stringResult);

            var tmpdegreesc = Math.Round(((float)obj.main.temp - 272.15), 2);

            return tmpdegreesc;
        }

        [TestMethod]
        public void TestMethod1()
        {
            double C = GetWeatherApi("tbilisi").Result;

            Assert.IsTrue(C != -10000);
        }
    }
}