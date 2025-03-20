// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.GetRecordHistoryResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class GetRecordHistoryResponse
    {
        public DateTime create_time { get; set; }

        public string type { get; set; } = "";

        public Decimal affect { get; set; } = 0.0M;

        public Decimal balance { get; set; } = 0.0M;
    }
}