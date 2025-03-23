// Decompiled with JetBrains decompiler
// Type: Models.TradeAccount.VM_TradeBalance
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.TradeAccount
{
    public class VM_TradeBalance
    {
        public string sub_account { get; set; }

        public Decimal balance { get; set; }

        public Decimal position_value { get; set; }
    }
}