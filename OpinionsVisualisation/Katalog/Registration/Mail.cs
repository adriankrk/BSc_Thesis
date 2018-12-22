using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;

namespace Katalog.Rejestracja
{
    public static class Mail
    {
         private static SmtpClient _smtpClient;

        static Mail()
        {
            _smtpClient = new SmtpClient();
        }

        /// <summary>
        /// Wysłanie e-maila do pojedynczego odbiorcy.
        /// </summary>
        /// <param name="recipientAddress">Adres e-mail odbiorcy.</param>
        /// <param name="recipient">Nazwa odbiorcy.</param>
        /// <param name="subject">Temat e-maila.</param>
        /// <param name="news">Treść e-maila.</param>
        public static void SendEmail(string recipientAddress, string recipient, string subject, string news)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("katalogprojekt@gmail.com", "Katalog - Projekt", Encoding.UTF8);
                mail.To.Add(new MailAddress(recipientAddress, recipient, Encoding.UTF8));
                mail.Subject = subject;
                mail.Body = news;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;
                mail.Priority = MailPriority.High;

                _smtpClient.Send(mail);
            }
        }

    }
}