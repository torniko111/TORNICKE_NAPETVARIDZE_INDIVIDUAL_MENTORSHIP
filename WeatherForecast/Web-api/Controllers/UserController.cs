using BL.Interfaces;
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

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
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

    }
}
