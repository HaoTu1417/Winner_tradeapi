// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.ExchangeHelper
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using tradeapi.Models.Dto;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Utility
{
  public class ExchangeHelper
  {
    public static Decimal GetRate(string currency_symbol, string base_symbol)
    {
      if (currency_symbol == "USDT")
        currency_symbol = "USD";
      if (base_symbol == "USDT")
        base_symbol = "USD";
      Decimal num = 1M;
      if (currency_symbol == "KVND")
      {
        currency_symbol = "VND";
        num *= 1000M;
      }
      if (base_symbol == "KVND")
      {
        base_symbol = "VND";
        num /= 1000M;
      }
      Decimal rate;
      if (string.IsNullOrEmpty(currency_symbol) || string.IsNullOrEmpty(base_symbol) || currency_symbol.Equals(base_symbol))
      {
        rate = 1M;
      }
      else
      {
        ViewExchangeRateDto viewExchangeRate = ViewExchangeRateService.GetViewExchangeRate(currency_symbol, base_symbol);
        rate = viewExchangeRate == null ? 1M : viewExchangeRate.inward_rate * num;
      }
      return rate;
    }

    public static Decimal Convert(Decimal amount, string currency_symbol, string base_symbol)
    {
      if (currency_symbol == base_symbol)
        return amount;
      if (currency_symbol == "USDT")
        currency_symbol = "USD";
      if (base_symbol == "USDT")
        base_symbol = "USD";
      Decimal rate = ExchangeHelper.GetRate(currency_symbol, base_symbol);
      return ExchangeHelper.Round(amount * rate, base_symbol);
    }

    public static Decimal Round(Decimal value, string currency)
    {
      int num;
      switch (currency)
      {
        case "USD":
          num = 2;
          break;
        case "VND":
          num = 0;
          break;
        case "TWD":
          num = 0;
          break;
        default:
          num = 2;
          break;
      }
      int decimals = num;
      return Math.Round(value, decimals);
    }
  }
}
