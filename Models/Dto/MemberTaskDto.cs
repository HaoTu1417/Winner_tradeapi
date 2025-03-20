// Decompiled with JetBrains decompiler
// Type: Models.Dto.MemberTaskDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class MemberTaskDto
    {
        public int pk { get; set; }

        public int sub_type { get; set; }

        public string currency { get; set; }

        public string lang { get; set; }

        public Decimal coin { get; set; }

        public string title { get; set; }

        public string content { get; set; }
    }
}