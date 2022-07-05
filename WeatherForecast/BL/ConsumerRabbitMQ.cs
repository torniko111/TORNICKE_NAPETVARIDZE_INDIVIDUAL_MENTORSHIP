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
        private readonly IEmailService emailService;

        public ConsumerRabbitMQ(IEmailService emailService)
        {
            this.emailService = emailService;
        }


        public async Task Consume(ConsumeContext<RabitPublishClass> context)
        {
            var msg = context.Message.Message;
            var maillist = context.Message.MailList;

            //emailDto.Body = msg;
            //emailDto.To = "tornike.nafetvaridze@gmail.com";
            //emailDto.Subject = "Report Statistics";

            foreach (var item in maillist)
            {
                EmailDto emaildto = new EmailDto() { Body = msg, To = item, Subject = "Report for Average Statistics" };
                emailService.SendEmail(emaildto);
            }
        }
    }
}
