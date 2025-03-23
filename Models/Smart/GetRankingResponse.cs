// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Smart.GetRankingResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Smart
{
    public class GetRankingResponse
    {
        public int sn { get; set; }

        public string stock_code { get; set; } = "";

        public string stock_name { get; set; } = "";

        public Decimal final_price { get; set; }

        public int recommend { get; set; }
    }
}