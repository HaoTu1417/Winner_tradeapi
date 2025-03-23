// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.PreMarginCallResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class PreMarginCallResponse
    {
        public string sub_account { get; set; } = "";

        public Decimal init_money { get; set; }

        public Decimal balance { get; set; }

        public Decimal warningline { get; set; }

        public Decimal breakline { get; set; }

        public Decimal max_add_money { get; set; }

        public Decimal wallet_balance { get; set; }

        public string currency { get; set; } = "";

        public Decimal exchange_rate { get; set; }
    }
}