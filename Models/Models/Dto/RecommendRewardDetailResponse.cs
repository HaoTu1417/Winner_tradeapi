// Decompiled with JetBrains decompiler
// Type: Models.Dto.RecommendRewardDetailResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class RecommendRewardDetailResponse
    {
        public string real_name { get; set; }

        public int generation { get; set; }

        public DateTime borrow_date { get; set; }

        public Decimal management_fee { get; set; }

        public Decimal rate { get; set; }

        public Decimal reward { get; set; }

        public string currency { get; set; }
    }
}