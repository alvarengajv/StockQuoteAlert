using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockQuoteAlert.Domain.Interfaces;
using StockQuoteAlert.Infrastructure.Services;

namespace StockQuoteAlert.Console.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("Smtp");
            var smtpServer = smtpSettings.GetValue<string>("Server");
            var smtpPort = smtpSettings.GetValue<int>("Port");
            var smtpUser = smtpSettings.GetValue<string>("User");
            var smtpPassword = smtpSettings.GetValue<string>("Password");
            var fromEmail = smtpSettings.GetValue<string>("From");
            var toEmail = smtpSettings.GetValue<string>("To");

            services.AddScoped<IMailKitService>(provider =>
                new MailKitService(smtpServer, smtpPort, smtpUser, smtpPassword, fromEmail, toEmail));


            services.AddScoped<IBrapiQuoteService, BrapiQuoteService>();

            return services;
        }
    }
}
