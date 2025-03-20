// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.VerifyIdentityRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class VerifyIdentityRequest
    {
        public string name { get; set; }

        public string mobile_country { get; set; }

        public string mobile { get; set; }

        public int id_type { get; set; }

        public string id_number { get; set; }

        public string img_front { get; set; }

        public string lang { get; set; } = "EN";
    }
}