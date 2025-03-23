// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.RegisterRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class RegisterRequest : LangRequest
    {
        private string _email = "";

        public string account { get; set; }

        public string country_pk { get; set; } = "";

        public string email
        {
            get => this._email;
            set => this._email = value.ToLower();
        }

        public string mobile_country { get; set; }

        public string mobile { get; set; }

        public string passwd { get; set; }

        public string verity_mail { get; set; }

        public string invitation_code { get; set; } = "";
    }
}