using BL;
using BL.Interfaces;
using BL.MailService;
using DAL.data;
using DAL.IRepositories;
using DAL.Models;
using DAL.TypeRepository;
using Hangfire;
using Hangfire.Storage;
using IdentityServer4.AccessTokenValidation;
using IsRoleDemo.Data;
using IsRoleDemo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using MassTransit;
using BL.Models;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherSettings"));
var mailSendingInerval = builder.Configuration.GetValue<string>("MailSendingInterval");

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration.GetValue<string>("WeatherSettings:LogPath"), rollingInterval: RollingInterval.Minute, outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "IdentityServer.Demo.Api",
            Version = "v1",
        });
    c.CustomSchemaIds(x => x.FullName);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                },
                new List<string>()
            }
        });
});

builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        // base-address of your identityserver
        options.Authority = "https://localhost:5001";
        // name of the API resource
        options.ApiName = "myApi";
    });

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IConfigurationReader, ConfigurationReader>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddTransient<IRabbitMqPublisher, RabbitMqPublisher>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<EmailDto>();
builder.Services.AddScoped<ConsumerRabbitMQ>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ConsumerRabbitMQ>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        var uri = new Uri(builder.Configuration.GetValue<string>("ServiceBus:Uri"));
        cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
        cfg.UseMessageRetry(r => r.Immediate(5));
        cfg.Host(uri, host =>
        {
            host.Username(builder.Configuration.GetValue<string>("ServiceBus:Username"));
            host.Password(builder.Configuration.GetValue<string>("ServiceBus:Password"));
        });
        cfg.ReceiveEndpoint(builder.Configuration.GetValue<string>("ServiceBus:Queue"), c =>
        {
            c.ConfigureConsumer<ConsumerRabbitMQ>(ctx);
        });
    });
});

// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
app.Services.GetRequiredService<IOptionsMonitor<WeatherSettings>>().OnChange(config => LocalOrRabbitMq(builder.Configuration.GetValue<string>("WeatherSettings:Flag")));
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

app.Run();

static void AllReccuringJobsDeleter()
{
    using var connection = JobStorage.Current.GetConnection();
    foreach (var recurringJob in connection.GetRecurringJobs())
    {
        RecurringJob.RemoveIfExists(recurringJob.Id);
    }
}

static void MailSenderReccuringJobRabbitMQ()
{
    RecurringJob.AddOrUpdate<RabbitMqPublisher>($"MailSenderTrhoughtRabbitMQ", x => x.SendMessage(), $" * * * * * ");
}

static void MailSenderReccuringJob()
{
    RecurringJob.AddOrUpdate<RabbitMqPublisher>($"MailSenderDirectly", x => x.SendMessageDirectly(), $" * * * * * ");
}

static void LocalOrRabbitMq(string Flag)
{
    if (Flag == "Locally")
    {
        MailSenderReccuringJob();
    }
    else
    {
        MailSenderReccuringJobRabbitMQ();
    }
}