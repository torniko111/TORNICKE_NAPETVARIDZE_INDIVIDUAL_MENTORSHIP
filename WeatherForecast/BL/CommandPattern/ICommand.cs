using System;
using System.Collections.Generic;
using System.Text;

namespace BL.CommandPattern
{
    public interface  ICommand
    {
        CommandAction Action { get; }
        void Execute();
    }
}
