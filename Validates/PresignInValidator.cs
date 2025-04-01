// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.SignInValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using tradeapi.Common;
using tradeapi.Models.Member;

#nullable enable
namespace tradeapi.Validates
{
  public class PresignInValidator : AbstractValidator<SignInRequest>
  {
    public PresignInValidator()
    {
      this.RuleFor<string>((Expression<Func<SignInRequest, string>>) (x => x.email)).Custom<SignInRequest, string>((Action<string, ValidationContext<SignInRequest>>) ((email, context) =>
      {
        if (string.IsNullOrEmpty(email))
          throw new AppException(1202, "fill_email");
      }));
      this.RuleFor<string>((Expression<Func<SignInRequest, string>>) (x => x.passwd)).Custom<SignInRequest, string>((Action<string, ValidationContext<SignInRequest>>) ((passwd, context) =>
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
      this.RuleFor<string>((Expression<Func<SignInRequest, string>>) (x => x.lang)).Custom<SignInRequest, string>((Action<string, ValidationContext<SignInRequest>>) ((lang, context) =>
      {
        if (string.IsNullOrEmpty(lang))
          throw new AppException(1217, "incorrect_language");
      }));
      this.RuleFor<long?>((Expression<Func<SignInRequest, long?>>) (x => x.time_stamp)).Custom<SignInRequest, long?>((Action<long?, ValidationContext<SignInRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
    }

    public int DbAuth(SignInRequest input)
    {
      MemberResponse byUsernameOrEmail = MemberServices.GetByUsernameOrEmail(input.email);
      if (byUsernameOrEmail == null)
        throw new AppException(1270, "none_account");
      if (byUsernameOrEmail.status == 0)
        throw new AppException(1230, "user_permission_closed");
      try
      {
        if (!BCrypt.Net.BCrypt.Verify(input.passwd, byUsernameOrEmail.passwd))
          throw new AppException(1209, "incorrect_password");
      }
      catch (AppException ex)
      {
        BCrypt.Net.BCrypt.HashPassword(input.passwd);
        throw new AppException(1209, "incorrect_password");
      }
      catch (Exception ex)
      {
        throw new AppException(1230, "user_permission_closed");
      }
      return byUsernameOrEmail.pk;
    }

    private static bool VersionWork(string version) => true;
  }
}
