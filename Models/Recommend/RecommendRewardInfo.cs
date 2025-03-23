// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Recommend.RecommendRewardInfo
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Recommend
{
    public class RecommendRewardInfo
    {
        public int year { get; set; }

        public int month { get; set; }

        public Decimal total_reward { get; set; }

        public string members { get; set; }

        public string borrow_fees { get; set; }

        public string rewards { get; set; }

        public int state { get; set; }

        public DateTime withdraw { get; set; }

        public DateTime paydate { get; set; }
    }
}