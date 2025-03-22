// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.ThirdPayRechargeValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models.Dto;
using tradeapi.Models.ThirdPay;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Validates
{
    public class ThirdPayRechargeValidator : AbstractValidator<ThirdPayRechargeRequest>
    {
        public ThirdPayRechargeValidator()
        {
            this.RuleFor<ThirdPayRechargeRequest>((Expression<Func<ThirdPayRechargeRequest, ThirdPayRechargeRequest>>) (x => x)).Custom<ThirdPayRechargeRequest, ThirdPayRechargeRequest>((Action<ThirdPayRechargeRequest, ValidationContext<ThirdPayRechargeRequest>>) ((req, context) =>
            {
                if (string.IsNullOrEmpty(req.pay_code))
                    throw new AppException(2390, "fill_pay_code");
                if (string.IsNullOrEmpty(req.currency))
                    throw new AppException(2321, "fill_currency");
                WalletPaymentDto walletPaymentDto = !(req.amount == 0M) ? WalletPaymentService.FindByPayCode(req.pay_code) : throw new AppException(2341, "fill_amount");
                if (walletPaymentDto == null)
                    throw new AppException(2391, "third_party_not_exist");
                if (req.amount < walletPaymentDto.min_recharge)
                    throw new AppException(2349, "min_recharge_not_sufficient");
            }));
        }
    }
}