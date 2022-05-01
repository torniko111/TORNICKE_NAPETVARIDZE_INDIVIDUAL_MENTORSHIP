// See https://aka.ms/new-console-template for more information

using BussinessLayer;
Random rnd = new Random();

int temperature  = rnd.Next(0, 30);


if (temperature > 15)
{
    CommentContext first = new CommentContext(new FreshStrategy());
    first.ChooseComment("moscow");
} else
{
    CommentContext second = new CommentContext(new WarmlyStrategy());
    second.ChooseComment("1");
}



