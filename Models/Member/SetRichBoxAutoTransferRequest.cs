// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.SetRichBoxAutoTransferRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class SetRichBoxAutoTransferRequest
    {
        public int enable_auto_transfer { get; set; }

        public string lang { get; set; } = "VN";
    }
}