// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.CollectInfoResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class CollectInfoResponse
    {
        public int type { get; set; }

        public string card { get; set; }

        public string payee { get; set; }

        public int min_recharge { get; set; }

        public string currency { get; set; }

        public Decimal exchange_rate { get; set; }

        public string bank_name { get; set; }

        public string fastbtn { get; set; } = "";

        public List<string> fastbtns { get; set; }
    }
}