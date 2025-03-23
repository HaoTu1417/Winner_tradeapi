// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.ResetPhoneNumberValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using Models.Dto;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi.Models.Member;

#nullable enable
namespace tradeapi.Validates
{
  public class ResetPhoneNumberValidator : AbstractValidator<ResetPhoneRequest>
  {
    public ResetPhoneNumberValidator()
    {
      this.RuleFor<long?>((Expression<Func<ResetPhoneRequest, long?>>) (x => x.time_stamp)).Custom<ResetPhoneRequest, long?>((Action<long?, ValidationContext<ResetPhoneRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
      this.RuleFor<string>((Expression<Func<ResetPhoneRequest, string>>) (x => x.country_code)).Custom<ResetPhoneRequest, string>((Action<string, ValidationContext<ResetPhoneRequest>>) ((country_code, context) =>
      {
        if (string.IsNullOrEmpty(country_code))
          throw new AppException(1231, "fill_country");
      }));
      this.RuleFor<string>((Expression<Func<ResetPhoneRequest, string>>) (x => x.phone_number)).Custom<ResetPhoneRequest, string>((Action<string, ValidationContext<ResetPhoneRequest>>) ((phone_number, context) =>
      {
        if (string.IsNullOrEmpty(phone_number))
          throw new AppException(1301, "enter_phone_number");
      }));
      this.RuleFor<string>((Expression<Func<ResetPhoneRequest, string>>) (x => x.password)).Custom<ResetPhoneRequest, string>((Action<string, ValidationContext<ResetPhoneRequest>>) ((password, context) =>
      {
        if (string.IsNullOrEmpty(password))
          throw new AppException(1207, "enter_password");
      }));
    }

    public int DbAuth(TokenModel tokenModel, string password)
    {
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      if (memberDto == null)
        throw new AppException(1270, "none_account");
      if (!memberDto.status)
        throw new AppException(1230, "user_permission_closed");
      if (!BCrypt.Net.BCrypt.Verify(password, memberDto.passwd))
        throw new AppException(1240, "user_verify_failed");
      return memberDto.pk;
    }
  }
}
