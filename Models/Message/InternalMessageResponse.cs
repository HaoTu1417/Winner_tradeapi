// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Message.InternalMessageResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Message
{
    public class InternalMessageResponse
    {
        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public DateTime Date { get; set; }

        public int IsRead { get; set; }

        public List<string> ImgUrls { get; set; }
    }
}