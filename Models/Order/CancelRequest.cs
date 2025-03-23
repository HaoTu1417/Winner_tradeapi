// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Order.CancelRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Order
{
    public class CancelRequest : LangRequest
    {
        public string trade_order_sn { get; set; } = "";
    }
}