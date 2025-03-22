// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Quotate.MarketResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Quotate
{
    public class MarketResponse
    {
        public string market { get; set; } = "US";

        public List<string> exchanges { get; set; }

        public string label { get; set; } = "";
    }
}