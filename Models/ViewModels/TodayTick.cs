// Decompiled with JetBrains decompiler
// Type: Models.ViewModels.TodayTick
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace Models.ViewModels
{
    public class TodayTick
    {
        public DateTime date { get; set; } = DateTime.Today;

        public List<string[]> tick { get; set; } = new List<string[]>();
    }
}