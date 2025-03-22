// Decompiled with JetBrains decompiler
// Type: Models.Dto.RecommendRegisterResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class RecommendRegisterResponse
    {
        public string nickname { get; set; }

        public DateTime register_date { get; set; }

        public DateTime? first_borrow_date { get; set; }
    }
}