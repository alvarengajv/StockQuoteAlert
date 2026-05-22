using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using StockQuoteAlert.Domain.Interfaces;

namespace StockQuoteAlert.Infrastructure.Services
{
    public class BrapiQuoteService : IBrapiQuoteService
    {
        private static readonly HttpClient _http = new HttpClient();

        static BrapiQuoteService()
        {
            _http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<decimal> GetStockPriceAsync(string symbol)
        {
            var url = $"https://brapi.dev/api/quote/{symbol}";
            var json = await _http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var currentPrice = doc.RootElement
                .GetProperty("results")[0]
                .GetProperty("regularMarketPrice")
                .GetDecimal();

            return currentPrice;
        }
    }
}
