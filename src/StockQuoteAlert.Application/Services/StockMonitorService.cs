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
        private readonly IBrapiQuoteService _BrapiQuoteService;
        private readonly IMailKitService _mailKitService;

        public StockMonitorService(IBrapiQuoteService BrapiQuoteService, IMailKitService mailKitService)
        {
            _BrapiQuoteService = BrapiQuoteService;
            _mailKitService = mailKitService;
        }

        public async Task RunAsync(StockAlertDto alert, TimeSpan interval, CancellationToken cancellationToken)
        {
            StockAlertDtoValidator.Validate(alert);

            Console.WriteLine($"Iniciando monitoramento de {alert.Symbol}...");
            Console.WriteLine($"Venda acima de {alert.SellPrice:F2} | Compra abaixo de {alert.BuyPrice:F2}");
            Console.WriteLine("Pressione Ctrl+C para encerrar.");
            Console.WriteLine();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var currentPrice = await _BrapiQuoteService.GetStockPriceAsync(alert.Symbol);

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {alert.Symbol}: R$ {currentPrice:F2}");

                    if (currentPrice >= alert.SellPrice)
                    {
                        await _mailKitService.SendEmailAsync(
                            $"Alerta de Venda: {alert.Symbol}",
                            $"O ativo {alert.Symbol} atingiu o preço de venda de {alert.SellPrice:F2}. \nPreço atual: {currentPrice:F2}.");
                        Console.WriteLine("  → Email de VENDA enviado.");
                    }
                    else if (currentPrice <= alert.BuyPrice)
                    {
                        await _mailKitService.SendEmailAsync(
                            $"Alerta de Compra: {alert.Symbol}",
                            $"O ativo {alert.Symbol} atingiu o preço de compra de {alert.BuyPrice:F2}. Preço atual: {currentPrice:F2}.");
                        Console.WriteLine("  → Email de COMPRA enviado.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Erro: {ex.Message}. Tentando novamente em {interval.TotalSeconds}s.");
                }

                await Task.Delay(interval, cancellationToken);
            }
        }
    }
}

