// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Recommend.RecommendProfitModel
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Recommend
{
    public class RecommendProfitModel
    {
        public int org { get; set; }

        public DateTime dt { get; set; }

        public string yymm { get; set; }

        public double money { get; set; }

        public int borrow_fee_fk { get; set; }

        public string currency { get; set; }
    }
}