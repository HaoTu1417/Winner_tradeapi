// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.WithdrawValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models.SubAccount;

#nullable enable
namespace tradeapi.Validates
{
    public class WithdrawValidator : AbstractValidator<WithdrawRequest>
    {
        public WithdrawValidator()
        {
            this.RuleFor<string>((Expression<Func<WithdrawRequest, string>>) (x => x.sub_account)).Custom<WithdrawRequest, string>((Action<string, ValidationContext<WithdrawRequest>>) ((sub_account, context) =>
            {
                if (string.IsNullOrEmpty(sub_account))
                    throw new AppException(2385, "fill_subaccount");
            }));
            this.RuleFor<Decimal>((Expression<Func<WithdrawRequest, Decimal>>) (x => x.withdraw_amount)).Custom<WithdrawRequest, Decimal>((Action<Decimal, ValidationContext<WithdrawRequest>>) ((amount, context) =>
            {
                if (amount == 0M)
                    throw new AppException(2386, "fill_withdraw_amount");
            }));
        }
    }
}