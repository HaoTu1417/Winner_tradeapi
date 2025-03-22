// Decompiled with JetBrains decompiler
// Type: Models.Dto.TradePositionDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class TradePositionDto
    {
        public string sub_account { get; set; }

        public string stock_code { get; set; }

        public string stock_name { get; set; } = "";

        public int stock_type { get; set; }

        public string market { get; set; } = "";

        public int holding_volume { get; set; }

        public int stop_lose_pos { get; set; }

        public int new_pos { get; set; }

        public int close_pos { get; set; }

        public Decimal lastprice { get; set; }

        public Decimal total { get; set; }

        public Decimal cost_purchase { get; set; }

        public int cost_volume { get; set; }

        public Decimal cost_price { get; set; }

        public int live_volume { get; set; }

        public Decimal live_cost { get; set; }
    }
}