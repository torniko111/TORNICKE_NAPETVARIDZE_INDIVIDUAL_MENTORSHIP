using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class WeatherSettings
    {
        public string Cities { get; set; }
        public string Cron { get; set; }
        public string LogPath { get; set; }
        public string ApiKey { get; set; }
        public string Flag { get; set; }
    }
}
