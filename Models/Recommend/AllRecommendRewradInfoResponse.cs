// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Recommend.AllRecommendRewradInfoResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Recommend
{
    public class AllRecommendRewradInfoResponse
    {
        public int year { get; set; }

        public int month { get; set; }

        public string yymm { get; set; }

        public string currency { get; set; }

        public Decimal total_reward { get; set; }

        public int state { get; set; }

        public List<RecommendRewardSummaryVw> rewards { get; set; }
    }
}