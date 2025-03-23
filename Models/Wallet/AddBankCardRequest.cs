// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Wallet.AddBankCardRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Wallet
{
    public class AddBankCardRequest : LangRequest
    {
        public int card_type { get; set; }

        public string card { get; set; }

        public string currency { get; set; }

        public string name { get; set; } = "";

        public string country { get; set; } = "";

        public string bank_name { get; set; } = "";

        public string bank_branch { get; set; } = "";
    }
}