// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.TradeAccountResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class TradeAccountResponse
    {
        public string sub_account { get; set; } = "";

        public bool is_trading { get; set; }

        public bool is_over { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        public string loan_type { get; set; } = "";

        public string loan_name { get; set; } = "";

        public Decimal balance { get; set; }

        public Decimal mem_money { get; set; }

        public Decimal margin { get; set; }

        public Decimal margin_float { get; set; }

        public string market { get; set; } = "";

        public string market_name { get; set; } = "";

        public Decimal init_money { get; set; }

        public Decimal position_value { get; set; }

        public Decimal warningline { get; set; }

        public Decimal warning_value { get; set; }

        public Decimal breakline { get; set; }

        public Decimal break_value { get; set; }

        public string currency { get; set; }

        public Decimal withdrawable_amount { get; set; }

        public Decimal management_fee { get; set; }

        public long multiple { get; set; }

        public Decimal available_balance { get; set; }
    }
}