using System;
using System.Collections.Generic;
using System.Text;

namespace BL.CommandPattern
{
    public class Invoker
    {
        ICommand cmd = null;
        public ICommand GetCommand(CommandAction action)
        {
            switch (action)
            {
                case CommandAction.City:
                    cmd = new ChooseCity();
                    break;
                case CommandAction.CityForecast:
                    cmd = new ChooseCityForecast();
                    break;
                case CommandAction.Stop:
                    cmd = new StopCommand();
                    break;
                default:
                    break;
            }
            return cmd;
        }
    }
}
