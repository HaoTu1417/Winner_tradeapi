// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.SignInResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class SignInResponse
    {
        public string Token { get; set; }

        public string SubAccount { get; set; }

        public int Status { get; set; }

        public string Market { get; set; }

        public bool is_test_account { get; set; }

        public string lang { get; set; }
    }
}