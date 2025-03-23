// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.TradeMoneyCheckDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class TradeMoneyCheckDto
    {
        public string sub_account { get; set; }

        public int pk { get; set; }

        public string sn { get; set; }

        public int type { get; set; }

        public int state { get; set; }

        public Decimal frozen { get; set; }

        public Decimal exchange { get; set; }

        public string currency { get; set; }

        public Decimal amount { get; set; }

        public DateTime request_time { get; set; }

        public string acccept_by { get; set; }

        public DateTime accept_time { get; set; }
    }
}