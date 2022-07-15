using BL.MailService;
using BL.Models;
using DAL.IRepositories;
using IsRoleDemo.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWeatherRepository _weatherRepository;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailservice;

        public RabbitMqPublisher(IPublishEndpoint publishEndpoint, UserManager<ApplicationUser> userManager, IWeatherRepository weatherRepository, IConfiguration config, IEmailService emailservice)
        {
            _publishEndpoint = publishEndpoint;
            _userManager = userManager;
            _weatherRepository = weatherRepository;
            _config = config;
            _emailservice = emailservice;
        }

        public List<string> GetAllUsersEmails()
        {
            var result = _userManager.Users.Where(x => x.Subcribed == true).Select(x => x.NormalizedEmail).ToList();

            return result;
        }

        public string AverageStatistics(string city, string period)
        {
            string[] cities = city.Split(", ");

            var stringbuilder = new StringBuilder();
            for (int i = 0; i < cities.Length; i++)
            {
                int minusdate = int.Parse(period);
                var CityByDateFiltered = _weatherRepository.GetAll().Where(x => x.CityName == cities[i] && x.CreatedOn >= (DateTime.Now.AddHours(-minusdate)));

                var CityAverages = CityByDateFiltered.GroupBy(g => g.CityName, s => s.TempC).Select(g => new
                {
                    City = g.Key,
                    AvgTemperature = Math.Round(g.Average(), 2)
                });
                if (!CityAverages.Any())
                {
                    stringbuilder.AppendLine(cities[i] + " no statistics");
                }
                foreach (var item in CityAverages)
                {
                    stringbuilder.AppendLine($"Average C for : { cities[i]} before {period} hours from now is {item.AvgTemperature}");
                }
            }

            return stringbuilder.ToString();
        }

        public void SendMessage()
        {
            string Cities = _config.GetSection("Cities").Value;
            string Period = _config.GetSection("MailSendingInterval").Value;

            var usersMaillist = GetAllUsersEmails();
            var Average = AverageStatistics(Cities, Period).ToString();

            RabitPublishClass rabitPublishClass = new() {Message = Average, MailList = usersMaillist };
            _publishEndpoint.Publish<RabitPublishClass>(rabitPublishClass);
        }

        public void SendMessageDirectly()
        {
            string Cities = _config.GetSection("Cities").Value;
            string Period = _config.GetSection("MailSendingInterval").Value;

            var usersMaillist = GetAllUsersEmails();
            var Average = AverageStatistics(Cities, Period).ToString();

            EmailDto email = new EmailDto();
            email.Subject = "Average Statistics Report";

            email.Body = Average;

            foreach (var users in usersMaillist)
            {
                email.To = users;
                _emailservice.SendEmail(email);
            } 
        }
    }
}
