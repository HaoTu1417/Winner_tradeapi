// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.CmsDocumentDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Dto
{
    public class CmsDocumentDto
    {
        public int pk { get; set; }

        public string cid { get; set; }

        public string lang { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public string flag { get; set; }

        public int view { get; set; }

        public int comment { get; set; }

        public int good { get; set; }

        public int bad { get; set; }

        public int mark { get; set; }

        public int sort { get; set; }

        public bool status { get; set; }

        public bool trash { get; set; }
    }
}