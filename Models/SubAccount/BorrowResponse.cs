// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.BorrowResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class BorrowResponse
    {
        public string order_id { get; set; } = "";

        public int status { get; set; }

        public string borrow_type { get; set; } = "";

        public Decimal capital { get; set; }

        public Decimal balance { get; set; }

        public Decimal profit_and_loss { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }
    }
}