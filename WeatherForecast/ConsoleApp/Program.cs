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
using System.IO;
using System.Threading.Tasks;


namespace ConsoleApp
{
    internal class Program
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
            var svc1 = ActivatorUtilities.CreateInstance<UserService>(host.Services);



            Console.WriteLine("enter City: ");
            string city = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(city) || city.Length < 2)
            {
                Console.WriteLine("please enter valid city name: ");
                city = Console.ReadLine();
            }


            var tmpDegreesC = UserService.GetWeatherApi(city).Result;


            Console.WriteLine($"Current temperature is {tmpDegreesC}°C in {city}");



            Console.ReadKey();
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
