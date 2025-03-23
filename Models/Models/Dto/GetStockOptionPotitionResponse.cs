// Decompiled with JetBrains decompiler
// Type: Models.Dto.GetStockOptionPotitionResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class GetStockOptionPotitionResponse
    {
        public string market { get; set; } = "";

        public string stock_code { get; set; }

        public string stock_name { get; set; } = "";

        public string currency { get; set; } = "";

        public int quantity { get; set; }

        public int freeze { get; set; }

        public Decimal last_price { get; set; }

        public Decimal sell_price { get; set; }

        public DateTime sell_price_date { get; set; }

        public Decimal growth => (this.sell_price - this.last_price) / this.last_price;

        public Decimal total_cost { get; set; }
    }
}