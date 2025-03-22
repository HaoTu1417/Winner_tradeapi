// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.MoneyReviewDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using tradeapi.Models.Enums;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class MoneyReviewDto
    {
        public int id { get; set; }

        public string sn { get; set; }

        public int mem_id { get; set; }

        public string sub_account { get; set; }

        public int agent_id { get; set; }

        public string agent_name { get; set; }

        public FinanceType type { get; set; }

        public int state { get; set; }

        public Decimal frozen { get; set; }

        public Decimal amount { get; set; }

        public string info { get; set; }

        public string request_json { get; set; }

        public DateTime request_time { get; set; }

        public string acccept_by { get; set; }

        public DateTime accept_time { get; set; }

        public string message { get; set; }
    }
}