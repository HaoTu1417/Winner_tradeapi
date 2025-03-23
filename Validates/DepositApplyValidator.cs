// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.DepositApplyValidator
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
    public class DepositApplyValidator : AbstractValidator<WalletRecordRequest>
    {
        public DepositApplyValidator()
        {
            this.RuleFor<long?>((Expression<Func<WalletRecordRequest, long?>>) (x => x.time_stamp)).Custom<WalletRecordRequest, long?>((Action<long?, ValidationContext<WalletRecordRequest>>) ((time, context) =>
            {
                if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    throw new AppException(1170, "request_time_out");
            }));
        }
    }
}