using System;
using System.Collections.Generic;
using System.Text;

namespace BL.CommandPattern
{
    public class Invoker
    {
        ICommand cmd = null;
        public ICommand GetCommand(string action)
        {
            switch (action)
            {
                case "1":
                    cmd = new ChooseCity();
                    break;
                case "2":
                    cmd = new ChooseCityForecast();
                    break;
                default:
                    break;
            }
            return cmd;
        }
    }
}
