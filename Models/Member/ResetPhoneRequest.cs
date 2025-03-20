// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.ResetPhoneRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class ResetPhoneRequest : LangRequest
    {
        public string country_code { get; set; } = "";

        public string phone_number { get; set; } = "";

        public string password { get; set; } = "";

        public string verification_code { get; set; } = "";
    }
}