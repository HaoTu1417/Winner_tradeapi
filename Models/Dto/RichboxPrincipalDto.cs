// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.RichboxPrincipalDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class RichboxPrincipalDto
    {
        public int member_fk { get; set; }

        public uint pk { get; set; }

        public Decimal amount { get; set; }

        public DateTime date { get; set; }

        public string remarks { get; set; } = "";
    }
}