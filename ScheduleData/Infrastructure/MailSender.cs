using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ScheduleData.Infrastructure
{
    public static class MailSender
    {


        public static void SendMail(string toEmail, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress("schedule.noreply@gmail.com", "Расписание ПетрГУ");
                var toAddress = new MailAddress(toEmail, "noreply");
                const string fromPassword = "xgmst321";
  
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}