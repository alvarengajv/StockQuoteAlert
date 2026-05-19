using System;
using System.Collections.Generic;
using System.Text;

namespace StockQuoteAlert.Application.DTOs
{
    public class StockAlertDto
    {
        public required string Symbol { get; set; }
        public required decimal SellPrice { get; set; }
        public required decimal BuyPrice { get; set; }
    }
}
