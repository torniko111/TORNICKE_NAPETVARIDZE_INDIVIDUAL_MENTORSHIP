using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class WeatherApiModel
    {
        [JsonProperty("Coord")]
        public CoordModel CityCoordinates { get; set; }

        [JsonProperty("Weather")]
        public WeatherModel[] WeatherType { get; set; }

        [JsonProperty("_base")]
        public string InformationSource { get; set; }

        [JsonProperty("main")]
        public MainModel Celsius { get; set; }

        [JsonProperty("mvisibility")]
        public int Visibility { get; set; }

        [JsonProperty("wind")]
        public WindModel Wind { get; set; }

        [JsonProperty("clouds")]
        public CloudsModel Clouds { get; set; }

        [JsonProperty("dt")]
        public int CurrentTime { get; set; }

        [JsonProperty("sys")]
        public SysModel Country { get; set; }

        [JsonProperty("timezone")]
        public int Timezone { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cod")]
        public int ResponseStatusCode { get; set; }


        public class CoordModel
        {
            [JsonProperty("lon")]
            public float Lon { get; set; }

            [JsonProperty("lat")]
            public float Lat { get; set; }
        }

        public class MainModel
        {
            [JsonProperty("temp")]
            public float Temp { get; set; }

            [JsonProperty("feels_like")]
            public float FeelsLike { get; set; }

            [JsonProperty("temp_min")]
            public float TempMin { get; set; }

            [JsonProperty("temp_max")]
            public float TempMax { get; set; }

            [JsonProperty("pressure")]
            public int Pressure { get; set; }

            [JsonProperty("humidity")]
            public int Humidity { get; set; }
        }

        public class WindModel
        {
            [JsonProperty("speed")]
            public float Speed { get; set; }

            [JsonProperty("deg")]
            public int Deg { get; set; }
        }

        public class CloudsModel
        {
            [JsonProperty("all")]
            public int Cloudiness { get; set; }
        }

        public class SysModel
        {
            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("sunrise")]
            public int Sunrise { get; set; }

            [JsonProperty("sunset")]
            public int Sunset { get; set; }
        }

        public class WeatherModel
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("main")]
            public string WetOrDry { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("icon")]
            public string WeatherIconId { get; set; }
        }

    }
}
