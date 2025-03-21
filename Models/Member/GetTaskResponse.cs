// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.GetTaskResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Member
{
    public class GetTaskResponse
    {
        public string id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public Decimal reward { get; set; }

        public bool isCompleted { get; set; }

        public string currency { get; set; }
    }
}