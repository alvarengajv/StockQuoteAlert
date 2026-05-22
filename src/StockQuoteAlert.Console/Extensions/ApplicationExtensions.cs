using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StockQuoteAlert.Application.Interfaces;
using StockQuoteAlert.Application.Services;

namespace StockQuoteAlert.Console.Extensions

{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStockMonitorService, StockMonitorService>();
            return services;
        }
    }
}
