// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.OrderDetailResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class OrderDetailResponse
    {
        public string trade_order_sn { get; set; }

        public DateTime order_time { get; set; }

        public string stock_code { get; set; }

        public string stock_name { get; set; }

        public int price_type { get; set; }

        public int order_type { get; set; }

        public Decimal price { get; set; }

        public int volume { get; set; }

        public Decimal avg_price { get; set; }

        public int succeed_volume { get; set; }

        public int free_volume { get; set; }

        public int dir { get; set; }

        public int status { get; set; }
    }
}