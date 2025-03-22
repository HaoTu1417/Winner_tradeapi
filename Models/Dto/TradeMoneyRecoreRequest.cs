// Decompiled with JetBrains decompiler
// Type: Models.Dto.TradeMoneyRecoreRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class TradeMoneyRecoreRequest
    {
        public int member_fk { get; set; }

        public string sub_account { get; set; }

        public int temp_id { get; set; }

        public string sn { get; set; }

        public string currency { get; set; }

        public Decimal affect { get; set; }

        public Decimal balance { get; set; }

        public Decimal exchange { get; set; }

        public int op { get; set; }

        public Decimal wallet_amount { get; set; }

        public string reviewer { get; set; }

        public object[]? list { get; set; }

        public DateTime create_datetime { get; set; }
    }
}