// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.RichBox.BookResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace tradeapi.Models.RichBox
{
    public class BookResponse
    {
        public decimal? interest_rate { get; set; }

        public decimal? begin_profit { get; set; }

        public Decimal accrued_interest { get; set; }

        public Decimal wallet_balance { get; set; }

        public Decimal principal_balance { get; set; }

        public Decimal interest_balance { get; set; }
    }
}