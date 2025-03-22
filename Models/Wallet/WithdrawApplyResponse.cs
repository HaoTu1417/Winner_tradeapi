// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.WithdrawApplyResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class WithdrawApplyResponse
    {
        public string order_no { get; set; }

        public DateTime request_time { get; set; }

        public string card { get; set; }

        public Decimal amount { get; set; }
    }
}