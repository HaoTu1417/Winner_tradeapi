// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.ThirdPay.ThirdPayRechargeResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models.ThirdPay
{
    public class ThirdPayRechargeResponse
    {
        public string pay_url { get; set; }

        public DateTime validity_period { get; set; }
    }
}