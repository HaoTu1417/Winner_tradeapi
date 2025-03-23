// Decompiled with JetBrains decompiler
// Type: sendmail.Models.SendMailModel
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Collections.Generic;

#nullable enable
namespace sendmail.Models
{
    public class SendMailModel
    {
        public string Email { get; set; }

        public Dictionary<string, string> sender { get; set; }

        public List<receive> to { get; set; }

        public string subject { get; set; }

        public string htmlContent { get; set; }
    }
}