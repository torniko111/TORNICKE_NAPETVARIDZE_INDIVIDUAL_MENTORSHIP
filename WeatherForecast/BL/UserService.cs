
using BL.Interfaces;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWeatherRepository weatherRepository;
        public static CommentContext comment;



        public UserService(IUserRepository userRepository, IWeatherRepository weatherRepository)
        {
            _userRepository = userRepository;
            this.weatherRepository = weatherRepository;
        }

        public async Task<User> AddAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public Task DeleteAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<double> AddWeather(string city)
        {
            if (string.IsNullOrWhiteSpace(city) || city.Length == 1)
            {
                throw new ArgumentNullException(nameof(city));
            }

            WeatherApiModel obj = await GetCurrWeather(city);

            var weather = new Weather()
            {
                Lat = obj.coord.lat,
                Lon = obj.coord.lon,
            };
            await weatherRepository.AddAsync(weather);
            var tmpdegreesc = Math.Round(((float)obj.main.temp - 272.15), 2);


            if (tmpdegreesc > 16)
            {
                comment = new CommentContext(new FreshStrategy());
            }
            else
            {
                comment = new CommentContext(new WarmlyStrategy());
            }
            Console.WriteLine(comment.Comment());

            return tmpdegreesc;
        }

        private async Task<WeatherApiModel> GetCurrWeather(string city)
        {
            //needs to be moved to appsettings
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org");

            var response = await client.GetAsync($"/data/2.5/weather?q={city}&units=metric&appid={apikey}");
            var stringResult = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<WeatherApiModel>(stringResult);
            return obj;
        }

        public async Task<Dictionary<double, string>> GetWeatherForecast(string city, int days)
        {
            Dictionary<double, string> result = new Dictionary<double, string>();
            if (string.IsNullOrWhiteSpace(city) || city.Length == 1)
            {
                throw new ArgumentNullException(nameof(city));
            }

            //needs to be moved to appsettings
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org");
            double lat;
            double lon;
            var latlon = await client.GetAsync($"/data/2.5/weather?q={city}&appid={apikey}");
            var stringResult2 = await latlon.Content.ReadAsStringAsync();
            var obj2 = JsonConvert.DeserializeObject<dynamic>(stringResult2);

            lat = (double)obj2.coord.lat;
            lon = (double)obj2.coord.lon;

            var response = await client.GetAsync($"/data/2.5/forecast?lat={lat}&lon={lon}&cnt={days}&units=metric&appid={apikey}");
            var stringResult = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<dynamic>(stringResult);
            var list = obj.list;
            int i = 0;
            foreach (var item in list)
            {
                var tmpdegreesc = (double)item.main.temp;

                if (tmpdegreesc > 16)
                {
                    comment = new CommentContext(new FreshStrategy());
                }
                else
                {
                    comment = new CommentContext(new WarmlyStrategy());
                }


                result.Add(tmpdegreesc, comment.Comment());

            }

            return result;
        }

        public Task<List<User>> ListAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new System.NotImplementedException();
        }


        public async Task<MaxTemperatureModel> GetMaxCurrentTemperature(string[] cities)
        {
            var tasks = new List<Task>();
            var result = new MaxTemperatureModel();
            foreach (var city in cities)
            {

                var task = Task.Run(async () =>
                {

                    Stopwatch st = Stopwatch.StartNew();
                    var temp = await GetCurrWeather(city);
                    st.Stop();
                    result.Temperatures.Add(new MaxTemperatureModel.TemperatureDate() {City= city, Miliseconds = st.Elapsed.Milliseconds, Temperature = temp.main.temp });
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            return result;
        }
    }
}