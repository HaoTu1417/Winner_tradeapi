// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.IndChart.SetIndicatorRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.IndChart
{
    public class SetIndicatorRequest
    {
        public string name { get; set; } = "";

        public double param1 { get; set; }

        public double param2 { get; set; }

        public double param3 { get; set; }

        public double param4 { get; set; }

        public double param5 { get; set; }

        public string lang { get; set; } = "EN";
    }
}