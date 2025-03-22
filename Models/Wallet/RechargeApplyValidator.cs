// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.RechargeApplyValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models.Wallet;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Validates
{
  public class RechargeApplyValidator : AbstractValidator<RechargeapplyRequest>
  {
    public RechargeApplyValidator()
    {
      this.RuleFor<long?>((Expression<Func<RechargeapplyRequest, long?>>) (x => x.time_stamp)).Custom<RechargeapplyRequest, long?>((Action<long?, ValidationContext<RechargeapplyRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
      this.RuleFor<RechargeapplyRequest>((Expression<Func<RechargeapplyRequest, RechargeapplyRequest>>) (x => x)).Custom<RechargeapplyRequest, RechargeapplyRequest>((Action<RechargeapplyRequest, ValidationContext<RechargeapplyRequest>>) ((recharge, context) =>
      {
        if (string.IsNullOrEmpty(recharge.RechargeMethod))
          throw new AppException(2311, "fill_recharge_method");
        if (recharge.RechargeMethod != "bank" && recharge.RechargeMethod != "crypto")
          throw new AppException(2310, "invlid_recharge_method");
        if (string.IsNullOrEmpty(recharge.Currency))
          throw new AppException(2321, "fill_currency");
        if (recharge.ExchangeRate == 0M)
          throw new AppException(2331, "fill_exchange_rate");
        if (recharge.ExchangeRate > 100000M)
          throw new AppException(2332, "exchange_rate_too_large");
        if (recharge.Amount == 0M)
          throw new AppException(2341, "fill_amount");
        CollectInfoResponse collectInfoResponse = AdminBankService.Find(recharge.RechargeMethod == "bank" ? 1 : 2, recharge.Currency);
        if (collectInfoResponse == null)
          throw new AppException(2300, "bank_account_not_found");
        if (recharge.Amount < (Decimal) collectInfoResponse.min_recharge)
          throw new AppException(2349, "min_recharge_not_sufficient");
        if (string.IsNullOrEmpty(recharge.Payee))
          throw new AppException(2361, "fill_payee");
        if (recharge.RechargeMethod == "bank" && string.IsNullOrEmpty(recharge.WalletLast5Digits))
          throw new AppException(2371, "fill_wallet_last_5_digits");
        if (recharge.RechargeMethod == "bank" && AdminBankService.Find(recharge.Currency, recharge.AccountNumber, recharge.Payee) == null)
          throw new AppException(2300, "bank_account_not_found");
        if (string.IsNullOrEmpty(recharge.AccountNumber))
          throw new AppException(2351, "fill_account_number");
      }));
    }
  }
}
