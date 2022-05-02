using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class FreshStrategy : IChooseCommentStrategy
    {
        public void Comment(string City)
        { 
            Console.WriteLine("its fresh");
        }
    }
}
