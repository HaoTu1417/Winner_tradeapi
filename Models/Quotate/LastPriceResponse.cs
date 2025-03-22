// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Quotate.LastPriceResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Quotate
{
    public class LastPriceResponse
    {
        public string code { get; set; } = "";

        public string name { get; set; } = "";

        public Decimal prev_close { get; set; }

        public Decimal open { get; set; }

        public Decimal high { get; set; }

        public Decimal low { get; set; }

        public Decimal price { get; set; }

        public Decimal price_change { get; set; }

        public Decimal percentage_change { get; set; }

        public Decimal volume { get; set; }

        public Decimal[] bid { get; set; } = Array.Empty<Decimal>();

        public int[] bid_size { get; set; } = Array.Empty<int>();

        public Decimal[] ask { get; set; } = Array.Empty<Decimal>();

        public int[] ask_size { get; set; } = Array.Empty<int>();

        public List<Dictionary<string, string>> bids { get; set; } = new List<Dictionary<string, string>>();

        public List<Dictionary<string, string>> asks { get; set; } = new List<Dictionary<string, string>>();

        public Decimal ceiling { get; set; }

        public Decimal floor { get; set; }
    }
}