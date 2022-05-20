using BL;
using BL.Interfaces;
using DAL.data;
using DAL.IRepositories;
using DAL.Models;
using DAL.Repositories;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherSettings"));
string city = builder.Configuration.GetValue<string>("WeatherSettings:Cities");
string Cron = builder.Configuration.GetValue<string>("WeatherSettings:Cron");


builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration.GetValue<string>("WeatherSettings:LogPath"), rollingInterval: RollingInterval.Minute, outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IConfigurationReader, ConfigurationReader>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IWeatherService, WeatherService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHangfireDashboard("/hangfire");
app.MapControllers();

using (var connection = JobStorage.Current.GetConnection())
{
    foreach (var recurringJob in connection.GetRecurringJobs())
    {
        app.Services.GetRequiredService<IOptionsMonitor<WeatherSettings>>().OnChange(config => RecurringJob.RemoveIfExists(recurringJob.Id));
    }
}


if (city.Contains(","))
{
    string[] cities = city.Split(',');
    if (Cron.Contains(","))
    {
        string[] Crons = Cron.Split(',');
        for (int i = 0; i < cities.Length; i++)
        {
            RecurringJob.AddOrUpdate<IUserService>(x => x.GetCurrentWeatherByCity(cities[i]), Crons[i]);
        }
    }
    else
    {
        //one db call
        RecurringJob.AddOrUpdate<IUserService>("mcxeta", x => x.GetCurrentWeatherByCitiesSameTime(cities), Cron);
    }
}
else
{
    if (Cron.Contains(","))
    {
        string[] Crons = Cron.Split(',');
        RecurringJob.AddOrUpdate<IUserService>(x => x.GetCurrentWeatherByCity(city), Crons[0]);
    }
    else
    {
        RecurringJob.AddOrUpdate<IUserService>(x => x.GetCurrentWeatherByCity(city), Cron);
    }
}


app.Run();