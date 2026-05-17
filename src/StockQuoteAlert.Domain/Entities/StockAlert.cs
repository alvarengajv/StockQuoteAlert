using System;
using System.Collections.Generic;
using System.Text;

namespace StockQuoteAlert.Domain.Entities
{
    internal class StockAlert
    {
        public string Symbol { get; set; }
        public decimal SellPrice { get; set; }
        public decimal BuyPrice { get; set; }

    }
}
