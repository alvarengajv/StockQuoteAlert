using System;
using System.Collections.Generic;
using System.Text;

namespace StockQuoteAlert.Domain.Interfaces
{
    public interface IYahooQuoteService
    {
        Task<decimal> GetStockPriceAsync(string symbol);
    }
}
