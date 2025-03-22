// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Message.SendRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Message
{
    public class SendRequest
    {
        public string title { get; set; }

        public string content { get; set; }

        public string[]? img_urls { get; set; }

        public string lang { get; set; } = "EN";
    }
}