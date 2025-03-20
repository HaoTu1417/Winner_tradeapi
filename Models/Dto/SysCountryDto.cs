// Decompiled with JetBrains decompiler
// Type: Models.Dto.SysCountryDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace Models.Dto
{
    public class SysCountryDto
    {
        public string pk { get; set; }

        public string label { get; set; }

        public bool enable { get; set; }

        public string lang { get; set; }

        public string currency { get; set; }

        public string flag { get; set; }

        public string code { get; set; }
    }
}