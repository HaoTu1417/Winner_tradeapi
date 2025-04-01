// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.RegisterValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Models.Member;

#nullable enable
namespace tradeapi.Validates
{
  public class RegisterValidator : AbstractValidator<RegisterRequest>
  {
    public RegisterValidator()
    {
      this.RuleFor<string>((Expression<Func<RegisterRequest, string>>) (x => x.account)).Custom<RegisterRequest, string>((Action<string, ValidationContext<RegisterRequest>>) ((account, context) =>
      {
        // verify account input
        if (string.IsNullOrEmpty(account))
          throw new AppException(1234, "enter_account");
        if (account.Length < 4 || account.Length > 16)
          throw new AppException(1235, "account_length_4_16_error");
        if (!Regex.IsMatch(account, "^[A-Za-z][A-Za-z0-9]*$"))
          throw new AppException(1236, "account_alpha_numeric");
      }));
      
      //verify email format
      this.RuleFor<string>((Expression<Func<RegisterRequest, string>>) (x => x.email)).Custom<RegisterRequest, string>((Action<string, ValidationContext<RegisterRequest>>) ((email, context) =>
      {
        if (string.IsNullOrEmpty(email))
          throw new AppException(1202, "fill_email");
        if (!new EmailAddressAttribute().IsValid((object) email))
          throw new AppException(1203, "invalid_email_format");
       
      }));
      this.RuleFor<string>((Expression<Func<RegisterRequest, string>>) (x => x.passwd)).Custom<RegisterRequest, string>((Action<string, ValidationContext<RegisterRequest>>) ((passwd, context) =>
      {
        //verify password input
        if (string.IsNullOrEmpty(passwd))
          throw new AppException(1207, "enter_password");
        if (passwd.Length < 6 || passwd.Length > 96)
          throw new AppException(1208, "password_length_6_96_error");
        if (!Regex.IsMatch(passwd, "[A-Za-z]"))
          throw new AppException(1205, "password_needs_letter");
        if (!Regex.IsMatch(passwd, "^[A-Za-z0-9]*$"))
          throw new AppException(1206, "password_alpha_numeric");
      }));
      
      // verify mail otp format
      this.RuleFor<string>((Expression<Func<RegisterRequest, string>>) (x => x.verity_mail)).Custom<RegisterRequest, string>((Action<string, ValidationContext<RegisterRequest>>) ((verity, context) =>
      {
        if (!string.IsNullOrEmpty(verity) && !Regex.IsMatch(verity, "^\\d{4}$"))
          throw new AppException(1212, "incorrect_verification_code");
      }));
      
      // Verify phone number
      this.RuleFor<string>((Expression<Func<RegisterRequest, string>>)(x => x.mobile)).Custom<RegisterRequest, string>((Action<string, ValidationContext<RegisterRequest>>)((phone, context) =>
      {
          if (string.IsNullOrEmpty(phone))
              throw new AppException(1220, "fill_phone_number"); // Mã lỗi mới
          if (!Regex.IsMatch(phone, "^\\d{10}$"))
              throw new AppException(1219, "phone_number_must_be_10_digits");
      }));
      
      
      // Veriy sms otp format
      this.RuleFor(x => x.verify_phone).Custom((verifyPhone, context) =>
      {
        if (string.IsNullOrEmpty(verifyPhone))
          throw new AppException(1215, "fill_sms_verification_code"); // New error code
        if (!Regex.IsMatch(verifyPhone, "^\\d{4}$"))
          throw new AppException(1216, "incorrect_sms_verification_code");
      });
      this.RuleFor<string>((Expression<Func<RegisterRequest, string>>) (x => x.lang)).Custom<RegisterRequest, string>((Action<string, ValidationContext<RegisterRequest>>) ((language, context) =>
      {
        if (string.IsNullOrEmpty(language))
          throw new AppException(1217, "incorrect_language");
      }));
      this.RuleFor<long?>((Expression<Func<RegisterRequest, long?>>) (x => x.time_stamp)).Custom<RegisterRequest, long?>((Action<long?, ValidationContext<RegisterRequest>>) ((time, context) =>
      {
        if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
          throw new AppException(1170, "request_time_out");
      }));
    }

    private string CreatePassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string dbpassword)
    {
      return BCrypt.Net.BCrypt.Verify(password, dbpassword);
    }

    public void DbAuth(RegisterRequest input)
    {

      // Nếu có nhập email thì check cả email
      if (!string.IsNullOrEmpty(input.email))
      {
        if (MemberServices.CheckEmailExists(input.email))
          throw new AppException(1201, "already_member");
        if (!string.IsNullOrEmpty(input.verity_mail))
          VerifyBiz.CheckMailVerifyCode(input.email, input.verity_mail);
      }

      // Check if phone is duplicated

      if (MemberServices.CheckPhoneExists(input.mobile))
      {
        throw new AppException(1204, "already_phone");
      }
      // check code is correct (exist and match) 
      if (string.IsNullOrEmpty(input.verify_phone))
      {
        throw new AppException(1215, "fill_sms_verification_code");
        
      }
        VerifyBiz.CheckPhoneVerifyCode(input.mobile, input.verify_phone); // You may need to implement this
      
      
        
      
     
      
      
      
      if (MemberServices.CheckUsernameExists(input.account))
        throw new AppException(1233, "already_account");
      if (!string.IsNullOrEmpty(input.invitation_code) && MemberServices.CheckInvitationCodeExists(input.invitation_code) == 0)
        throw new AppException(1214, "incorrect_invitation_code");
    }
  }
}
