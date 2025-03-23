// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.ResetPasswordValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using Models.Dto;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.Member;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Validates
{
  public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
  {
    public ResetPasswordValidator()
    {
      this.RuleFor<long?>((Expression<Func<ResetPasswordRequest, long?>>) (x => x.time_stamp)).Custom<ResetPasswordRequest, long?>((Action<long?, ValidationContext<ResetPasswordRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
      this.RuleFor<string>((Expression<Func<ResetPasswordRequest, string>>) (x => x.orgpasswd)).Custom<ResetPasswordRequest, string>((Action<string, ValidationContext<ResetPasswordRequest>>) ((passwd, context) =>
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
      this.RuleFor<string>((Expression<Func<ResetPasswordRequest, string>>) (x => x.newpasswd)).Custom<ResetPasswordRequest, string>((Action<string, ValidationContext<ResetPasswordRequest>>) ((passwd, context) =>
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

    public void OldDbAuth(string userid, string password)
    {
      AccountDto accountDto = new AccountServices().Get(userid);
      if (accountDto == null)
        throw new AppException(1270, "none_account");
      if (accountDto.sub_pwd != password)
        throw new AppException(1240, "user_verify_failed");
      if (accountDto.close_time.HasValue && accountDto.close_time.Value.AddDays(40.0) < DateTime.Now)
        throw new AppException(1230, "user_permission_closed");
    }

    public int DbAuth(TokenModel tokenModel, ResetPasswordRequest input)
    {
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      if (memberDto == null)
        throw new AppException(1270, "none_account");
      if (!memberDto.status)
        throw new AppException(1230, "user_permission_closed");
      if (!BCrypt.Net.BCrypt.Verify(input.orgpasswd, memberDto.passwd))
        throw new AppException(1240, "user_verify_failed");
      return memberDto.pk;
    }

    private static bool VersionWork(string version) => true;
  }
}
