// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Message.GetListPublicResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Message
{
    public class GetListPublicResponse
    {
        public List<ListPublicResponse> list { get; set; }

        public int total_unread_lists { get; set; }

        public int total_unread_public_lists { get; set; }
    }
}