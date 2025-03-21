// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.SysMarketDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Dto
{
    public class SysMarketDto
    {
        public string code { get; set; }

        public string exchange { get; set; }

        public string label { get; set; }

        public List<string> exchanges { get; set; }

        public string currency { get; set; }

        public bool enable { get; set; }

        public string name { get; set; }

        public Decimal buy_fee { get; set; }

        public Decimal sell_fee { get; set; }

        public Decimal min_buy_fee { get; set; }

        public Decimal min_sell_fee { get; set; }

        public string default_stock_code { get; set; }
    }
}