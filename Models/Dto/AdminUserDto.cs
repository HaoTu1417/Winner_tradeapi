// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.AdminUserDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class AdminUserDto
    {
        public int pk { get; set; }

        public string account { get; set; }

        public string role { get; set; }

        public string passwd { get; set; }

        public bool status { get; set; }

        public string nickname { get; set; }

        public int is_admin { get; set; }

        public int is_delete { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public int avatar { get; set; }

        public int sort { get; set; }

        public string lang { get; set; }

        public DateTime last_login_time { get; set; }

        public string last_login_ip { get; set; }

        public int change_password { get; set; }

        public int mfa_secret { get; set; }

        public string invitation_code { get; set; }
    }
}