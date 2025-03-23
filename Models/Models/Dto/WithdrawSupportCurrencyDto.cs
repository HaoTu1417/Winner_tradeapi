// Decompiled with JetBrains decompiler
// Type: Models.Dto.WithdrawSupportCurrencyDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace Models.Dto
{
    public class WithdrawSupportCurrencyDto
    {
        public string code { get; set; }

        public string currency { get; set; }

        public int type { get; set; }

        public bool enable { get; set; }
    }
}