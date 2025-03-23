// Decompiled with JetBrains decompiler
// Type: Models.Dto.RichBoxRecordDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace Models.Dto
{
    public class RichBoxRecordDto
    {
        public int pk { get; set; }

        public int member_fk { get; set; }

        public Decimal affect { get; set; }

        public Decimal balance { get; set; }

        public int src { get; set; }

        public DateTime create_time { get; set; }

        public Decimal interest_rate { get; set; }
    }
}