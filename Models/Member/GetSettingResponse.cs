// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.GetSettingResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class GetSettingResponse
    {
        public string Avatar { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsRealNameVerified { get; set; }

        public int? DateFormat { get; set; }

        public int AuthStatus { get; set; }

        public bool AlreadySetPaypwd { get; set; }

        public int stock_chart_setting { get; set; }

        public int enable_auto_transfer { get; set; }

        public int need_withdraw_selfie { get; set; }

        public string lang { get; set; }
    }
}