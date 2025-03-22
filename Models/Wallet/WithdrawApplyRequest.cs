// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.WithdrawApplyRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class WithdrawApplyRequest : LangRequest
    {
        public string card_pk { get; set; }

        public string currency { get; set; }

        public Decimal amount { get; set; }

        public string pay_pwd { get; set; }

        public string id_selfie { get; set; }
    }
}