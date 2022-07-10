using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IRabbitMqPublisher
    {
        void SendMessage();
        void SendMessageDirectly();
        List<string> GetAllUsersEmails();
        string AverageStatistics(string city, string period);
    }
}
