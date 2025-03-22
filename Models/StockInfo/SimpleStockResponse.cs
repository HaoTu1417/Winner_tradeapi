// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.StockInfo.SimpleStockResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.StockInfo
{
    public class SimpleStockResponse
    {
        public string StockCode { get; set; }

        public string StockName { get; set; }

        public bool Enable { get; set; }

        public float Price { get; set; }

        public float Change { get; set; }
    }
}