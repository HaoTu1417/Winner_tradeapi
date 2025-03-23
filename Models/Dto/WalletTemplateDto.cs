// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.WalletTemplateDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Dto
{
    public class WalletTemplateDto
    {
        public int pk { get; set; }

        public int temp_id { get; set; }

        public string lang { get; set; }

        public string name { get; set; }

        public string template { get; set; }

        public string param { get; set; }

        public string demo { get; set; }
    }
}