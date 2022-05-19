using BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Dashboard3Controller : ControllerBase
    {
        private readonly IConfigurationReader configurationReader;

        public Dashboard3Controller(IConfigurationReader configurationReader)
        {
            this.configurationReader = configurationReader;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Content(this.configurationReader.ReadDashboardHeaderSettings());
        }
    }
}
