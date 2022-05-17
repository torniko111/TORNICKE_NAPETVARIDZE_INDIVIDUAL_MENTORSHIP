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
    public class UnitTest1 : IUserService
    {

        public Task<User> AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
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
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org");

            var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={apikey}");
            var stringResult = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<dynamic>(stringResult);

            var tmpdegreesc = Math.Round(((float)obj.main.temp - 272.15), 2);


            if (tmpdegreesc > 16)
            {
                UserService.comment = new CommentContext(new FreshStrategy());
            }
            else
            {
                UserService.comment = new CommentContext(new WarmlyStrategy());
            }
            Console.WriteLine(UserService.comment.Comment());

            return tmpdegreesc;
        }

        public Task<Dictionary<double, string>> GetWeatherForecast(string city, int days)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> ListAsync()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestMethod1()
        {
            double C = -10000;
            C = AddWeather("tbilisi").Result;

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
                if (UserService.comment.Comment() == "its fresh")
                {
                    result = true;
                }
            }
            else if (C <= 16)
            {
                if (UserService.comment.Comment() == "its Warm")
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
                if (UserService.comment.Comment() == "its fresh")
                {
                    result = true;
                }
            }
            else if (C <= 16)
            {
                if (UserService.comment.Comment() == "its Warm")
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsTrue(result);
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Weather>> getreport(DateTime from, DateTime to)
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
    }
}