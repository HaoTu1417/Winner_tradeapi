// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.HoldingResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using tradeapi.Models.Enums;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class HoldingResponse
    {
        public string stock_code { get; set; }

        public string stock_name { get; set; }

        public OrderSide dir { get; set; }

        public int holding { get; set; }

        public int frozen { get; set; }

        public Decimal lastprice { get; set; }

        public Decimal cost_price { get; set; }

        public Decimal profit { get; set; }

        public Decimal profit_percentage { get; set; }

        public string market { get; set; } = "";
    }
}