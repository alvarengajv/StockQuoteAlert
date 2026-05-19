using System;
using System.Collections.Generic;
using System.Text;

namespace StockQuoteAlert.Domain.Interfaces
{
    public interface IMailKitService
    {
        Task SendEmailAsync(string subject, string body);
    }
}
