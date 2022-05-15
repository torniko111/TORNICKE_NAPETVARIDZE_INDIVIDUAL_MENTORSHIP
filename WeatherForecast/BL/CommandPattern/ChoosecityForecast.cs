using System;
using System.Collections.Generic;
using System.Text;

namespace BL.CommandPattern
{
    public class ChooseCityForecast : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("please cities for forecast: ");
        }
        public string Name
        {
            get { return "2"; }
        }
    }
}
