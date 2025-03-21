// Decompiled with JetBrains decompiler
// Type: Models.Dto.RecommendRewardDetailDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class RecommendRewardDetailDto
    {
        public int recommend_reward_fk { get; set; }

        public int member_fk { get; set; }

        public int parent { get; set; }

        public int borrow_fee_fk { get; set; }

        public int pk { get; set; }

        public string yymm { get; set; }

        public DateTime borrow_date { get; set; }

        public string currency { get; set; }

        public double management_fee { get; set; }

        public int generation { get; set; }

        public double rate { get; set; }

        public double reward { get; set; }
    }
}