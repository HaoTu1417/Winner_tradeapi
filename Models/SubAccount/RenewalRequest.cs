// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.RenewalRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class RenewalRequest : LangRequest
    {
        public string sub_account { get; set; } = "";

        public int borrow_duration { get; set; }

        public Decimal use_coupon { get; set; }
    }
}