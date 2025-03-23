// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.StockFavoriteDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models
{
    public class StockFavoriteDto
    {
        public int pk { get; set; }

        public int member_fk { get; set; }

        public string market { get; set; } = "";

        public string stock_code { get; set; } = "";
    }
}