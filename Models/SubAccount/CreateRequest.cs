// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.CreateRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace tradeapi.Models.SubAccount
{
    public class CreateRequest : LangRequest
    {
        public int borrow_plan_pk { get; set; }

        public Decimal deposit_money { get; set; }

        public int trading_time { get; set; }

        public int borrow_duration { get; set; }

        public int multiple { get; set; }

        public bool auto_renewal { get; set; }
    }
}