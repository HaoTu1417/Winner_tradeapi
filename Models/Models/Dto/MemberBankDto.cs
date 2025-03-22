// Decompiled with JetBrains decompiler
// Type: Models.Dto.MemberBankDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class MemberBankDto
    {
        public int member_fk { get; set; }

        public string card_pk { get; set; }

        public int card_type { get; set; }

        public string currency { get; set; }

        public string country { get; set; }

        public string bank { get; set; }

        public string branch { get; set; }

        public string card { get; set; }

        public string account { get; set; }

        public int cms_files_fk { get; set; }

        public bool is_confirm { get; set; }

        public bool is_delete { get; set; }

        public string create_ip { get; set; }

        public DateTime create_time { get; set; }
    }
}