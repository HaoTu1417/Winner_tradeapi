// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.VerifyMailCodeValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models.Member;

#nullable enable
namespace tradeapi.Validates
{
    public class VerifyMailCodeValidator : AbstractValidator<VerifyMailCodeRequest>
    {
        public VerifyMailCodeValidator()
        {
            this.RuleFor<string>((Expression<Func<VerifyMailCodeRequest, string>>) (x => x.email)).Custom<VerifyMailCodeRequest, string>((Action<string, ValidationContext<VerifyMailCodeRequest>>) ((email, context) =>
            {
                if (string.IsNullOrEmpty(email))
                    throw new AppException(1202, "fill_email");
                if (!new EmailAddressAttribute().IsValid((object) email))
                    throw new AppException(1203, "invalid_email_format");
            }));
        }

        public void CheckMember(string email)
        {
            if (MemberServices.CheckEmailExists(email))
                throw new AppException(1201, "already_member");
        }

        public bool CheckMailInMember(string email) => MemberServices.CheckEmailExists(email);
    }
}