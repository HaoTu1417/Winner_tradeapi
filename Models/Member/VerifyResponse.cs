// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.VerifyResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Member
{
    public class VerifyResponse
    {
        private string _email = "";

        public int id { get; set; }

        public string code { get; set; }

        public DateTime send_time { get; set; }

        public byte type { get; set; }

        public string email
        {
            get => this._email;
            set => this._email = value.ToLower();
        }
    }
}