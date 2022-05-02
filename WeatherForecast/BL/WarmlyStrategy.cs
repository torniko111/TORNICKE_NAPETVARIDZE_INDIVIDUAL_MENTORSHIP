using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class WarmlyStrategy : IChooseCommentStrategy
    {
        public void Comment(string City)
        {
            Console.WriteLine("its warm");
        }
    }
}
