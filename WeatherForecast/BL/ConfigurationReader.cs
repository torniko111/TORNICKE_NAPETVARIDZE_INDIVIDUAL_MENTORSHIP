using BL.Interfaces;
using DAL.Models;
using Hangfire;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class ConfigurationReader : IConfigurationReader
    {
        private WeatherSettings _dashboardHeaderConfig;

        public ConfigurationReader(IOptionsMonitor<WeatherSettings> optionsMonitor)
        {
            this._dashboardHeaderConfig = optionsMonitor.CurrentValue;
            optionsMonitor.OnChange(config =>
            {
                this._dashboardHeaderConfig = config;
            });
        }

        public string ReadDashboardHeaderSettings()
        {
            return JsonConvert.SerializeObject(this._dashboardHeaderConfig);
        }
    }
}
