// Decompiled with JetBrains decompiler
// Type: Models.Dto.MemberLoginDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class MemberLoginDto
    {
        public int member_fk { get; set; }

        public string ip { get; set; }

        public string ip_country { get; set; }

        public string login_account { get; set; }

        public string device { get; set; }

        public DateTime create_time { get; set; }

        public int status { get; set; }

        public string remark { get; set; }
    }
}