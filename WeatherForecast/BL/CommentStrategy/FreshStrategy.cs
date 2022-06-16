using BL.CommentStrategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class FreshStrategy : IChooseCommentStrategy
    {
        public string Comment()
        {
            return CommentConstants.FRESH;
        }
    }
}
