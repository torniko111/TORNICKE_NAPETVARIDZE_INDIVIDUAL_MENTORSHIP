using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class CommentContext
    {
        private IChooseCommentStrategy ChooseCommentStrategy;

        public CommentContext(IChooseCommentStrategy Comment)
        {
            this.ChooseCommentStrategy = Comment;
        }
        public void SetStrategy(IChooseCommentStrategy Comment)
        {
            this.ChooseCommentStrategy = Comment;
        }

        public void Comment(string city)
        {
            ChooseCommentStrategy.Comment(city);
        }

        public void ChooseComment()
        {
            Console.WriteLine("please text city");
        }

    }
}
