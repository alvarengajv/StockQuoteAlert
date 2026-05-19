using System;
using System.Collections.Generic;
using System.Text;
using StockQuoteAlert.Application.DTOs;

namespace StockQuoteAlert.Application.Interfaces
{
    public interface IStockMonitorService
    {
        Task RunAsync(StockAlertDto alert, CancellationToken cancellationToken);
    }
}
