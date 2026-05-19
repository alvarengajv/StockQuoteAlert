using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using StockQuoteAlert.Domain.Interfaces;

namespace StockQuoteAlert.Infrastructure.Services
{
    public class YahooQuoteService : IYahooQuoteService
    {
        private static readonly HttpClient _http = new HttpClient();

        public async Task<decimal> GetStockPriceAsync(string symbol)
        {
            var url = $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={symbol}.SA";
            var json = await _http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var currentPrice = doc.RootElement
                .GetProperty("quoteResponse")
                .GetProperty("result")[0]
                .GetProperty("regularMarketPrice")
                .GetDecimal();

            return currentPrice;
        }
    }
}
