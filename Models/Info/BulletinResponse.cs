// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.BulletinResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.Info
{
    public class BulletinResponse
    {
        public int pk { get; set; }

        public int doc_fk { get; set; }

        public string title { get; set; } = "";

        public string summary { get; set; } = "";

        public int view { get; set; }

        public int trash { get; set; }

        public DateTime? date { get; set; }
    }
}