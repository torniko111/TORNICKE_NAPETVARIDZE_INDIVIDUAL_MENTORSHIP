
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.CommandPattern
{
    public class ChooseCity : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("please enter city: ");
        }
        public CommandAction Action
        {
            get { return CommandAction.City; }
        }
    }
}
