// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.WalletResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class WalletResponse
    {
        public string account { get; set; }

        public int id_auth { get; set; }

        public int level_id { get; set; }

        public Decimal balance { get; set; }

        public Decimal coupon { get; set; }

        public Decimal freeze { get; set; }

        public Decimal available { get; set; }

        public string currency { get; set; }

        public int total_unread { get; set; }

        public Decimal richbox_balance { get; set; }

        public Decimal richbox_interest { get; set; }
    }
}