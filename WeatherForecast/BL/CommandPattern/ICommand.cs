using System;
using System.Collections.Generic;
using System.Text;

namespace BL.CommandPattern
{
    public interface  ICommand
    {
        string Name { get; }
        void Execute();
    }
}
