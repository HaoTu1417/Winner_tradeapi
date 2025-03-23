// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.PasswordApplyValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using tradeapi.Common;
using tradeapi.Models.Member;

#nullable enable
namespace tradeapi.Validates
{
  public class PasswordApplyValidator : AbstractValidator<PasswordApplyRequest>
  {
    public PasswordApplyValidator()
    {
      this.RuleFor<long?>((Expression<Func<PasswordApplyRequest, long?>>) (x => x.time_stamp)).Custom<PasswordApplyRequest, long?>((Action<long?, ValidationContext<PasswordApplyRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
      this.RuleFor<string>((Expression<Func<PasswordApplyRequest, string>>) (x => x.email)).Custom<PasswordApplyRequest, string>((Action<string, ValidationContext<PasswordApplyRequest>>) ((email, context) =>
      {
        if (string.IsNullOrEmpty(email))
          throw new AppException(1202, "fill_email");
        if (!new EmailAddressAttribute().IsValid((object) email))
          throw new AppException(1203, "invalid_email_format");
      }));
      this.RuleFor<string>((Expression<Func<PasswordApplyRequest, string>>) (x => x.newpasswd)).Custom<PasswordApplyRequest, string>((Action<string, ValidationContext<PasswordApplyRequest>>) ((passwd, context) =>
      {
        if (string.IsNullOrEmpty(passwd))
          throw new AppException(1207, "enter_password");
        if (passwd.Length < 6 || passwd.Length > 96)
          throw new AppException(1208, "password_length_6_96_error");
        if (!Regex.IsMatch(passwd, "[A-Za-z]"))
          throw new AppException(1205, "password_needs_letter");
        if (!Regex.IsMatch(passwd, "^[A-Za-z0-9]*$"))
          throw new AppException(1206, "password_alpha_numeric");
      }));
    }
  }
}
