// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Recommend.RecommendRewardSummaryVw
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace tradeapi.Models.Recommend
{
    public class RecommendRewardSummaryVw
    {
        public int layer { get; set; }

        public int monthly_members { get; set; }

        public Decimal monthly_borrow_fee { get; set; }

        public Decimal monthly_reward { get; set; }
    }
}