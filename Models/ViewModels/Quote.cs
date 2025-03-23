// Decompiled with JetBrains decompiler
// Type: Models.ViewModels.Quote
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.ViewModels
{
    public class Quote
    {
        public string stock_code { get; set; } = "";

        public string exchange { get; set; } = "";

        public DateTime? update_time { get; set; }

        public Decimal prev_day_c { get; set; }

        public Decimal prev_day_v { get; set; }

        public Decimal day_o { get; set; }

        public Decimal day_h { get; set; }

        public Decimal day_l { get; set; }

        public Decimal day_c { get; set; }

        public Decimal day_v { get; set; }

        public Decimal[] bid { get; set; } = Array.Empty<Decimal>();

        public Decimal[] bid_size { get; set; } = Array.Empty<Decimal>();

        public Decimal[] ask { get; set; } = Array.Empty<Decimal>();

        public Decimal[] ask_size { get; set; } = Array.Empty<Decimal>();

        public Decimal price { get; set; }

        public Decimal ceiling { get; set; }

        public Decimal floor { get; set; }
    }
}