// Decompiled with JetBrains decompiler
// Type: Models.Dto.GetUserStockOptionResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using tradeapi.Models;

#nullable enable
namespace Models.Dto
{
    public class GetUserStockOptionResponse : LangRequest
    {
        public int member_fk { get; set; }

        public string market { get; set; } = "";

        public string stock_code { get; set; }

        public string stock_name { get; set; } = "";

        public int quantity { get; set; }

        public int freeze { get; set; }
    }
}