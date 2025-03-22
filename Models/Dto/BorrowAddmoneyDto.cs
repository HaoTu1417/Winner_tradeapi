// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.BorrowAddmoneyDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class BorrowAddmoneyDto
    {
        public string sub_account { get; set; }

        public int member_fk { get; set; }

        public int pk { get; set; }

        public string currency { get; set; }

        public Decimal exchange { get; set; }

        public Decimal money { get; set; }

        public Decimal freeze { get; set; }

        public int status { get; set; }

        public DateTime add_time { get; set; }

        public DateTime verify_time { get; set; }

        public int target_uid { get; set; }

        public string target_name { get; set; }
    }
}