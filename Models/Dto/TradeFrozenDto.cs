// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.TradeFrozenDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class TradeFrozenDto
    {
        public string sub_account { get; set; }

        public int trade_order_fk { get; set; }

        public int pk { get; set; }

        public string info { get; set; }

        public int type { get; set; }

        public int frozen_volume { get; set; }

        public Decimal frozen_money { get; set; }

        public DateTime frozen_datetime { get; set; }
    }
}