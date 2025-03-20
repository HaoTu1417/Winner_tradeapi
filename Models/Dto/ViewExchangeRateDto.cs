// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.ViewExchangeRateDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class ViewExchangeRateDto
    {
        public string date { get; set; }

        public string currency_symbol { get; set; }

        public string base_symbol { get; set; }

        public Decimal inward_rate { get; set; }

        public Decimal outward_rate { get; set; }

        public DateTime create_time { get; set; }
    }
}