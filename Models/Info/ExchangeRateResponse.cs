// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.ExchangeRateResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Info
{
    public class ExchangeRateResponse
    {
        public string exchange { get; set; } = "USD-VND";

        public Decimal exchange_rate { get; set; }
    }
}