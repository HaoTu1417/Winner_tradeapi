// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.AdResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Models.Dto;
using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Info
{
    public class AdResponse
    {
        public List<CmsBannerDto> bnr { get; set; }

        public List<string> marq { get; set; }

        public string? pop_msg { get; set; }

        public bool? show_pop { get; set; }
    }
}