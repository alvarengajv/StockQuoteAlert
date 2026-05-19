using System;
using System.Collections.Generic;
using System.Text;
using StockQuoteAlert.Application.DTOs;

namespace StockQuoteAlert.Application.Validations
{
    public static class StockAlertDtoValidator
    {
        public static void Validate(StockAlertDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Symbol))
                throw new ArgumentException("Símbolo não pode ser vazio.", nameof(dto.Symbol));

            if (dto.SellPrice <= 0)
                throw new ArgumentException("Preço de venda deve ser maior que zero.", nameof(dto.SellPrice));

            if (dto.BuyPrice <= 0)
                throw new ArgumentException("Preço de compra deve ser maior que zero.", nameof(dto.BuyPrice));

            if (dto.BuyPrice >= dto.SellPrice)
                throw new ArgumentException("Preço de compra deve ser menor que preço de venda.");
        }
    }
}
