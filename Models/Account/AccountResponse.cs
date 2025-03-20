// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Account.AccountResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Account
{
    public class AccountResponse
    {
        public string sub_account { get; set; }

        public string loan_type { get; set; }

        public int status { get; set; }

        public Decimal mem_money { get; set; }

        public Decimal margin { get; set; }

        public Decimal loan_money { get; set; }

        public Decimal balance { get; set; }

        public Decimal warningline { get; set; }

        public Decimal breakline { get; set; }

        public Decimal frozen_money { get; set; }

        public Decimal total { get; set; }

        public Decimal income { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        public DateTime? close_time { get; set; }
    }
}