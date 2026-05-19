using System;
using System.Collections.Generic;
using System.Text;
using StockQuoteAlert.Domain.Interfaces;
using StockQuoteAlert.Application.DTOs;
using StockQuoteAlert.Application.Interfaces;
using StockQuoteAlert.Application.Validations;

namespace StockQuoteAlert.Application.Services
{
    public class StockMonitorService : IStockMonitorService
    {
        private readonly IYahooQuoteService _yahooQuoteService;
        private readonly IMailKitService _mailKitService;

        public StockMonitorService(IYahooQuoteService yahooQuoteService, IMailKitService mailKitService)
        {
            _yahooQuoteService = yahooQuoteService;
            _mailKitService = mailKitService;
        }

        public async Task RunAsync(StockAlertDto alert, CancellationToken cancellationToken)
        {
            StockAlertDtoValidator.Validate(alert);

            while (!cancellationToken.IsCancellationRequested)
            {
                var currentPrice = await _yahooQuoteService.GetStockPriceAsync(alert.Symbol);

                if (currentPrice >= alert.SellPrice)
                {
                    await _mailKitService.SendEmailAsync(
                        $"Alerta de Venda : {alert.Symbol}",
                        $"O ativo {alert.Symbol} atingiu o preço de venda de {alert.SellPrice:C}. Preço atual: {currentPrice:C}.");
                }
                else if (currentPrice <= alert.BuyPrice)
                {
                    await _mailKitService.SendEmailAsync(
                        $"Alerta de Compra: {alert.Symbol}",
                        $"O ativo {alert.Symbol} atingiu o preço de compra de {alert.BuyPrice:C}. Preço atual: {currentPrice:C}.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}
