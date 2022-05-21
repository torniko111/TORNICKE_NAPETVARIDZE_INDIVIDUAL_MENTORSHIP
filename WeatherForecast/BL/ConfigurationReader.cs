﻿using BL.Interfaces;
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
        private Settings dashboardHeaderConfig;

        public ConfigurationReader(IOptionsMonitor<Settings> optionsMonitor)
        {
            this.dashboardHeaderConfig = optionsMonitor.CurrentValue;
            optionsMonitor.OnChange(config =>
            {
                this.dashboardHeaderConfig = config;
                RecurringJob.RemoveIfExists("IUserService.GetCurrentWeatherByCity");
            });
        }

        public string ReadDashboardHeaderSettings()
        {
            return JsonConvert.SerializeObject(this.dashboardHeaderConfig);
        }
    }
}
