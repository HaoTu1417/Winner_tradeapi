// Decompiled with JetBrains decompiler
// Type: Models.Dto.RecommendRewardDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class RecommendRewardDto
    {
        public int pk { get; set; }

        public int member_fk { get; set; }

        public string yymm { get; set; }

        public int year { get; set; }

        public int month { get; set; }

        public string currency { get; set; }

        public Decimal total_reward { get; set; }

        public int state { get; set; }

        public DateTime withdraw { get; set; }

        public DateTime paydate { get; set; }

        public string note { get; set; }

        public DateTime create_time { get; set; }
    }
}