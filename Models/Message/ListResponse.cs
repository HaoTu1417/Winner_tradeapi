// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Message.ListResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Message
{
    public class ListResponse
    {
        public int pk { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public DateTime Date { get; set; }

        public bool IsRead { get; set; }
    }
}