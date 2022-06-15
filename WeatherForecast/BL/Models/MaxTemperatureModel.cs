using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class MaxTemperatureModel
    {
        public TemperatureDate MaxTemperature { get { return Temperatures.OrderByDescending(x => x.Temperature).FirstOrDefault(); } }
        public List<TemperatureDate> Temperatures { get; set; } = new List<TemperatureDate>();
        public int Failed { get; set; }
        public int Canceled { get; set; }

        public class TemperatureDate 
        {
            public string City { get; set; }
            public double Temperature { get; set; }
            public double Miliseconds { get; set; }
            public string Failedorcanceled { get; set; }


            public override string ToString()
            {
                return $"city = {City}, C : {Temperature}, ExecutionTime: {Miliseconds}";
            }
        }
    }


}
