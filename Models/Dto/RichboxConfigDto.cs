// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.RichboxConfigDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class RichboxConfigDto
    {
        public uint id { get; set; }

        public bool enable { get; set; }

        public DateTime active_date { get; set; }

        public DateTime diactive_date { get; set; }

        public string currency { get; set; } = "";

        public Decimal min_investment { get; set; }

        public Decimal max_investment { get; set; }

        public Decimal interest_rate { get; set; }

        public Decimal begin_profit { get; set; }

        public TimeSpan closing_time { get; set; }

        public TimeSpan give_interest_time { get; set; }

        public string feature { get; set; } = "";

        public string description { get; set; } = "";

        public string trade_info { get; set; } = "";
    }
}