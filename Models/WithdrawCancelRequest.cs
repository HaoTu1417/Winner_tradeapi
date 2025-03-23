// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.WithdrawCancelRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class WithdrawCancelRequest
    {
        public string order_no { get; set; }

        public string lang { get; set; } = "EN";
    }
}