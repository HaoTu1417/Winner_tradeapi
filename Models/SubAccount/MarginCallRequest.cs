// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.MarginCallRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class MarginCallRequest : LangRequest
    {
        public string sub_account { get; set; } = "";

        public Decimal add_money { get; set; }
    }
}