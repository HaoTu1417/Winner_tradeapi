// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.TokenModel
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

#nullable enable
namespace tradeapi.Models
{
    public class TokenModel
    {
        public string? sub_account { get; set; }

        public int status { get; set; }

        public int member_fk { get; set; }

        public string ip { get; set; }

        public string device { get; set; }

        public string lang { get; set; }

        public string time_zone { get; set; }

        public string? market { get; set; }
    }
}