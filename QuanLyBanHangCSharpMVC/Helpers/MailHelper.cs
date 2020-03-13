using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QuanLyBanHangCSharpMVC.Helpers
{
    public class MailHelper
    {
        public async static Task SendEmail(string toEmailAddress, string subject, string content)
        {
            await Task.Run(() =>
            {
                string fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"];
                string emailPassWord = ConfigurationManager.AppSettings["FromEmailPassword"];
                string fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"];
                string smtpHost = ConfigurationManager.AppSettings["SMTPHost"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                bool enabledSSL = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"]);

                string body = content;
                MailMessage mailMessage = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;

                SmtpClient client = new SmtpClient(smtpHost, smtpPort);
                client.EnableSsl = enabledSSL;
                client.Credentials = new NetworkCredential(fromEmailAddress, emailPassWord);
                client.Send(mailMessage);
            });
        }
    }
}