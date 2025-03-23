// Decompiled with JetBrains decompiler
// Type: Models.Dto.TradeOrderDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class TradeOrderDto
    {
        public string sub_account { get; set; }

        public int pk { get; set; }

        public string sn { get; set; }

        public string stock_code { get; set; }

        public string stock_name { get; set; }

        public string market { get; set; }

        public int dir { get; set; }

        public int order_type { get; set; }

        public int price_type { get; set; }

        public Decimal price { get; set; }

        public int status { get; set; }

        public int volume { get; set; }

        public int free_volume { get; set; }

        public int succeed_volume { get; set; }

        public int cancel_volume { get; set; }

        public DateTime order_time { get; set; }

        public string order_ip { get; set; }

        public string order_client { get; set; }

        public int order_source { get; set; }

        public DateTime? cancel_datetime { get; set; }

        public int cancel_type { get; set; }

        public string cancel_by { get; set; }

        public int live_mode { get; set; }

        public string live_ordersn { get; set; }

        public int live_request { get; set; }

        public int live_succeed { get; set; }

        public Decimal live_price { get; set; }
    }
}