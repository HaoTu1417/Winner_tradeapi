// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.BorrowPlanDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class BorrowPlanDto
    {
        public int pk { get; set; }

        public bool enable { get; set; }

        public string borrow_type { get; set; }

        public string market { get; set; }

        public string name { get; set; }

        public string lang { get; set; }

        public string rate { get; set; }

        public Decimal warning_line { get; set; }

        public Decimal break_line { get; set; }

        public string max_proporting { get; set; }

        public bool renewal { get; set; }

        public string use_time { get; set; }

        public Decimal money_range_min { get; set; }

        public Decimal money_range_max { get; set; }

        public Decimal money_range_increase { get; set; }

        public string fastbtn { get; set; }

        public string slogan { get; set; }

        public string note { get; set; }

        public string unique_set { get; set; }
    }
}