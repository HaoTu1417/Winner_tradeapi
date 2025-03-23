// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.SendVerifyCodeSmsValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using System.Text.RegularExpressions;
using tradeapi.Common;
using tradeapi.Models.Member;

#nullable enable
namespace tradeapi.Validates
{
    public class SendVerifyCodeSmsValidator : AbstractValidator<SendphoneVerifyRequest>
    {
        public SendVerifyCodeSmsValidator()
        {
            this.RuleFor(x => new
            {
                CountryCode = x.CountryCode,
                PhoneNumber = x.PhoneNumber
            }).Custom((phone, context) =>
            {
                if (string.IsNullOrEmpty(phone.CountryCode))
                    throw new AppException(2421, "sms_fill_country_code");
                if (string.IsNullOrEmpty(phone.PhoneNumber))
                    throw new AppException(2422, "sms_fill_phone_number");
                if (!Regex.IsMatch(phone.CountryCode, "^\\+[0-9]+$"))
                    throw new AppException(2423, "sms_invalid_country_code_format");
                if (!Regex.IsMatch(phone.PhoneNumber, "^[0-9]+$"))
                    throw new AppException(2424, "sms_invalid_phone_number_format");
            });
        }
    }
}