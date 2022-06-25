using System;
using System.Collections.Generic;
using System.Text;

namespace BL.CommandPattern
{
    public class StopCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Exited");
        }
        public CommandAction Action
        {
            get { return CommandAction.Stop; }
        }
    }
}
