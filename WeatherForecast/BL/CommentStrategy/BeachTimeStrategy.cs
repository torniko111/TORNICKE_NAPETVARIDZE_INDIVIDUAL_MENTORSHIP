using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CommentStrategy
{
    public class BeachTimeStrategy : IChooseCommentStrategy
    {
        public string Comment()
        {
            return CommentConstants.BEACH_TIME;
        }
    }
}
