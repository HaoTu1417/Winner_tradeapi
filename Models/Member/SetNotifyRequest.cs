// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Member.SetNotifyRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Models.Member
{
    public class SetNotifyRequest
    {
        public bool EmailNotify { get; set; } = true;

        public bool SiteMessageNotify { get; set; } = true;

        public bool AccountAlertNotify { get; set; } = true;

        public bool AccountMarginCallNotify { get; set; } = true;

        public bool StockTransactionNotify { get; set; } = true;

        public int AccountExpiryNotify { get; set; } = 2;

        public bool PromotionsNotify { get; set; } = true;

        public bool DepositApprovedNotify { get; set; } = true;

        public bool WithdrawalApprovedNotify { get; set; } = true;

        public bool TradingAccountApprovedNotify { get; set; } = true;

        public string lang { get; set; } = "EN";
    }
}