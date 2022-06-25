using BL;
using BL.CommandPattern;
using BL.Interfaces;
using DAL.data;
using DAL.IRepositories;
using DAL.TypeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Text.RegularExpressions;

var builder = new ConfigurationBuilder();
BuildConfig(builder);
IConfiguration confing = builder.Build();

Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Application Starting");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(confing.GetConnectionString("DefaultConnection")));
        services.AddScoped<IWeatherRepository, WeatherRepository>();
        services.AddTransient<IWeatherService, WeatherService>();
    })
    .UseSerilog()
    .Build();

WeatherService svc = ActivatorUtilities.CreateInstance<WeatherService>(host.Services);
int nav = 0;
Console.WriteLine("please choose numbers 1-3: 1. current weather by city, 2.weather forecast by city and number of days 3. exit.");

int.TryParse(Console.ReadLine(), out nav);
if(nav == 0)
{
    Console.WriteLine("please enter valid city name: ");
}

while (nav != 1 || nav != 2 || nav != 3)
{
    Invoker invoker = new();

    string city;
    if (nav == 1)
    {
        ICommand command = invoker.GetCommand(CommandAction.City);
        command.Execute();
        city = Console.ReadLine();
        city = Regex.Replace(city, @"\s+", "");
        if (city.Contains(','))
        {
            string[] namesArray = city.Split(',');

            var dateobjects = await svc.GetMaxCurrentTemperature(namesArray);

            foreach (var item in dateobjects.Temperatures)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine($"failed {dateobjects.Failed}, Canceled: {dateobjects.Canceled}");
        }
        else
        {
            while (string.IsNullOrWhiteSpace(city) || city.Length < 2)
            {
                Console.WriteLine("please enter valid city name: ");
                city = Console.ReadLine();
            }

            var tmpDegreesC = svc.AddWeather(city).Result;
            Console.WriteLine($"Current temperature is {tmpDegreesC}°C in {city}");
            Console.ReadKey();
        }
    }
    if (nav == 2)
    {
        ICommand commands = invoker.GetCommand(CommandAction.CityForecast);
        commands.Execute();
        city = Console.ReadLine();

        Console.WriteLine("enter Days of forecast: ");
        //needs to be checked
        int daysofforecast = int.Parse(Console.ReadLine());
        var tmpDegreesC = svc.GetWeatherForecast(city, daysofforecast).Result;
        foreach (KeyValuePair<double, string> author in tmpDegreesC)
        {
            Console.WriteLine("in {0} is : {1} , Comment: {2}",
                city, author.Key, author.Value);
        }
        Console.ReadKey();
    }
    if (nav == 3)
    {
        ICommand command = invoker.GetCommand(CommandAction.City);
        command.Execute();
        command = invoker.GetCommand(CommandAction.Stop);
        command.Execute();
        Console.ReadLine();
        break;
    }

}
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }