using BL;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.openweathermap.org");

            Console.WriteLine("enter City: ");
            string city = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(city) || city.Length < 2)
            {
                Console.WriteLine("please enter valid city name: ");
                city = Console.ReadLine();
            }

            var response = await client.GetAsync($"/data/2.5/weather?q={city},UK&appid={apikey}");
            var stringResult = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<dynamic>(stringResult);

            var tmpDegreesC = Math.Round(((float)obj.main.temp - 273.15), 2);
            Console.WriteLine($"Current temperature is {tmpDegreesC}°C in {city}");


            CommentContext comment;
            if (tmpDegreesC > 13)
            {
                comment = new CommentContext(new FreshStrategy());
            }
            else
            {
                comment = new CommentContext(new WarmlyStrategy());     
            }
            comment.Comment(city);


            Console.ReadKey();
        }
    }
}
