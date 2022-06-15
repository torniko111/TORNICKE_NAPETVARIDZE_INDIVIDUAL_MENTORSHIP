using BL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BL.Interfaces;
using DAL.Models;
using System.Collections.Generic;

namespace TestingForecast
{
    [TestClass]
    public class UnitTest1 : IWeatherService
    {

        public Task<Weather> AddAsync(Weather user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Weather user)
        {
            throw new NotImplementedException();
        }

        public Task<Weather> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<double> AddWeather(string city)
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


            if (tmpdegreesc > 16)
            {
                WeatherService.comment = new CommentContext(new FreshStrategy());
            }
            else
            {
                WeatherService.comment = new CommentContext(new DressWarmlyStrategy());
            }
            Console.WriteLine(WeatherService.comment.GetComment());

            return tmpdegreesc;
        }

        public Task<Dictionary<double, string>> GetWeatherForecast(string city, int days)
        {
            throw new NotImplementedException();
        }

        public Task<List<Weather>> ListAsync()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestMethod1()
        {
            double C = AddWeather("tbilisi").Result;

            Assert.IsTrue(C != -10000);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Arrange

            double C = AddWeather("london").Result;
            //Act

            bool result = false;

            if (C > 16)
            {
                if (WeatherService.comment.GetComment() == "its fresh")
                {
                    result = true;
                }
            }
            else if (C <= 16)
            {
                if (WeatherService.comment.GetComment() == "its Warm")
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            //Arrange

            double C = AddWeather("tbilisi").Result;
            //Act

            bool result = false;

            if (C > 16)
            {
                if (WeatherService.comment.GetComment() == "its fresh")
                {
                    result = true;
                }
            }
            else if (C <= 16)
            {
                if (WeatherService.comment.GetComment() == "its Warm")
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsTrue(result);
        }

        public Task UpdateAsync(Weather user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Weather>> Getreport(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task GetCurrentWeatherByCity(string city)
        {
            throw new NotImplementedException();
        }

        public Task GetCurrentWeatherBycitiesSameTime(string[] city)
        {
            throw new NotImplementedException();
        }

        public Task GetCurrentWeatherBycitiesDifferentTime(string city)
        {
            throw new NotImplementedException();
        }

        public Task GetCurrentWeatherByCitiesSameTime(string[] city)
        {
            throw new NotImplementedException();
        }

        public Task<List<Weather>> Getreport(DateTime from, DateTime to, string city)
        {
            throw new NotImplementedException();
        }
    }
}