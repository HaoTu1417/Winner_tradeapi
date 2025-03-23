// Decompiled with JetBrains decompiler
// Type: Models.Dto.RecommendRegisterDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable disable
namespace Models.Dto
{
    public class RecommendRegisterDto
    {
        public int member_fk { get; set; }

        public int invitee_fk { get; set; }

        public DateTime register_date { get; set; }

        public DateTime? first_borrow_date { get; set; }

        public bool is_admin { get; set; }
    }
}