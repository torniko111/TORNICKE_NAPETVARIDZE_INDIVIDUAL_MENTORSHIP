using BL;
using BL.Interfaces;
using DAL.data;
using IsRoleDemo.Data;
using DAL.IRepositories;
using DAL.Models;
using DAL.TypeRepository;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using IsRoleDemo.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherSettings"));


builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration.GetValue<string>("WeatherSettings:LogPath"), rollingInterval: RollingInterval.Minute, outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "myApi";
        options.Authority = "https://localhost:5001";
    });

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IConfigurationReader, ConfigurationReader>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire");
app.MapControllers();

app.Services.GetRequiredService<IOptionsMonitor<WeatherSettings>>().OnChange(config => AllReccuringJobsDeleter());
app.Services.GetRequiredService<IOptionsMonitor<WeatherSettings>>().OnChange(config => CallWeather(builder.Configuration.GetValue<string>("WeatherSettings:Cities"), builder.Configuration.GetValue<string>("WeatherSettings:Cron")));
static void CallWeather(string city, string Cron)
{
    if (city.Contains(','))
    {
        string[] cities = city.Split(',');
        if (Cron.Contains(','))
        {
            string[] Crons = Cron.Split(',');
            for (int i = 0; i < cities.Length; i++)
            {
                RecurringJob.AddOrUpdate<WeatherService>($"{cities[i]}", x => x.GetCurrentWeatherByCity(cities[i]), Crons[i]);
            }
        }
        else
        {
            //one db call
            RecurringJob.AddOrUpdate<WeatherService>($"{cities[0]}", x => x.GetCurrentWeatherByCitiesSameTime(cities), Cron);
        }
    }
    else
    {
        if (Cron.Contains(','))
        {
            string[] Crons = Cron.Split(',');
            RecurringJob.AddOrUpdate<WeatherService>($"{city}", x => x.GetCurrentWeatherByCity(city), Crons[0]);
        }
        else
        {
            RecurringJob.AddOrUpdate<WeatherService>($"{city}", x => x.GetCurrentWeatherByCity(city), Cron);
        }
    }
}

static void AllReccuringJobsDeleter()
{
    using var connection = JobStorage.Current.GetConnection();
    foreach (var recurringJob in connection.GetRecurringJobs())
    {
        RecurringJob.RemoveIfExists(recurringJob.Id);
    }
}
app.Run();