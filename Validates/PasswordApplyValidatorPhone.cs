using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FluentValidation;
using tradeapi.Common;
using tradeapi.Models.Member;
using tradeApi2.Models.Member;

namespace tradeapi.Validates;


public class PasswordApplyValidatorPhone: AbstractValidator<PasswordApplyPhoneRequest>
{
    public PasswordApplyValidatorPhone()
    {
        this.RuleFor<long?>((Expression<Func<PasswordApplyPhoneRequest, long?>>) (x => x.time_stamp)).Custom<PasswordApplyPhoneRequest, long?>((Action<long?, ValidationContext<PasswordApplyPhoneRequest>>) ((time, context) =>
        {
            if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                throw new AppException(1170, "request_time_out");
        }));
        this.RuleFor<string>((Expression<Func<PasswordApplyPhoneRequest, string>>) (x => x.phone_verifyCode)).Custom<PasswordApplyPhoneRequest, string>((Action<string, ValidationContext<PasswordApplyPhoneRequest>>) ((phone_verifyCode, context) =>
        {
            if (string.IsNullOrEmpty(phone_verifyCode))
                throw new AppException(1215, "fill_sms_verification_code"); // New error code
            if (!Regex.IsMatch(phone_verifyCode, "^\\d{4}$"))
                throw new AppException(1216, "incorrect_sms_verification_code");
        }));
        this.RuleFor<string>((Expression<Func<PasswordApplyPhoneRequest, string>>) (x => x.phone)).Custom<PasswordApplyPhoneRequest, string>((Action<string, ValidationContext<PasswordApplyPhoneRequest>>) ((phone, context) =>
        {
            if (string.IsNullOrEmpty(phone))
                throw new AppException(1227, "fill_phone"); // New error code
            if (!Regex.IsMatch(phone, "^\\d{10}$"))
                throw new AppException(1228, "incorrect_phone_format");
        }));
        this.RuleFor<string>((Expression<Func<PasswordApplyPhoneRequest, string>>) (x => x.newpasswd)).Custom<PasswordApplyPhoneRequest, string>((Action<string, ValidationContext<PasswordApplyPhoneRequest>>) ((passwd, context) =>
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
