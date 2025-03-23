// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.StockHolidayDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class StockHolidayDto
    {
        public int pk { get; set; }

        public string market { get; set; }

        public string name { get; set; }

        public int year { get; set; }

        public DateTime date { get; set; }

        public bool is_allday { get; set; }

        public DateTime open { get; set; }

        public DateTime close { get; set; }
    }
}