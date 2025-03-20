// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.MutilangSubjectDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Dto
{
    public class MutilangSubjectDto
    {
        public string lang { get; set; }

        public string title { get; set; }

        public int enable { get; set; }

        public string icon { get; set; }

        public int admin_default { get; set; }

        public int app_default { get; set; }
    }
}