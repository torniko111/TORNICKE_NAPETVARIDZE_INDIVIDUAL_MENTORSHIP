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
using BL.CommentStrategy;
using System.Linq;

namespace TestingForecast
{
    [TestClass]
    public class WeatherApiUnitTests : IWeatherService
    {
        private readonly List<Weather> WeatherObjects = new()
        {
            new Weather() { Id = 1, TempC =20, CityName = "london", CreatedOn =DateTime.Now},
            new Weather() { Id = 1, TempC =21, CityName = "tbilisi", CreatedOn =DateTime.Now},
            new Weather() { Id = 1, TempC =22, CityName = "rustavi", CreatedOn =DateTime.Now},
            new Weather() { Id = 1, TempC =23, CityName = "london", CreatedOn =DateTime.Now}
        };
        private readonly Dictionary<string, double> _weather = new()
        {
            {"london", -1 },
            {"tbilisi", 20 },
            {"tashkent", 30},
            {"warshava", 40 },
        };

        Task<List<Weather>> WeatherObjectsAsync(DateTime from, DateTime to, string city)
        {
            return Task.Run(() =>
            WeatherObjects.Where(x => x.CreatedOn > from && x.CreatedOn < to && x.CityName == city).ToList());
        }

        Task<double> Double(string city)
        {
            return Task.Run(() =>
            _weather[city]);
        }

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

        public Task<double> AddWeather(string city)
        {
            double tmpdegreesc = Double(city).Result;

            switch (tmpdegreesc)
            {
                case < 0:
                    WeatherService.comment = new CommentContext(new DressWarmlyStrategy());
                    break;
                case < 21:
                    WeatherService.comment = new CommentContext(new FreshStrategy());
                    break;
                case < 31:
                    WeatherService.comment = new CommentContext(new GoodWeatherStrategy());
                    break;
                case > 30:
                    WeatherService.comment = new CommentContext(new BeachTimeStrategy());
                    break;
                default:
                    break;
            }

            return Double(city);
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
        public void GetComment_CurrentTemperature_CommentForLondon()
        {
            //Arrange
            double C = AddWeather("london").Result;

            //Act
            bool result = false;

            if (WeatherService.comment.GetComment() == "Dress warmly")
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetComment_CurrentTemperature_CommentForTbilisi()
        {
            //Arrange
            double C = AddWeather("tbilisi").Result;

            //Act
            bool result = false;

            if (WeatherService.comment.GetComment() == "It's fresh")
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetComment_CurrentTemperature_CommentForTashkent()
        {
            //Arrange
            double C = AddWeather("tashkent").Result;

            //Act
            bool result = false;

            if (WeatherService.comment.GetComment() == "Good weather")
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetComment_CurrentTemperature_CommentForWarshava()
        {
            //Arrange
            double C = AddWeather("warshava").Result;

            //Act
            bool result = false;

            if (WeatherService.comment.GetComment() == "it's time to go to the beach")
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetReports_InputLondon_Count2()
        {
            //Arrange
            var returnedLondonObjects = Getreport(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), "london");

            //Act
            int count = returnedLondonObjects.Result.Count;

            //Assert
            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void GetReports_InputTbilisi_Count1()
        {
            //Arrange
            var returnedLondonObjects = Getreport(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), "tbilisi");

            //Act
            int count = returnedLondonObjects.Result.Count;

            //Assert
            Assert.AreEqual(count, 1);
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

        public async Task<List<Weather>> Getreport(DateTime from, DateTime to, string city)
        {
            var result = await WeatherObjectsAsync(from, to, city);
            return result;
        }

        public Task<string> AverageStatistics(string[] city, string[] period)
        {
            throw new NotImplementedException();
        }

        public Task<string> AverageStatistics(string city, string period)
        {
            throw new NotImplementedException();
        }

        void IWeatherService.AddAsync(Weather user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public void Subcribe(string name)
        {
            throw new NotImplementedException();
        }

        public void UnSubcribe(string name)
        {
            throw new NotImplementedException();
        }
    }
}