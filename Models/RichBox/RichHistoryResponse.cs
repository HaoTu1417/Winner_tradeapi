// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.RichBox.RichHistoryResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace tradeapi.Models.RichBox
{
    public class RichHistoryResponse
    {
        public int src { get; set; }

        public DateTime date { get; set; }

        public int type { get; set; }

        public Decimal amount { get; set; }

        public Decimal blance { get; set; }
    }
}