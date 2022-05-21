using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Weather : BaseEntity
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Clouds { get; set; }
        public double TempC { get; set; }
        public double TempCMin { get; set; }
        public double TempCMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public int Visibility { get; set; }
        public int WindSpeed { get; set; }
        public string Country { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
        //public TimeZone TimeZone { get; set; }
        public string CityName { get; set; }
    }
}
