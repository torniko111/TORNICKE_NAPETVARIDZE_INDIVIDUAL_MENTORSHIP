using BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Dashboard3Controller : ControllerBase
    {
        private readonly IConfigurationReader _configurationReader;

        public Dashboard3Controller(IConfigurationReader configurationReader)
        {
            this._configurationReader = configurationReader;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Content(this._configurationReader.ReadDashboardHeaderSettings());
        }
    }
}
