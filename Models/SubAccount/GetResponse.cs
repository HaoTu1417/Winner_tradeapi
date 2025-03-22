// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.GetResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class GetResponse
    {
        public string sub_account { get; set; } = "";

        public Decimal total_assets { get; set; }

        public int status { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        public bool is_default { get; set; }

        public bool auto_renewal { get; set; }

        public string loan_type { get; set; } = "";

        public string market { get; set; } = "";

        public Decimal loan_money { get; set; }

        public Decimal position_market_value { get; set; }

        public Decimal warningline { get; set; }

        public Decimal warningline_distance { get; set; }

        public Decimal breakline { get; set; }

        public Decimal breakline_distance { get; set; }
    }
}