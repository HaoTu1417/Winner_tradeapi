// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.BorrowApply
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class BorrowApply
    {
        public int pk { get; set; }

        public int borrow_plan_fk { get; set; }

        public int member_fk { get; set; }

        public string member_username { get; set; }

        public string member_real_name { get; set; }

        public string order_id { get; set; }

        public sbyte status { get; set; }

        public string borrow_type { get; set; }

        public string currency { get; set; }

        public string market { get; set; }

        public int borrow_duration { get; set; }

        public sbyte? auto_renewal { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        public Decimal deposit_money { get; set; }

        public Decimal borrow_money { get; set; }

        public sbyte multiple { get; set; }

        public Decimal rate { get; set; }

        public Decimal borrow_interest { get; set; }

        public string time_zone { get; set; }

        public Decimal? warning_line { get; set; }

        public Decimal? break_line { get; set; }

        public Decimal init_money { get; set; }

        public Decimal total_coupon { get; set; }

        public Decimal total_fee { get; set; }

        public DateTime create_time { get; set; }

        public DateTime? verify_time { get; set; }
    }
}