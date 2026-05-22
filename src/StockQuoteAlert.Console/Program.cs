using System.Globalization;
using StockQuoteAlert.Application.DTOs;
using StockQuoteAlert.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockQuoteAlert.Console.Extensions;


if (args.Length != 3)
{
    Console.WriteLine("Uso: stock-quote-alert.exe <ATIVO> <PRECO_VENDA> <PRECO_COMPRA>");
    return;
}

var alert = new StockAlertDto
{
    Symbol = args[0],
    SellPrice = decimal.Parse(args[1], CultureInfo.InvariantCulture),
    BuyPrice = decimal.Parse(args[2], CultureInfo.InvariantCulture)
};

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection()
    .AddApplicationServices()
    .AddInfrastructureServices(configuration)
    .BuildServiceProvider();

var intervalSeconds = configuration.GetValue<int>("Monitoring:IntervalSeconds", 60);
var interval = TimeSpan.FromSeconds(intervalSeconds);

var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    Console.WriteLine();
    Console.WriteLine("Encerrando monitoramento...");
    cancellationTokenSource.Cancel();
};

var stockMonitorService = services.GetRequiredService<IStockMonitorService>();

try
{
    await stockMonitorService.RunAsync(alert, interval, cancellationTokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Monitoramento encerrado.");
}

