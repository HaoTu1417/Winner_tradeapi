// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.FeeRate
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace tradeapi.Utility
{
    public class FeeRate
    {
        public Decimal buy_fee_rate { get; set; }

        public Decimal min_buy_fee { get; set; }

        public Decimal sell_fee_rate { get; set; }

        public Decimal min_sell_fee { get; set; }
    }
}