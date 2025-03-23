// Decompiled with JetBrains decompiler
// Type: Models.Dto.VwTradeAccountDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class VwTradeAccountDto
    {
        public string sub_account { get; set; }

        public Decimal balance { get; set; }

        public Decimal warning_value { get; set; }

        public Decimal break_value { get; set; }

        public int member_fk { get; set; }

        public string account { get; set; }

        public string member_name { get; set; }

        public int type { get; set; }

        public string market { get; set; }

        public string currency { get; set; }

        public string loan_type { get; set; }

        public Decimal mem_money { get; set; }

        public Decimal frozen_money { get; set; }

        public Decimal margin { get; set; }

        public Decimal margin_float { get; set; }

        public Decimal loan_money { get; set; }

        public string time_zone { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        public DateTime close_time { get; set; }

        public int status { get; set; }

        public Decimal warningline { get; set; }

        public Decimal breakline { get; set; }

        public DateTime notice_warning { get; set; }

        public DateTime notice_close { get; set; }

        public Decimal position_value { get; set; }
    }
}