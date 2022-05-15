using BL.Interfaces;
using DAL.Models;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IRecurringJobManager recurringJobManager;

        public UserController(IUserService userService, ILogger<UserController> logger, IRecurringJobManager recurringJobManager)
        {
            _userService = userService;
            _logger = logger;
            this.recurringJobManager = recurringJobManager;
        }


        [HttpGet("getcurrweatherbycity")]
        public async Task<double> getCurrTemp(string city)
        {
            return await _userService.AddWeather(city);
        }

        [HttpGet("getweatherforecastbycityandnumberofdays")]
        public async Task<Dictionary<double, string>> getweatherforecast(string city, int days)
        {
            return await _userService.GetWeatherForecast(city, days);
        }


        [HttpGet("reccuring")]
        public async Task reccuring(string cities)
        {
            if (cities.Contains(","))
            {
                string[] citys = cities.Split(',');

                for (int i = 0; i < citys.Length; i++)
                {
                    recurringJobManager.AddOrUpdate($"addedrecc for city {citys[i]}", () => getCurrTemp(citys[i]), Cron.Minutely());
                }
            }
            else
            {
                recurringJobManager.AddOrUpdate($"addedrecc for city {cities}", () => getCurrTemp(cities), Cron.Minutely());
            }

        }
    }
}
