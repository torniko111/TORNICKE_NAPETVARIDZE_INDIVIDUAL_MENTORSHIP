using BL.Interfaces;
using DAL.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService userService, ILogger<WeatherController> logger, IRecurringJobManager recurringJobManager)
        {
            _weatherService = userService;
            _logger = logger;
        }

        [Authorize(Roles ="member")]
        [HttpGet("getCurrentWeatherByCity")]
        public async Task<double> getCurrTemp(string city)
        {
            return await _weatherService.AddWeather(city);
        }

        [HttpGet("getWeatherForecastByCityAndNumberOfDays")]
        public async Task<Dictionary<double, string>> getweatherforecast(string city, int days)
        {
            return await _weatherService.GetWeatherForecast(city, days);
        }

        [HttpGet("reportsByDate")]
        public async Task<List<Weather>> getreports(DateTime from, DateTime to, string city)
        {
            return await _weatherService.getreport(from, to, city);
        }
    }
}
