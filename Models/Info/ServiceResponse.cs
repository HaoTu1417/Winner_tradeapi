// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.ServiceResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Info
{
    public class ServiceResponse
    {
        public string lang { get; set; }

        public string svc_phone { get; set; }

        public string svc_workday { get; set; }

        public string svc_nonworkday { get; set; }

        public string svc_link { get; set; }

        public string svc_email { get; set; }
    }
}