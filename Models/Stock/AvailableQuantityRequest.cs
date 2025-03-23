// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Stock.AvailableQuantityRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable disable
namespace tradeapi.Models.Stock
{
    public class AvailableQuantityRequest : LangRequest
    {
        public int stock_option_fk { get; set; }
    }
}