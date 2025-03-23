// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.PromotionContentResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Info
{
    public class PromotionContentResponse
    {
        public int pk { get; set; }

        public string title { get; set; } = "";

        public string topic_content { get; set; } = "";

        public string activity_name { get; set; } = "";

        public bool show_activity_time { get; set; }

        public DateTime starttime { get; set; }

        public DateTime endtime { get; set; }
    }
}