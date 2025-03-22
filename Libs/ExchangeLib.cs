// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.ExchangeLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Libs
{
    public class ExchangeLib
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
            return !currency_symbol.Equals(base_symbol) ? ViewExchangeRateService.GetViewExchangeRate(currency_symbol, base_symbol).inward_rate * num : 1M;
        }

        public static Decimal Convert(Decimal amount, string currency_symbol, string base_symbol)
        {
            Decimal rate = ExchangeLib.GetRate(currency_symbol, base_symbol);
            return amount * rate;
        }
    }
}