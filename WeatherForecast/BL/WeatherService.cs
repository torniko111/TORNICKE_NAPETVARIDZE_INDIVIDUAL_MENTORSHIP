
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
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        public static CommentContext comment;
        readonly Stopwatch st = new();


        public WeatherService(IWeatherRepository weatherRepository)
        {
            this._weatherRepository = weatherRepository;
        }

        public void AddAsync(Weather weather)
        {
             _weatherRepository.Add(weather);
        }

        public Task DeleteAsync(Weather weather)
        {
            throw new System.NotImplementedException();
        }
        public async Task<List<Weather>> Getreport(DateTime from, DateTime to, string city)
        {
            return await _weatherRepository.GetByDateRange(from, to, city);
        }
        public Task<Weather> GetByIdAsync(int id)
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
                Lat = obj.CityCoordinates.Lat,
                Lon = obj.CityCoordinates.Lon,
                CityName = obj.Name,
                TempC = obj.Celsius.Temp

            };
            _weatherRepository.Add(weather);
            var tmpdegreesc = Math.Round(((float)obj.Celsius.Temp), 2);


            if (tmpdegreesc > 16)
            {
                comment = new CommentContext(new FreshStrategy());
            }
            else
            {
                comment = new CommentContext(new WarmlyStrategy());
            }
            Console.WriteLine(comment.GetComment());

            return tmpdegreesc;
        }

        private async Task<WeatherApiModel> GetCurrWeather(string city, CancellationToken cancellation)
        {
            //needs to be moved to appsettings
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new();
            client.BaseAddress = new Uri("https://api.openweathermap.org");
            st.Start();
            var response = await client.GetAsync($"/data/2.5/weather?q={city}&units=metric&appid={apikey}", cancellation);
            var stringResult = await response.Content.ReadAsStringAsync(cancellation);
            st.Stop();
            var obj = JsonConvert.DeserializeObject<WeatherApiModel>(stringResult);

            return obj;
        }

        public async Task<Dictionary<double, string>> GetWeatherForecast(string city, int days)
        {
            Dictionary<double, string> result = new();
            if (string.IsNullOrWhiteSpace(city) || city.Length == 1)
            {
                throw new ArgumentNullException(nameof(city));
            }

            //needs to be moved to appsettings
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new();
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


                result.Add(tmpdegreesc, comment.GetComment());

            }

            return result;
        }

        public Task<List<Weather>> ListAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(Weather weather)
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
                            if (temp.ResponseStatusCode == 404)
                            {
                                failed++;
                                result.Failed = failed;
                            }
                            else
                            {
                                result.Temperatures.Add(new MaxTemperatureModel.TemperatureDate() { City = city, Miliseconds = st.Elapsed.Milliseconds, Temperature = temp.Celsius.Temp });
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

        public async Task GetCurrentWeatherByCity(string city)
        {
            CancellationToken cancellationToken = CancellationToken.None;
            if (string.IsNullOrWhiteSpace(city) || city.Length == 1)
            {
                throw new ArgumentNullException(nameof(city));
            }

            WeatherApiModel obj = await GetCurrWeather(city, cancellationToken);

            var weather = new Weather()
            {
                Lat = obj.CityCoordinates.Lat,
                Lon = obj.CityCoordinates.Lon,
                CityName = obj.Name,
                TempC = obj.Celsius.Temp

            };
            _weatherRepository.Add(weather);

        }

        public async Task GetCurrentWeatherByCitiesSameTime(string[] city)
        {
            CancellationToken cancellationToken = CancellationToken.None;
            List<Weather> weathers = new();

            for (int i = 0; i < city.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(city[i]) || city.Length == 1)
                {
                    throw new ArgumentNullException(nameof(city));
                }

                WeatherApiModel obj = await GetCurrWeather(city[i], cancellationToken);

                weathers.Add(new Weather()
                {
                    Lat = obj.CityCoordinates.Lat,
                    Lon = obj.CityCoordinates.Lon,
                    CityName = obj.Name,
                    TempC = obj.Celsius.Temp
                });
            }
            await _weatherRepository.AddRange(weathers);
        }

        Task<Weather> IWeatherService.AddAsync(Weather weather)
        {
            throw new NotImplementedException();
        }
    }
}