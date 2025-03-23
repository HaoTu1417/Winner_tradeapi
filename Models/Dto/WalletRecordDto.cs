// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.WalletRecordDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class WalletRecordDto
    {
        public int member_fk { get; set; }

        public int pk { get; set; }

        public int type { get; set; }

        public string currency { get; set; }

        public Decimal affect { get; set; }

        public Decimal freeze { get; set; }

        public Decimal balance { get; set; }

        public Decimal coupon { get; set; }

        public string param { get; set; }

        public int templat_id { get; set; }

        public string info { get; set; }

        public DateTime create_time { get; set; }

        public string create_ip { get; set; }
    }
}