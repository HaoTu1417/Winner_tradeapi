// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.IndChart.StockHistoryList
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.IndChart
{
    public class StockHistoryList
    {
        public string stock_code { get; set; }

        public string date { get; set; }

        public Decimal open { get; set; }

        public Decimal high { get; set; }

        public Decimal low { get; set; }

        public Decimal close { get; set; }

        public ulong volume { get; set; }
    }
}