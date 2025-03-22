// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.AddBankCardValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models.Wallet;

#nullable enable
namespace tradeapi.Validates
{
    public class AddBankCardValidator : AbstractValidator<AddBankCardRequest>
    {
        public AddBankCardValidator()
        {
            this.RuleFor<long?>((Expression<Func<AddBankCardRequest, long?>>) (x => x.time_stamp)).Custom<AddBankCardRequest, long?>((Action<long?, ValidationContext<AddBankCardRequest>>) ((time, context) =>
            {
                if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    throw new AppException(1170, "request_time_out");
            }));
            this.RuleFor<AddBankCardRequest>((Expression<Func<AddBankCardRequest, AddBankCardRequest>>) (x => x)).Custom<AddBankCardRequest, AddBankCardRequest>((Action<AddBankCardRequest, ValidationContext<AddBankCardRequest>>) ((req, context) =>
            {
                if (req.card_type == 2)
                    return;
                if (string.IsNullOrEmpty(req.name))
                    throw new AppException(2342, "fill_card_name");
                if (string.IsNullOrEmpty(req.bank_name))
                    throw new AppException(2344, "fill_bank_name");
                if (string.IsNullOrEmpty(req.currency))
                    throw new AppException(2321, "fill_currency");
                if (string.IsNullOrEmpty(req.card))
                    throw new AppException(2322, "fill_card");
            }));
        }
    }
}