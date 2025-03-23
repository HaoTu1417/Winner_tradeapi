// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.WalletDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class WalletDto
    {
        public int member_fk { get; set; }

        public string currency { get; set; }

        public Decimal balance { get; set; }

        public Decimal freeze { get; set; }

        public Decimal margin { get; set; }

        public Decimal operate_balance { get; set; }

        public Decimal richbox_balance { get; set; }

        public bool status { get; set; }

        public Decimal coupon { get; set; }

        public Decimal total_recharge { get; set; }

        public Decimal total_withdraw { get; set; }

        public DateTime last_update_time { get; set; }

        public Decimal richbox_interest { get; set; }

        public Decimal richbox_rate { get; set; }

        public Decimal anxin_balance { get; set; }
    }
}