using BL.Interfaces;
using DAL.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class ConfigurationReader : IConfigurationReader
    {
        private WeatherSettings dashboardHeaderConfig;

        public ConfigurationReader(IOptionsMonitor<WeatherSettings> optionsMonitor)
        {
            this.dashboardHeaderConfig = optionsMonitor.CurrentValue;
            optionsMonitor.OnChange(config =>
            {
                this.dashboardHeaderConfig = config;
            });
        }

        public string ReadDashboardHeaderSettings()
        {
            return JsonConvert.SerializeObject(this.dashboardHeaderConfig);
        }
    }
}
