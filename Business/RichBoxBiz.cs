// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.RichBoxBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.RichBox;
using tradeapi.Models.Wallet;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class RichBoxBiz
  {
    public static Decimal NotRecordedInterest(
      int member_fk,
      Decimal total_principal,
      RichboxConfigDto richbox_config)
    {
      DateTime utcNow = DateTime.UtcNow;
      DateTime? nullable1 = RichboxPrincipalService.FirstDate(member_fk);
      DateTime? nullable2 = RichboxInterestService.LastDate(member_fk) ?? RichboxPrincipalService.LastDate(member_fk);
      if (nullable1.HasValue && nullable2.HasValue && total_principal >= richbox_config.begin_profit)
      {
        DateTime dateTime = utcNow;
        DateTime? nullable3 = nullable1;
        if ((nullable3.HasValue ? new TimeSpan?(dateTime - nullable3.GetValueOrDefault()) : new TimeSpan?()).Value.TotalHours >= 24.0)
        {
          Decimal num1 = (Decimal) Math.Max((utcNow - nullable2.Value).TotalMinutes - 1.0, 0.0);
          Decimal num2 = richbox_config.interest_rate / 365M / 24M / 60M;
          return total_principal * num1 * num2;
        }
      }
      return 0M;
    }

    public static Decimal Last24HrPricipalInterest(int member_fk, RichboxConfigDto richbox_config)
    {
      DateTime now = DateTime.UtcNow;
      List<RichboxPrincipalDto> list = RichboxPrincipalService.FindAllByMember(member_fk).Where<RichboxPrincipalDto>((Func<RichboxPrincipalDto, bool>) (x => x.amount > richbox_config.begin_profit && x.date >= now.AddHours(-24.0))).OrderBy<RichboxPrincipalDto, DateTime>((Func<RichboxPrincipalDto, DateTime>) (x => x.date)).ToList<RichboxPrincipalDto>();
      Decimal num1 = 0M;
      if (list != null && list.Any<RichboxPrincipalDto>())
      {
        Decimal num2 = richbox_config.interest_rate / 365M / 24M / 60M;
        Decimal num3 = 0M;
        for (int index = 0; index < list.Count; ++index)
        {
          num3 += list[index].amount;
          if (num3 > richbox_config.begin_profit)
          {
            Decimal num4 = (Decimal) Math.Max(((index < list.Count - 1 ? list[index + 1].date : now) - list[index].date).TotalMinutes - 1.0, 0.0);
            num1 += num3 * num4 * num2;
          }
        }
      }
      return num1;
    }

    public static string GetRechargeMessage(int member_fk)
    {
      DateTime now = DateTime.UtcNow;
      RichboxConfigDto richbox_config = RichboxConfigService.Find();
      return RichBoxRecordService.FindAllByMember(member_fk).Where<RichBoxRecordDto>((Func<RichBoxRecordDto, bool>) (x => x.src == 1 && x.affect > richbox_config.begin_profit && x.create_time >= now.AddHours(-24.0))).Sum<RichBoxRecordDto>((Func<RichBoxRecordDto, Decimal>) (x => x.affect)) < richbox_config.begin_profit ? "rb_insufficient_balance_to_interest" : "";
    }

    public static BookResponse GetBook()
    {
      RichboxConfigDto richboxConfigDto = RichboxConfigService.Find();
      return new BookResponse()
      {
        interest_rate = richboxConfigDto?.interest_rate.Decimal(),
        begin_profit = richboxConfigDto?.begin_profit.Decimal()
      };
    }

    public static BookResponse GetBook(int member_fk)
    {
      RichboxConfigDto richboxConfigDto = RichboxConfigService.Find();
      Decimal richBoxRate = MemberBiz.GetRichBoxRate(member_fk);
      richboxConfigDto.interest_rate = richBoxRate;
      WalletResponse walletById = WalletBiz.GetWalletById(member_fk);
      Decimal totalWithdraw = RichBoxRecordService.GetTotalWithdraw(member_fk);
      Decimal num = walletById.richbox_interest - totalWithdraw;
      return new BookResponse()
      {
        interest_rate = richboxConfigDto?.interest_rate.Decimal(),
        begin_profit = richboxConfigDto?.begin_profit.Decimal(),
        accrued_interest = walletById.richbox_interest,
        wallet_balance = walletById.balance - walletById.freeze,
        principal_balance = walletById.richbox_balance,
        interest_balance = num < 0M ? 0M : num
      };
    }

    public static BookResponse Recharge(int member_fk, RechargeRequest req)
    {
      if (req.amount <= 0M)
        throw new AppException(2387, "amount_must_be_greater_than_0");
      MemberDto member = MemberServices.GetMember(member_fk);
      if (Tool.ToBool(ConfigLib.Get("enable_id_auth")) && member.id_auth != 1)
        throw new AppException(1303, "no_id_auth_yet");
      if (!WalletLib.IsRequestrMoneyOk(member_fk, req.amount))
        throw new AppException(2346, "balance_not_enough");
      if (WalletLib.BuyFund(member_fk, req.amount))
      {
        Decimal richboxBalance = WalletService.Find(member_fk).richbox_balance;
        Decimal richBoxRate = MemberServices.GetRichBoxRate(member_fk);
        RichBoxRecordService.FindPkAfterInsert(new RichBoxRecordDto()
        {
          member_fk = member_fk,
          affect = req.amount,
          balance = richboxBalance,
          src = 1,
          create_time = DateTime.UtcNow,
          interest_rate = richBoxRate
        });
      }
      return RichBoxBiz.GetBook(member_fk);
    }

    public static BookResponse Withdraw(int member_fk, RichWithdrawRequest req)
    {
      if (req.amount <= 0M)
        throw new AppException(2387, "amount_must_be_greater_than_0");
      Decimal richBoxBalance = WalletService.GetRichBoxBalance(member_fk);
      RichBoxRecordService.GetTotalWithdraw(member_fk);
      if (richBoxBalance < req.amount)
        throw new AppException(2346, "balance_not_enough");
      if (WalletLib.SellFund(member_fk, req.amount))
      {
        Decimal richBoxRate = MemberServices.GetRichBoxRate(member_fk);
        RichBoxRecordService.FindPkAfterInsert(new RichBoxRecordDto()
        {
          member_fk = member_fk,
          affect = -req.amount,
          balance = richBoxBalance - req.amount,
          src = 1,
          create_time = DateTime.UtcNow,
          interest_rate = richBoxRate
        });
      }
      return RichBoxBiz.GetBook(member_fk);
    }

    public static BookResponse Interest(int member_fk, InterestRequest req)
    {
      if (req.amount <= 0M)
        throw new AppException(2387, "amount_must_be_greater_than_0");
      WalletDto walletDto = WalletService.Find(member_fk);
      Decimal richboxBalance = walletDto.richbox_balance;
      Decimal num = walletDto.richbox_interest - RichBoxRecordService.GetTotalWithdraw(member_fk);
      if (richboxBalance < req.amount || num < req.amount)
        throw new AppException(2346, "balance_not_enough");
      if (WalletLib.SellFund(member_fk, req.amount))
      {
        Decimal richBoxRate = MemberServices.GetRichBoxRate(member_fk);
        RichBoxRecordService.FindPkAfterInsert(new RichBoxRecordDto()
        {
          member_fk = member_fk,
          affect = -req.amount,
          balance = richboxBalance - req.amount,
          src = 1,
          create_time = DateTime.UtcNow,
          interest_rate = richBoxRate
        });
      }
      return RichBoxBiz.GetBook(member_fk);
    }

    public static List<RichHistoryResponse> GetHistory(int member_fk, RichHistoryRequest req)
    {
      IEnumerable<RichBoxRecordDto> all = RichBoxRecordService.FindAll(member_fk);
      IEnumerable<RichHistoryResponse> richHistoryResponses;
      switch (req.type)
      {
        case 1:
          richHistoryResponses = all.Select<RichBoxRecordDto, RichHistoryResponse>((Func<RichBoxRecordDto, RichHistoryResponse>) (x => new RichHistoryResponse()
          {
            src = x.src,
            date = x.create_time,
            type = x.affect > 0M ? 1 : 2,
            amount = x.affect,
            blance = x.balance
          }));
          break;
        case 2:
          richHistoryResponses = all.Where<RichBoxRecordDto>((Func<RichBoxRecordDto, bool>) (x => x.affect > 0M && x.src == 1)).Select<RichBoxRecordDto, RichHistoryResponse>((Func<RichBoxRecordDto, RichHistoryResponse>) (x => new RichHistoryResponse()
          {
            src = x.src,
            date = x.create_time,
            type = 1,
            amount = x.affect,
            blance = x.balance
          }));
          break;
        case 3:
          richHistoryResponses = all.Where<RichBoxRecordDto>((Func<RichBoxRecordDto, bool>) (x => x.affect < 0M)).Select<RichBoxRecordDto, RichHistoryResponse>((Func<RichBoxRecordDto, RichHistoryResponse>) (x => new RichHistoryResponse()
          {
            src = x.src,
            date = x.create_time,
            type = 2,
            amount = x.affect,
            blance = x.balance
          }));
          break;
        case 4:
          richHistoryResponses = all.Where<RichBoxRecordDto>((Func<RichBoxRecordDto, bool>) (x => x.src == 2)).Select<RichBoxRecordDto, RichHistoryResponse>((Func<RichBoxRecordDto, RichHistoryResponse>) (x => new RichHistoryResponse()
          {
            src = x.src,
            date = x.create_time,
            type = x.affect > 0M ? 1 : 2,
            amount = x.affect,
            blance = x.balance
          }));
          break;
        default:
          throw new ArgumentException("Invalid type value");
      }
      IEnumerable<RichHistoryResponse> source = richHistoryResponses;
      return source.ToList<RichHistoryResponse>().GetRange(0, Math.Min(source.Count<RichHistoryResponse>(), 20));
    }
  }
}
