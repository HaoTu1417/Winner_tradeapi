// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Account.SimpleAccountResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Account
{
    public class SimpleAccountResponse
    {
        public string sub_account { get; set; }

        public int status { get; set; }

        public string market { get; set; }
    }
}