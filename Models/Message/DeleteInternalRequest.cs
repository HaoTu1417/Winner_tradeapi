// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Message.DeleteInternalRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Message
{
    public class DeleteInternalRequest : LangRequest
    {
        public List<int> PkList { get; set; }
    }
}