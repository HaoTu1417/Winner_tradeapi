// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.AnswerRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Info
{
    public class AnswerRequest
    {
        public string lang { get; set; } = "EN";

        public int catalog_pk { get; set; }

        public int question_pk { get; set; }
    }
}