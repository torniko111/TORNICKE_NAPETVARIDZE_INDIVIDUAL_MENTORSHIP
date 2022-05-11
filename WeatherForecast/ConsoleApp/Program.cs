using BL;
using BL.Interfaces;
using DAL.data;
using DAL.IRepositories;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WeatherDb;Trusted_Connection=True;MultipleActiveResultSets=true"));
                    services.AddScoped<IWeatherRepository, WeatherRepository>();
                    services.AddScoped<IRoleRepository, RoleRepository>();
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddTransient<IRoleService, RoleService>();
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<IWeatherService, WeatherService>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<WeatherService>(host.Services);
            UserService svc1 = ActivatorUtilities.CreateInstance<UserService>(host.Services);
            int nav = 0;
            while (nav != 1 || nav != 2 || nav != 3)
            {
                Console.WriteLine("please choose numbers 1-3: 1. current weather by city, 2.weather forecast by city and number of days 3. exit.");

                int.TryParse(Console.ReadLine(), out nav);
                string city;
                if (nav == 1)
                {
                    Console.WriteLine("enter City: ");
                    city = Console.ReadLine();
                    city = Regex.Replace(city, @"\s+", "");
                    if (city.Contains(","))
                    {
                        string[] namesArray = city.Split(',');

                        var dateobjects = await svc1.GetMaxCurrentTemperature(namesArray);

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

                        var tmpDegreesC = svc1.AddWeather(city).Result;
                        Console.WriteLine($"Current temperature is {tmpDegreesC}°C in {city}");
                        Console.ReadKey();
                    }
                    if (nav == 2)
                    {
                        Console.WriteLine("enter City: ");
                        city = Console.ReadLine();

                        Console.WriteLine("enter Days of forecast: ");
                        //needs to be checked
                        int daysofforecast = int.Parse(Console.ReadLine());
                        var tmpDegreesC = svc1.GetWeatherForecast(city, daysofforecast).Result;
                        foreach (KeyValuePair<double, string> author in tmpDegreesC)
                        {
                            Console.WriteLine("in {0} is : {1} , Comment: {2}",
                                city, author.Key, author.Value);
                        }
                        Console.ReadKey();
                    }
                    if (nav == 3)
                    {
                        Console.WriteLine("Exited");
                        Console.ReadLine();
                        break;
                    }
                }
            }
        }
            static void BuildConfig(IConfigurationBuilder builder)
            {
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .AddEnvironmentVariables();
            }
        }
    } 
