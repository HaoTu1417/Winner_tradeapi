// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.CouponrecordResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class CouponrecordResponse
    {
        public string Category { get; set; } = "";

        public string Title { get; set; } = "";

        public DateTime Date { get; set; }

        public Decimal TransactionAmount { get; set; } = 0.0M;

        public Decimal CouponBalance { get; set; } = 0.0M;
    }
}