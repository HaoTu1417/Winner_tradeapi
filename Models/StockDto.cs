// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.StockDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models
{
    public class StockDto
    {
        public string stock_code { get; set; } = "";

        public string stock_name { get; set; } = "";

        public string market { get; set; } = "";

        public string exchange { get; set; } = "";

        public bool enable { get; set; }

        public bool disable_alwayse { get; set; }

        public bool program_enable { get; set; }

        public string program_msg { get; set; } = "";

        public bool main_switch { get; set; }

        public int close_reason { get; set; }

        public DateTime opentrade { get; set; }

        public DateTime update_datetime { get; set; }

        public Decimal yclose { get; set; }

        public Decimal limitbuy { get; set; }

        public Decimal limitsell { get; set; }

        public Decimal final_price { get; set; }

        public int volume { get; set; }

        public string full_info { get; set; } = "";
    }
}