// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Sys.SysSignInRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Sys
{
    public class SysSignInRequest : LangRequest
    {
        public string appkey { get; set; }

        public int CurTime { get; set; }

        public string nonce { get; set; }

        public new string lang
        {
            get => this._lang;
            set => this._lang = value.ToUpper();
        }

        private string _lang { get; set; } = "EN";
    }
}