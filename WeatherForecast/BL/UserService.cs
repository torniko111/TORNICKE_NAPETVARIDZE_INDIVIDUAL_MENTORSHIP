
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
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWeatherRepository weatherRepository;
        public static CommentContext comment;

        Stopwatch st = new Stopwatch();


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
            CancellationToken cancellationToken = CancellationToken.None;
            if (string.IsNullOrWhiteSpace(city) || city.Length == 1)
            {
                throw new ArgumentNullException(nameof(city));
            }

            WeatherApiModel obj = await GetCurrWeather(city, cancellationToken);

            var weather = new Weather()
            {
                Lat = obj.coord.lat,
                Lon = obj.coord.lon,
                CityName = obj.name,
                TempC = obj.main.temp
                
            };
            await weatherRepository.AddAsync(weather);
            var tmpdegreesc = Math.Round(((float)obj.main.temp), 2);


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

        private async Task<WeatherApiModel> GetCurrWeather(string city, CancellationToken cancellation)
        {
            //needs to be moved to appsettings
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org");
            st.Start();
            var response = await client.GetAsync($"/data/2.5/weather?q={city}&units=metric&appid={apikey}", cancellation);
            var stringResult = await response.Content.ReadAsStringAsync();
            st.Stop();
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
            int failed = 0;
            int canceled = 0;
            var result = new MaxTemperatureModel();

            foreach (var city in cities)
            {
                var clt = new CancellationTokenSource();
                clt.CancelAfter(200);
                var task = Task.Run(async () =>
                {
                    if (!clt.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var temp = await GetCurrWeather(city, clt.Token);
                            if (temp.cod == 404)
                            {
                                failed++;
                                result.Failed = failed;
                            } else
                            {
                                result.Temperatures.Add(new MaxTemperatureModel.TemperatureDate() { City = city, Miliseconds = st.Elapsed.Milliseconds, Temperature = temp.main.temp });
                            }
                        }
                        catch (Exception ex)
                        {
                            canceled++;
                            result.Canceled = canceled;
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("canceled");
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            return result;
        }

        public async Task<List<Weather>> getreport(DateTime from, DateTime to)
        {
            return await weatherRepository.ListAsync();
        }
    }
}