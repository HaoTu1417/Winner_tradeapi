// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.BulletinContentResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Info
{
    public class BulletinContentResponse
    {
        public int pk { get; set; }

        public string title { get; set; } = "";

        public string summary { get; set; } = "";

        public string topic_content { get; set; } = "";
    }
}