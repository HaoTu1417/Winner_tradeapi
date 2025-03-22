// Decompiled with JetBrains decompiler
// Type: Models.Dto.WalletWithdrawDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class WalletWithdrawDto
    {
        public int member_fk { get; set; }

        public string member_bank_fk { get; set; }

        public int pk { get; set; }

        public string order_no { get; set; }

        public Decimal wallet_amount { get; set; }

        public Decimal exchange { get; set; }

        public string currency { get; set; }

        public Decimal money { get; set; }

        public Decimal fee { get; set; }

        public int status { get; set; }

        public string note { get; set; }

        public DateTime create_time { get; set; }

        public string create_ip { get; set; }

        public int verify_admin_pk { get; set; }

        public DateTime verify_time { get; set; }

        public string reject_result { get; set; }

        public string id_selfie { get; set; }
    }
}