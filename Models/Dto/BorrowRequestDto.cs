// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.BorrowRequestDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class BorrowRequestDto
    {
        public int borrow_plan_fk { get; set; }

        public string sub_account { get; set; }

        public int borrow_fk { get; set; }

        public int member_fk { get; set; }

        public int pk { get; set; }

        public int type { get; set; }

        public Decimal borrow_fee { get; set; }

        public Decimal use_coupon { get; set; }

        public Decimal fee_received { get; set; }

        public int borrow_duration { get; set; }

        public DateTime new_end_time { get; set; }

        public int status { get; set; }

        public DateTime add_time { get; set; }

        public DateTime verify_time { get; set; }
    }
}