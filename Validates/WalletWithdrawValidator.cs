// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.WalletWithdrawValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Wallet;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Validates
{
  public class WalletWithdrawValidator : AbstractValidator<WithdrawApplyRequest>
  {
    public WalletWithdrawValidator(int member_fk)
    {
      this.RuleFor<long?>((Expression<Func<WithdrawApplyRequest, long?>>) (x => x.time_stamp)).Custom<WithdrawApplyRequest, long?>((Action<long?, ValidationContext<WithdrawApplyRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
      this.RuleFor<string>((Expression<Func<WithdrawApplyRequest, string>>) (x => x.card_pk)).Custom<WithdrawApplyRequest, string>((Action<string, ValidationContext<WithdrawApplyRequest>>) ((card_pk, context) =>
      {
        if (string.IsNullOrEmpty(card_pk))
          throw new AppException(2352, "fill_card_pk");
        if (MemberBankService.Find(card_pk) == null)
          throw new AppException(2353, "invalid_card_pk");
      }));
      this.RuleFor<WithdrawApplyRequest>((Expression<Func<WithdrawApplyRequest, WithdrawApplyRequest>>) (x => x)).Custom<WithdrawApplyRequest, WithdrawApplyRequest>((Action<WithdrawApplyRequest, ValidationContext<WithdrawApplyRequest>>) ((req, context) =>
      {
        if (req.amount == 0M)
          throw new AppException(2341, "fill_amount");
        WalletDto walletDto = WalletService.Find(member_fk);
        if (walletDto.balance - walletDto.freeze < req.amount)
          throw new AppException(2346, "balance_not_enough");
        Decimal result1;
        Decimal.TryParse(ConfigLib.Get("min_per_withdraw"), out result1);
        if (req.amount < result1)
          throw new AppException(2350, "min_withdraw_amount_unmet");
        string s = ConfigLib.Get("max_daily_withdraw");
        Decimal result2;
        Decimal.TryParse(s, out result2);
        if ((Decimal) WalletWithdrawService.GetWithdrawAmountToday(member_fk) + req.amount > result2)
          throw new AppException(2347, "daily_amount_exceeded");
        ConfigLib.Get("max_monthly_withdraw");
        Decimal result3;
        Decimal.TryParse(s, out result3);
        if ((Decimal) WalletWithdrawService.GetWithdrawAmountThisMonth(member_fk) + req.amount > result3)
          throw new AppException(2348, "monthly_amount_exceeded");
      }));
      this.RuleFor<WithdrawApplyRequest>((Expression<Func<WithdrawApplyRequest, WithdrawApplyRequest>>) (x => x)).Custom<WithdrawApplyRequest, WithdrawApplyRequest>((Action<WithdrawApplyRequest, ValidationContext<WithdrawApplyRequest>>) ((req, context) =>
      {
        if (string.IsNullOrEmpty(req.pay_pwd))
          throw new AppException(2354, "fill_pay_pwd");
        string paywd = MemberServices.Find(member_fk).paywd;
        if (!BCrypt.Net.BCrypt.Verify(req.pay_pwd, paywd))
          throw new AppException(2355, "wrong_pay_pwd");
        if (MemberServices.Find(member_fk).id_auth != 1)
          throw new AppException(1303, "no_id_auth_yet");
      }));
      this.RuleFor<WithdrawApplyRequest>((Expression<Func<WithdrawApplyRequest, WithdrawApplyRequest>>) (x => x)).Custom<WithdrawApplyRequest, WithdrawApplyRequest>((Action<WithdrawApplyRequest, ValidationContext<WithdrawApplyRequest>>) ((req, context) =>
      {
        if (MemberServices.Find(member_fk).is_test_account)
          throw new AppException(2394, "test_account_cant_withdraw");
      }));
    }
  }
}
