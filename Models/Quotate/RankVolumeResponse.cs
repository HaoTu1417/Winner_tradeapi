// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Quotate.RankVolumeResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Quotate
{
    public class RankVolumeResponse
    {
        public string market { get; set; } = "US";

        public string code { get; set; } = "";

        public string name { get; set; } = "";

        public Decimal price { get; set; }

        public Decimal prev_day_c { get; set; }

        public Decimal price_change { get; set; }

        public Decimal percentage_change { get; set; }

        public Decimal volume { get; set; }

        public bool is_favorite { get; set; }
    }
}