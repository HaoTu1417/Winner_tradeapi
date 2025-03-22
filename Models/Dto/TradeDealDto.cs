// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.TradeDealDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class TradeDealDto
    {
        public int pk { get; set; }

        public string deal_id { get; set; }

        public string sub_account { get; set; }

        public string trade_order_sn { get; set; }

        public string stock_code { get; set; }

        public string stock_name { get; set; }

        public int order_type { get; set; }

        public string market { get; set; }

        public int dir { get; set; }

        public Decimal final_price { get; set; }

        public int final_volume { get; set; }

        public DateTime create_datetime { get; set; }

        public string currency { get; set; }

        public Decimal total_pay { get; set; }

        public Decimal total_amount { get; set; }

        public Decimal total_cost { get; set; }

        public Decimal handling_fee { get; set; }

        public Decimal transfer_fee { get; set; }

        public Decimal stamp_fee { get; set; }

        public Decimal other_fee { get; set; }
    }
}