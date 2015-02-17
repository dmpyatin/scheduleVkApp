using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleData.Infrastructure;
using System.Net.Mail;
using ScheduleData.Exceptions;

namespace ScheduleData.Services
{
    public class NotificationService
    {

        public void SendEmail(string toEmail, string subject, string body)
        {

            var util = new RegexUtilities();

            if (util.IsValidEmail(toEmail))
            {
                if (util.IsValidDomain(toEmail))
                {
                    MailSender.SendMail(toEmail, subject, body);
                }
                else
                {
                    throw new IncorrectMailDomainException();
                }
            }
            else
            {
                throw new IncorrectMailAddressException();
            }
        }
    }
}
