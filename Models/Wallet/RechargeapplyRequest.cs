// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.RechargeapplyRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class RechargeapplyRequest : LangRequest
    {
        public string RechargeMethod { get; set; }

        public string Currency { get; set; }

        public Decimal ExchangeRate { get; set; }

        public Decimal Amount { get; set; }

        public string AccountNumber { get; set; } = "";

        public string Payee { get; set; }

        public string WalletLast5Digits { get; set; } = "";
    }
}