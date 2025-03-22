// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.SysSigninValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models.Sys;

#nullable enable
namespace tradeapi.Validates
{
    public class SysSigninValidator : AbstractValidator<SysSignInRequest>
    {
        public SysSigninValidator()
        {
            this.RuleFor<SysSignInRequest>((Expression<Func<SysSignInRequest, SysSignInRequest>>) (x => x)).Custom<SysSignInRequest, SysSignInRequest>((Action<SysSignInRequest, ValidationContext<SysSignInRequest>>) ((req, context) =>
            {
                if (!req.time_stamp.HasValue || req.time_stamp.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    throw new AppException(1170, "request_time_out");
            }));
        }
    }
}