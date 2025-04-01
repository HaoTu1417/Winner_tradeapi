using System.Linq.Expressions;
using FluentValidation;
using tradeapi.Common;
using tradeapi.Models.Wallet;

namespace tradeapi.Validates;

public class JYRechargeApplyValidator: AbstractValidator<RechargeapplyRequest>
{
    public JYRechargeApplyValidator()
    {
        this.RuleFor<long?>((Expression<Func<RechargeapplyRequest, long?>>) (x => x.time_stamp)).Custom<RechargeapplyRequest, long?>((Action<long?, ValidationContext<RechargeapplyRequest>>) ((time, context) =>
        {
            if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                throw new AppException(1170, "request_time_out");
        }));
        this.RuleFor<long?>((Expression<Func<RechargeapplyRequest, long?>>) (x => x.time_stamp)).Custom<RechargeapplyRequest, long?>((Action<long?, ValidationContext<RechargeapplyRequest>>) ((time, context) =>
        {
            if (!time.HasValue || time.Value + 600L < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                throw new AppException(1170, "request_time_out");
        }));
    }
}