// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.WalletPaymentDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class WalletPaymentDto
    {
        public int pk { get; set; }

        public string pay_code { get; set; }

        public string pay_name { get; set; }

        public string pay_type { get; set; }

        public string pay_url { get; set; }

        public string pay_Notice_url { get; set; }

        public string pay_Return_url { get; set; }

        public string pay_account { get; set; }

        public Decimal min_recharge { get; set; }

        public int status { get; set; }
    }
}