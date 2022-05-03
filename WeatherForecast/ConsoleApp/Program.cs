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


            Console.WriteLine("enter City: ");
            string city = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(city) || city.Length < 2)
            {
                Console.WriteLine("please enter valid city name: ");
                city = Console.ReadLine();
            }


            var tmpDegreesC = UserService.GetWeatherApi(city).Result;


            Console.WriteLine($"Current temperature is {tmpDegreesC}°C in {city}");



            Console.ReadKey();
        }
    }
}
