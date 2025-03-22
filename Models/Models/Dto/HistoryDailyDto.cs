// Decompiled with JetBrains decompiler
// Type: Models.Dto.HistoryDailyDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class HistoryDailyDto
    {
        public uint pk { get; set; }

        public DateTime date { get; set; }

        public string stock_code { get; set; } = "";

        public Decimal open { get; set; }

        public Decimal high { get; set; }

        public Decimal low { get; set; }

        public Decimal close { get; set; }

        public int volume { get; set; }
    }
}