// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Info.ExchangeResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Runtime.CompilerServices;

#nullable enable
namespace tradeapi.Models.Info
{
    // [RequiredMember]
    public class ExchangeResponse
    {
        // [RequiredMember]
        public string base_currency { get; set; }

        // [RequiredMember]
        public string quote_currency { get; set; }

        // [RequiredMember]
        public string base_currency_flag { get; set; }

        // [RequiredMember]
        public string quote_currency_flag { get; set; }

        public Decimal exchange_rate { get; set; }

        // [Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
        // [CompilerFeatureRequired("RequiredMembers")]
        public ExchangeResponse()
        {
        }
    }
}