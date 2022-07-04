using BL.Models;
using MassTransit;
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

        public RabbitMqPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task SendMessage(RabitPublishClass rabitPublishClass)
        {
            return _publishEndpoint.Publish<RabitPublishClass>(rabitPublishClass);
        }
    }
}
