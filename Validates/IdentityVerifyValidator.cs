// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.IdentityVerifyValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using System;
using System.Linq.Expressions;
using tradeapi.Common;
using tradeapi.Models.Member;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Validates
{
    public class IdentityVerifyValidator : AbstractValidator<VerifyIdentityRequest>
    {
        public IdentityVerifyValidator()
        {
            this.RuleFor<string>((Expression<Func<VerifyIdentityRequest, string>>) (x => x.img_front)).Custom<VerifyIdentityRequest, string>((Action<string, ValidationContext<VerifyIdentityRequest>>) ((img_front, context) =>
            {
                if (string.IsNullOrEmpty(img_front))
                    throw new AppException(1221, "need_to_upload_front_id_card");
                if (!Tool.IsValidImageFormat(img_front))
                    throw new AppException(1222, "front_id_card_format_invalid");
            }));
            this.RuleFor<string>((Expression<Func<VerifyIdentityRequest, string>>) (x => x.id_number)).Custom<VerifyIdentityRequest, string>((Action<string, ValidationContext<VerifyIdentityRequest>>) ((id, context) =>
            {
                if (MemberServices.FindByIdCard(id) != null)
                    throw new AppException(1304, "id_already_exist");
            }));
        }
    }
}