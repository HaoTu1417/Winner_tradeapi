// Decompiled with JetBrains decompiler
// Type: Models.Dto.RecommendInfoResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace Models.Dto
{
    public class RecommendInfoResponse
    {
        public Decimal total_reward { get; set; }

        public int total_invitations { get; set; }
    }
}