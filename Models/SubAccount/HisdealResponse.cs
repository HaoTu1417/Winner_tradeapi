// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.HisdealResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class HisdealResponse
    {
        public string sub_account { get; set; } = "";

        public string deal_id { get; set; } = "";

        public DateTime create_datetime { get; set; }

        public string stock_code { get; set; } = "";

        public string stock_name { get; set; } = "";

        public Decimal total_amount { get; set; }

        public int dir { get; set; }
    }
}