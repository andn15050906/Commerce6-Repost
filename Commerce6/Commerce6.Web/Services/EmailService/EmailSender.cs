using Microsoft.Extensions.Options;
using System.Web;
using MailKit.Security;
using MimeKit;

namespace Commerce6.Web.Services.EmailService
{
    public class EmailSender
    {
        private readonly string _senderMail;
        private readonly string _senderPassword;
        private readonly string _senderName;
        private readonly string _appDomain;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _senderMail = options.Value.SenderMail;
            _senderPassword = options.Value.SenderPassword;
            _senderName = options.Value.SenderName;
            _appDomain = options.Value.AppDomain;
        }

        public string? GetAppDomain() => String.IsNullOrEmpty(_appDomain) ? null : _appDomain;

        public async Task SendEmailAsync(string toAddress, string subject = "-Subject", string body = "-Body-")
        {
            //Log.Information("Sending to " + toAddress);
            var mail = new MimeMessage
            {
                Sender = new MailboxAddress(_senderName, _senderMail),
                Subject = subject,
                Body = new BodyBuilder { HtmlBody = HttpUtility.HtmlDecode(body) }.ToMessageBody()
            };
            mail.From.Add(mail.Sender);
            mail.To.Add(MailboxAddress.Parse(toAddress));

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_senderMail, _senderPassword);
                await smtp.SendAsync(mail);
            }
            catch (Exception e)
            {
                //Log.Information(e.Message);
            }
            smtp.Disconnect(true);
        }
    }
}
