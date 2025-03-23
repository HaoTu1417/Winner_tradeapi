// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.WalletRecordRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Runtime.CompilerServices;

#nullable enable
namespace tradeapi.Models.Wallet
{
  
    public class WalletRecordRequest : LangRequest
    {
        public int member_pk { get; set; }

        public int type { get; set; }

        public int subtype { get; set; }

        public string currency { get; set; }

        public int temp_id { get; set; }

        public Decimal affect { get; set; }

        public Decimal coupon { get; set; }

        public Decimal balance { get; set; }

        public DateTime createtime { get; set; }

        
        public  object[] list { get; set; }

        public string create_ip { get; set; }

       
        public WalletRecordRequest()
        {
        }
    }
}