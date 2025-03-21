// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.AccountDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class AccountDto
    {
        public string sub_account { get; set; }

        public string sub_pwd { get; set; }

        public int mem_id { get; set; }

        public string token { get; set; }

        public string mem_name { get; set; }

        public int type { get; set; }

        public string loan_type { get; set; }

        public Decimal mem_money { get; set; }

        public Decimal frozen_money { get; set; }

        public Decimal margin { get; set; }

        public Decimal loan_money { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        public DateTime? close_time { get; set; }

        public int status { get; set; }

        public Decimal warningline { get; set; }

        public Decimal breakline { get; set; }

        public DateTime? notice_warning { get; set; }

        public DateTime? notice_close { get; set; }

        public int agent_id { get; set; }

        public string agent_name { get; set; }

        public int live_state { get; set; }

        public Decimal live_balance { get; set; }

        public Decimal live_breakline { get; set; }

        public int live_Broker { get; set; }

        public int live_accountID { get; set; }
    }
}