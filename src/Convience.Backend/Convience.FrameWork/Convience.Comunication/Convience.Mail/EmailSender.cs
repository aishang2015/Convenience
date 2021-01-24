using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Convience.Mail
{
    public interface IEmailSender
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="recepients">接收者</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isHtml">是否是html</param>
        Task<(bool success, string errorMsg)> SendEmailAsync(MailboxAddress sender,
            MailboxAddress[] recepients, string subject, string body, bool isHtml = true);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="recepientName">接收者名</param>
        /// <param name="recepientEmail">接收者邮箱</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isHtml">是否是html</param>
        /// <returns></returns>
        Task<(bool success, string errorMsg)> SendEmailAsync(string recepientName,
            string recepientEmail, string subject, string body, bool isHtml = true);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="senderName">发送者名</param>
        /// <param name="senderEmail">发送者邮箱</param>
        /// <param name="recepientName">接收者名</param>
        /// <param name="recepientEmail">接收邮箱</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isHtml">是否是html</param>
        Task<(bool success, string errorMsg)> SendEmailAsync(string senderName,
            string senderEmail, string recepientName, string recepientEmail, string subject,
            string body, bool isHtml = true);
    }

    public class EmailSender : IEmailSender
    {
        private readonly MailOption _mailOption;

        private readonly ILogger _logger;

        public EmailSender(IOptions<MailOption> mailOption, ILogger<EmailSender> logger)
        {
            _mailOption = mailOption.Value;
            _logger = logger;
        }

        public async Task<(bool success, string errorMsg)> SendEmailAsync(MailboxAddress sender, MailboxAddress[] recepients, string subject, string body, bool isHtml = true)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(sender);
            message.To.AddRange(recepients);
            message.Subject = subject;
            message.Body = isHtml ? new BodyBuilder { HtmlBody = body }.ToMessageBody() : new TextPart("plain") { Text = body };

            try
            {
                using (var client = new SmtpClient())
                {
                    if (!_mailOption.UseSSL)
                    {
                        // 同意所有的ssl证书
                        client.ServerCertificateValidationCallback = (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                    }

                    // 连接邮箱
                    await client.ConnectAsync(_mailOption.Host, _mailOption.Port, _mailOption.UseSSL).ConfigureAwait(false);

                    // 取消oauth2认证
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // 认证
                    if (!string.IsNullOrWhiteSpace(_mailOption.Username))
                    {
                        await client.AuthenticateAsync(_mailOption.Username, _mailOption.Password).ConfigureAwait(false);
                    }

                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "邮件发送失败!");
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string errorMsg)> SendEmailAsync(string recepientName, string recepientEmail, string subject, string body, bool isHtml = true)
        {
            var from = new MailboxAddress(_mailOption.Name, _mailOption.EmailAddress);
            var to = new MailboxAddress(recepientName, recepientEmail);

            return await SendEmailAsync(from, new MailboxAddress[] { to }, subject, body, isHtml);
        }

        public async Task<(bool success, string errorMsg)> SendEmailAsync(string senderName, string senderEmail, string recepientName, string recepientEmail, string subject, string body, bool isHtml = true)
        {
            var from = new MailboxAddress(senderName, senderEmail);
            var to = new MailboxAddress(recepientName, recepientEmail);

            return await SendEmailAsync(from, new MailboxAddress[] { to }, subject, body, isHtml);
        }
    }
}
