using DatabaseFirstSample.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

var db = new ScaffolldedContext();
SemaphoreSlim _semaphoregate = new SemaphoreSlim(1);

// Create and save a new Blog
Console.Write("Enter a name for a new Blog: ");
var name = Console.ReadLine();

var blog = new Blog { Name = name };
db.Blogs.Add(blog);
db.SaveChanges();

// Display all Blogs from the database

async Task<IEnumerable<string>> BlogsName()
{
    await _semaphoregate.WaitAsync();
    var result = await db.Blogs.Select(b => b.Name).ToListAsync();
    _semaphoregate.Release();

    return result;
}

async Task<IEnumerable<Blog>> Blogs()
{
    await _semaphoregate.WaitAsync();
    var result = await db.Blogs.Select(b => b).ToListAsync();
    _semaphoregate.Release();
    return result;
}

var blogsString = await BlogsName();
var blogs = await Blogs();

Console.WriteLine("All blogs in the database:");
foreach (var item in blogsString)
{
    Console.WriteLine(item);
}

foreach (var item in blogs)
{
    Console.WriteLine(item.Name);
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();