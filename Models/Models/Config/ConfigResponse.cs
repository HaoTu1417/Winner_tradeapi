// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Config.ConfigResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models.Config
{
    public class ConfigResponse
    {
        public string apiserver { get; set; }

        public string fileserver { get; set; }

        public string filesite { get; set; }

        public string defaul_lan { get; set; }

        public string date_format { get; set; }

        public string wallet_currency { get; set; }

        public string app_download_url { get; set; }

        public string app_test_url { get; set; }

        public string ip_country { get; set; }

        public bool show_lang_menu { get; set; }

        public string web_site_url { get; set; }

        public bool management_fee_enable_coupon_use { get; set; }

        public List<string> markets { get; set; }
    }
}