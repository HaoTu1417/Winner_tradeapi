// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.SubAccount.HisAccountRecordResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.SubAccount
{
    public class HisAccountRecordResponse
    {
        public string sub_account { get; set; } = "";

        public string sn { get; set; }

        public DateTime create_datetime { get; set; }

        public Decimal affect { get; set; }

        public string info { get; set; }

        public string currency { get; set; }

        public string deal_id { get; set; }
    }
}