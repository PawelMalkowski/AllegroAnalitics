using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace AllegroAnalitics.Common
{
    public class SendRegistrationEmail
    {
        public async static void SendEmail(string mail, string token, string username)
        {
            MailAddress to = new MailAddress(mail);
            MailAddress addressFrom = new MailAddress("allegroanalistic@gmail.com", "AllegroAnalitics Confirm Email");
            MailMessage message = new MailMessage(addressFrom, to)
            {
                Subject = "Conifr your acoount",
                Body = $"Please confirm your account by clicking this link: <a href= \"http://localhost/Registration?Token={token}&userName={username}\">link</a>",
                IsBodyHtml = true
            };

            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("allegroanalistic@gmail.com", "Q1w2e3r4t5y6,"),
                EnableSsl = true
            };
            
            try
            {
                await client.SendMailAsync(message);
            }

            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
