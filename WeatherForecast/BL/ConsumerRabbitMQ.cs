using BL.MailService;
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
        private readonly EmailDto emailDto;
        private readonly IEmailService emailService;

        public ConsumerRabbitMQ(EmailDto emailDto, IEmailService emailService)
        {
            this.emailDto = emailDto;
            this.emailService = emailService;
        }



        public async Task Consume(ConsumeContext<RabitPublishClass> context)
        {
            var msg = context.Message.Message;
            var maillist = context.Message.MailList;

            emailDto.Body = "meore cda";
            emailDto.To = "tornike.nafetvaridze@gmail.com";
            emailDto.Subject = "first try thought rabbitMQ";

            emailService.SendEmail(emailDto);
            await Console.Out.WriteAsync(msg);
            foreach (var item in maillist)
            {
                Console.WriteLine(item);
            }
        }
    }
}
