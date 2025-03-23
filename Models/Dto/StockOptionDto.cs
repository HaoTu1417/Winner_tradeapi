// Decompiled with JetBrains decompiler
// Type: Models.Dto.StockOptionDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace Models.Dto
{
    public class StockOptionDto
    {
        public int pk { get; set; }

        public string stock_name { get; set; }

        public string stock_code { get; set; }

        public int spot { get; set; }

        public int remain_spot { get; set; }

        public Decimal price { get; set; }

        public int quantity { get; set; }

        public string currency { get; set; }

        public string market { get; set; }
    }
}