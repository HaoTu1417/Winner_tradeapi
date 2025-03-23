// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Order.OrderHoldingResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Order
{
    public class OrderHoldingResponse
    {
        public string stock_code { get; set; } = "";

        public string exchange { get; set; } = "";

        public int holding { get; set; }

        public Decimal balance { get; set; }

        public Decimal stock_cost_percent { get; set; }

        public Decimal total_cost_percent { get; set; }

        public int base_value { get; set; }

        public string currency { get; set; } = "VND";

        public int full_position { get; set; }

        public bool avalible { get; set; }
    }
}