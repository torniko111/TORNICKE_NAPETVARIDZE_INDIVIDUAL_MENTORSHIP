using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class AverageTemperature
    {
        private readonly int _periodOfHours;

        public IEnumerable<KeyValuePair<int, double>> MaxTemperature
        {
            get
            {
                return (IEnumerable<KeyValuePair<int, double>>)ListOfCitiesC.GroupBy(g => g.Name, s => s.Celsius).Select(g => new
                {
                    StreamId = g.Key,
                    AvgScore = g.Average()
                });
            }
        }
            
        public List<Temperatures> ListOfCitiesC { get; set; } = new();
        public class Temperatures
        {
            public string Name { get; set; }
            public double Celsius { get; set; }
        }
    }
}
