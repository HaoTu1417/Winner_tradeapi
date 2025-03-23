// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.Dto.Member_notifyDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable disable
namespace tradeapi.Models.Dto
{
    public class Member_notifyDto
    {
        public int member_fk { get; set; }

        public bool EmailNotify { get; set; }

        public bool SiteMessageNotify { get; set; }

        public bool AccountAlertNotify { get; set; }

        public bool AccountMarginCallNotify { get; set; }

        public bool StockTransactionNotify { get; set; }

        public int AccountExpiryNotify { get; set; }

        public bool PromotionsNotify { get; set; }

        public bool DepositApprovedNotify { get; set; }

        public bool WithdrawalApprovedNotify { get; set; }

        public bool TradingAccountApprovedNotify { get; set; }
    }
}