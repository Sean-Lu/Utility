using System;
using System.Net.Mail;

namespace Sean.Utility.Net.Mail
{
    /// <summary>  
    /// 使用 SMTP 协议发送电子邮件
    /// </summary>  
    public class SmtpClientProvider : IDisposable
    {
        /// <summary>
        /// <see cref="SmtpClient"/>
        /// </summary>
        public SmtpClient Client => _smtpClient;

        private readonly SmtpClient _smtpClient;

        public SmtpClientProvider(string host, int port = 25)
        {
            _smtpClient = new SmtpClient(host, port);
        }
        public SmtpClientProvider(Action<SmtpClient> action)
        {
            _smtpClient = new SmtpClient();
            action(_smtpClient);
        }

        public void SendMail(MailMessage message)
        {
            _smtpClient.Send(message);
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}
