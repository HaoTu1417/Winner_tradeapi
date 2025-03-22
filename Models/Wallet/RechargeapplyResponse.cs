// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.RechargeapplyResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class RechargeapplyResponse
    {
        public string order_no { get; set; } = "";

        public bool success { get; set; }

        public string redirect_url { get; set; } = "";
    }
}