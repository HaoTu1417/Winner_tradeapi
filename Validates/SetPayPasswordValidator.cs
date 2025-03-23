// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.SetPayPasswordValidator
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
using tradeapi.Models.Member;

#nullable enable
namespace tradeapi.Validates
{
  public class SetPayPasswordValidator : AbstractValidator<SetPayPasswordRequest>
  {
    public SetPayPasswordValidator()
    {
      this.RuleFor<long?>((Expression<Func<SetPayPasswordRequest, long?>>) (x => x.time_stamp)).Custom<SetPayPasswordRequest, long?>((Action<long?, ValidationContext<SetPayPasswordRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
      this.RuleFor<string>((Expression<Func<SetPayPasswordRequest, string>>) (x => x.newpasswd)).Custom<SetPayPasswordRequest, string>((Action<string, ValidationContext<SetPayPasswordRequest>>) ((passwd, context) =>
      {
        if (string.IsNullOrEmpty(passwd))
          throw new AppException(1207, "enter_password");
        if (string.IsNullOrEmpty(passwd))
          throw new AppException(1207, "enter_password");
        if (passwd.Length != 6)
          throw new AppException(2381, "paypassword_length_6_error");
        if (!Regex.IsMatch(passwd, "^[0-9]*$"))
          throw new AppException(2382, "password_only_number");
      }));
    }

    public int DbAuth(SetPayPasswordRequest input, TokenModel tokenModel)
    {
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      if (memberDto == null)
        throw new AppException(1270, "none_account");
      if (!memberDto.status)
        throw new AppException(1230, "user_permission_closed");
      if (!BCrypt.Net.BCrypt.Verify(input.password, memberDto.passwd))
        throw new AppException(1240, "user_verify_failed");
      return memberDto.pk;
    }
  }
}
