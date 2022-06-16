using BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppsSettingsChangesTrackerController : ControllerBase
    {
        private readonly IConfigurationReader _configurationReader;

        public AppsSettingsChangesTrackerController(IConfigurationReader configurationReader)
        {
            this._configurationReader = configurationReader;
        }
        [HttpGet("AppSettingsWatcher")]
        public IActionResult Index()
        {
            return Content(this._configurationReader.ReadDashboardHeaderSettings());
        }
    }
}
