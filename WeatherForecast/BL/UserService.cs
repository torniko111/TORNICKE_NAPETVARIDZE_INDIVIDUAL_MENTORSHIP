
using BL.Interfaces;
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
        public static CommentContext comment;
        public static double[][] arrays;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<double> GetWeatherApi(string city)
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
                comment = new CommentContext(new FreshStrategy());
            }
            else
            {
                comment = new CommentContext(new WarmlyStrategy());
            }
            Console.WriteLine(comment.Comment());

            return tmpdegreesc;
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

        //public  Task GetMaxCurrentTemperature(string[] cities)
        //{
        //    Stopwatch st = new Stopwatch();
        //    int length = cities.Length;
        //    for (int i = 0; i < length; i++)
        //    {
        //        arrays[i] = new double[2];
        //    }

        //    for (int i = 0; i < length; i++)
        //    {
        //        st.Start();
        //        var c = Task.Run(() => GetWeatherApi(cities[i]));
        //        st.Stop();
                

        //        arrays[i][0] = st.Elapsed.Milliseconds;
        //        arrays[i][1] = c.Result;
        //    }

        //    return null;
        //}
    }
}