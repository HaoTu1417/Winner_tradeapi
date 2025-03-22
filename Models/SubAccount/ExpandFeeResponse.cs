// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.ExpandFeeResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class ExpandFeeResponse
    {
        public Decimal borrow_fee { get; set; }

        public Decimal borrow_money { get; set; }

        public Decimal new_trading_quota { get; set; }

        public string currency { get; set; }
    }
}