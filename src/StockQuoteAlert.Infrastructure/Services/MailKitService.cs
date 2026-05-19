using System;
using System.Collections.Generic;
using System.Text;
using StockQuoteAlert.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace StockQuoteAlert.Infrastructure.Services
{
    public class MailKitService : IMailKitService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _toEmail;

        public MailKitService(string smtpServer, int smtpPort, string smtpUser, string smtpPassword, string fromEmail, string toEmail)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPassword = smtpPassword;
            _fromEmail = fromEmail;
            _toEmail = toEmail;
        }

        public async Task SendEmailAsync(string subject, string body)
        {
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentException("O assunto do e-mail não pode estar vazio.");

            if (string.IsNullOrEmpty(body))
                throw new ArgumentException("O corpo do e-mail não pode estar vazio.");


            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_fromEmail));
            message.To.Add(MailboxAddress.Parse(_toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

        }
    }
}
