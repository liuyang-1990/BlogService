using Blog.Infrastructure.DI;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Implement
{
    public class MailHelper
    {
        private static readonly Regex MailRegex = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");

        private static readonly string UserName;
        private static readonly string Password;
        static MailHelper()
        {
            var config = CoreContainer.Current.GetService<IConfiguration>();
            UserName = config["Authentication:Mail:UserName"];
            Password = config["Authentication:Mail:Password"];
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">发件人邮件地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <returns></returns>
        public static async Task SendEMailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException(nameof(to));
            }
            if (!MailRegex.IsMatch(to))
            {
                throw new ArgumentException("收件人地址不合法", nameof(to));
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("blog", "xy_liuy0305@163.com"));
            message.To.Add(new MailboxAddress(to));
            message.Priority = MessagePriority.Normal;
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = body };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.163.com", 465, true);
                await client.AuthenticateAsync(UserName, Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }
    }
}