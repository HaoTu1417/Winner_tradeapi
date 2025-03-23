// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.Money_recordDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class Money_recordDto
    {
        public int id { get; set; }

        public string sn { get; set; }

        public string sub_account { get; set; }

        public int mem_id { get; set; }

        public int type { get; set; }

        public int op { get; set; }

        public int deal_id { get; set; }

        public Decimal affect { get; set; }

        public string info { get; set; }

        public string reviewer { get; set; }

        public DateTime create_datetime { get; set; }

        public string stock_code { get; set; }

        public string stock_name { get; set; }
    }
}