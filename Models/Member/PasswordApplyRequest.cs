// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.PasswordApplyRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class PasswordApplyRequest : LangRequest
    {
        private string _email = "";

        public string email
        {
            get => this._email;
            set => this._email = value.ToLower();
        }

        public string email_verifyCode { get; set; } = "";

        public string newpasswd { get; set; } = "";

        public string captcha { get; set; } = "";
    }
}