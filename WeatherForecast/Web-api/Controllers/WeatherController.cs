using BL;
using BL.Interfaces;
using BL.MailService;
using BL.Models;
using DAL.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IEmailService _emailService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public WeatherController(IWeatherService userService, IEmailService emailService, IRabbitMqPublisher rabbitMqPublisher)
        {
            _weatherService = userService;
            _emailService = emailService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [Authorize(Roles = "member")]
        [HttpGet("getCurrentWeatherByCity")]
        public async Task<double> GetCurrTemp(string city)
        {
            return await _weatherService.AddWeather(city);
        }

        [HttpGet("getWeatherForecastByCityAndNumberOfDays")]
        public async Task<Dictionary<double, string>> GetWeatherForecast(string city, int days)
        {
            return await _weatherService.GetWeatherForecast(city, days);
        }
        [Authorize(Roles = "member")]
        [HttpGet("reportsByDate")]
        public async Task<List<Weather>> Getreports(DateTime from, DateTime to, string city)
        {
            return await _weatherService.Getreport(from, to, city);
        }

        [HttpGet("averageTemperatures")]
        public async Task<string> Averagetemperatures(string city, string period)
        {
            return await _weatherService.AverageStatistics(city, period);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAllUsers")]
        public List<string> AllUsers()
        {
            return _rabbitMqPublisher.GetAllUsersEmails();
        }

        [HttpPut("Subscribe")]
        public void Subscribe(string name)
        {
            _weatherService.Subcribe(name);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("UnSubscribe")]
        public void UnSubscribe(string name)
        {
            _weatherService.UnSubcribe(name);
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }

        [HttpPost("rabbitMQ")]
        public IActionResult SendMessageToRabbitMQ()
        {
            _rabbitMqPublisher.SendMessage();
            return Ok();
        }
    }
}