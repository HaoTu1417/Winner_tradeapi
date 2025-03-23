// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.ThirdPay.RechargeNotifyRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.ThirdPay
{
    public class RechargeNotifyRequest
    {
        public string? err_code { get; set; }

        public string? err_msg { get; set; }

        public string? mer_no { get; set; }

        public string mer_order_no { get; set; }

        public string order_amount { get; set; }

        public string? ccy_no { get; set; }

        public string? order_no { get; set; }

        public string? create_time { get; set; }

        public string? pay_time { get; set; }

        public string status { get; set; }
    }
}