using BL.Models;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ConsumerRabbitMQ : IConsumer<RabitPublishClass>
    {
        public async Task Consume(ConsumeContext<RabitPublishClass> context)
        {
            var msg = context.Message;

            await Console.Out.WriteAsync(msg.Message);
        }
    }
}
