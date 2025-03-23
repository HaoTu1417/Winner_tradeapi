// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.HomeVm
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Models
{
  public class HomeVm
  {
    public long TotalMember { get; set; }

    public long TodayRegister { get; set; }

    public long TodayVerify { get; set; }

    public long MemberIsTrading { get; set; }

    public long MemberHasPosition { get; set; }

    public Decimal RechargeNeedVerify { get; set; }

    public Decimal WithdrawNeedVerify { get; set; }

    public Decimal RechargeToday { get; set; }

    public Decimal WithdrawToday { get; set; }

    public long FirstRechargeMemberToday { get; set; }

    public Decimal FirstRechargeAmountToday { get; set; }

    public long NewSubAccountToday { get; set; }

    public Decimal DealEarnToday { get; set; }

    public Decimal HandlingFeeToday { get; set; }

    public Decimal ManagementFeeToday { get; set; }

    public Decimal RichBox { get; set; }

    public Decimal TotalRecharge { get; set; }

    public Decimal TotalWithdraw { get; set; }

    public string RechargeNeedVerifyString { get; set; }

    public string WithdrawNeedVerifyString { get; set; }

    public string RechargeTodayString { get; set; }

    public string WithdrawTodayString { get; set; }

    public string FirstRechargeAmountTodayString { get; set; }

    public string DealEarnTodayString { get; set; }

    public string HandlingFeeTodayString { get; set; }

    public string ManagementFeeTodayString { get; set; }

    public string RichBoxString { get; set; }

    public string TotalRechargeString { get; set; }

    public string TotalWithdrawString { get; set; }

    public string RechargeApplyCount { get; set; }

    public string WithdrawApplyCount { get; set; }

    public string MemberVerifying { get; set; }

    public string MessageRecordUnread { get; set; }
  }
}
