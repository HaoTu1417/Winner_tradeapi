// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.BorrowDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class BorrowDto
    {
        public int pk { get; set; }

        public string? sub_account { get; set; }

        public int borrow_plan_fk { get; set; }

        public int member_fk { get; set; }

        public string order_id { get; set; }

        public int status { get; set; }

        public string market { get; set; }

        public string borrow_type { get; set; }

        public string currency { get; set; }

        public Decimal deposit_money { get; set; }

        public Decimal init_money { get; set; }

        public int multiple { get; set; }

        public bool auto_renewal { get; set; }

        public Decimal borrow_money { get; set; }

        public Decimal borrow_interest { get; set; }

        public int repayment_type { get; set; }

        public int borrow_duration { get; set; }

        public int position { get; set; }

        public Decimal rate { get; set; }

        public int total { get; set; }

        public int trading_time { get; set; }

        public int loss_warn_sms_send { get; set; }

        public Decimal stock_money { get; set; }

        public Decimal total_coupon { get; set; }

        public Decimal total_fee { get; set; }

        public Decimal total_interest { get; set; }

        public DateTime create_time { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        public DateTime verify_time { get; set; }
    }
}