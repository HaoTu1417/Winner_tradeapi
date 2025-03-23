// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Order.PreSellRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Order
{
    public class PreSellRequest : LangRequest
    {
        public int price_type { get; set; }

        public string stock_code { get; set; } = "";

        public Decimal price { get; set; }

        public int volume { get; set; }
    }
}