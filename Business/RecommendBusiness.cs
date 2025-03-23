// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.RecommendBusiness
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Collections.Generic;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Recommend;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Business
{
  public class RecommendBusiness
  {
    public static RecommendInfoResponse GetRecommendInfo(int member_fk)
    {
      int invitationsByMemberFk = RecommendRegisterService.GetAllInvitationsByMemberFk(member_fk);
      Decimal rewardByMemberFk = RecommendRewardService.GetTotalRewardByMemberFk(member_fk);
      return new RecommendInfoResponse()
      {
        total_invitations = invitationsByMemberFk,
        total_reward = rewardByMemberFk
      };
    }

    public static List<RecommendRegisterResponse> GetRecommendRegisters(int member_fk)
    {
      return RecommendRegisterService.FindByMemberFk(member_fk);
    }

    public static List<AllRecommendRewradInfoResponse> GetAllRecommendRewradInfo(int member_fk)
    {
      List<AllRecommendRewradInfoResponse> byMember = RecommendRewardService.FindByMember(member_fk);
      if (byMember != null)
      {
        string str = DateTime.Now.ToString("yyMM");
        foreach (AllRecommendRewradInfoResponse rewradInfoResponse in byMember)
        {
          if (rewradInfoResponse.state == -1 && str != rewradInfoResponse.yymm)
          {
            rewradInfoResponse.state = 0;
            if (rewradInfoResponse.total_reward == 0M)
              RecommendRewardService.UpdateStatus(member_fk, rewradInfoResponse.yymm);
          }
          rewradInfoResponse.rewards = RecommendRewardSummaryService.FindByMemberFkAndYymm(member_fk, rewradInfoResponse.yymm);
        }
      }
      return byMember;
    }

    public static List<RecommendRewardDetailResponse> GetRecommendRewardDetails(
      int member_fk,
      string yymm)
    {
      return RecommendRewardDetailService.FindByYearMonth(member_fk, yymm);
    }

    public static void RegisterInvitee(int invitee, string invitation_code)
    {
      MemberDto byInvitationCode1 = MemberServices.GetMemberByInvitationCode(invitation_code);
      if (byInvitationCode1 != null)
      {
        RecommendRegisterService.Insert(new RecommendRegisterDto()
        {
          member_fk = byInvitationCode1.pk,
          invitee_fk = invitee,
          register_date = DateTime.UtcNow,
          is_admin = false
        });
        MemberServices.UpdateRecommend(invitee, invitation_code, byInvitationCode1.pk);
      }
      AdminUserDto byInvitationCode2 = MemberServices.GetAdminUserByInvitationCode(invitation_code);
      if (byInvitationCode2 == null)
        return;
      RecommendRegisterService.Insert(new RecommendRegisterDto()
      {
        member_fk = byInvitationCode2.pk,
        invitee_fk = invitee,
        register_date = DateTime.UtcNow,
        is_admin = true
      });
      MemberServices.UpdateRecommend(invitee, invitation_code, byInvitationCode2.pk);
      MemberServices.UpdateMemberServer(invitee, byInvitationCode2.pk);
    }

    private static double GetProfitRate(int deep)
    {
      switch (deep)
      {
        case 1:
          return Convert.ToDouble(ConfigLib.Get("layer_rate_1"));
        case 2:
          return Convert.ToDouble(ConfigLib.Get("layer_rate_2"));
        case 3:
          return Convert.ToDouble(ConfigLib.Get("layer_rate_3"));
        default:
          return 0.0;
      }
    }

    public static void Profit(int borrowFeeId)
    {
      BorrowFeeDto borrowFeeDto = BorrowFeeService.Find(borrowFeeId);
      if (borrowFeeDto == null || borrowFeeDto.fee_received == 0M)
        return;
      MemberDto memberDto = MemberServices.Find(borrowFeeDto.member_fk);
      if (memberDto != null && memberDto.is_test_account)
      {
        LogLib.Log("this account " + memberDto.account + " is test account, no recommend profit");
      }
      else
      {
        DateTime createTime = borrowFeeDto.create_time;
        RecommendProfitModel data = new RecommendProfitModel()
        {
          org = borrowFeeDto.member_fk,
          dt = createTime,
          yymm = createTime.ToString("yyMM"),
          money = (double) borrowFeeDto.fee_received,
          borrow_fee_fk = borrowFeeId,
          currency = ConfigLib.Get("wallet_currency")
        };
        RecommendBusiness.AssignProfit(1, borrowFeeDto.member_fk, data);
      }
    }

    private static void AssignProfit(int deep, int man, RecommendProfitModel data)
    {
      if (deep > 3)
        return;
      try
      {
        MemberDto parent = MemberServices.FindParent(man);
        if (parent == null)
          return;
        if (parent.is_test_account)
        {
          LogLib.Log("parent account " + parent.account + " is test account, no recommend profit");
        }
        else
        {
          RecommendRewardDetailDto source = new RecommendRewardDetailDto()
          {
            member_fk = data.org,
            parent = parent.pk,
            borrow_fee_fk = data.borrow_fee_fk,
            yymm = data.yymm,
            borrow_date = data.dt,
            currency = data.currency,
            management_fee = data.money,
            generation = deep,
            rate = RecommendBusiness.GetProfitRate(deep)
          };
          source.reward = source.rate * source.management_fee;
          RecommendRewardDetailService.FindPkAfterInsert(source);
          RecommendBusiness.SaveRecommendReward(data.yymm, parent.pk, (Decimal) source.reward);
          RecommendBusiness.UpdateSummary(data.yymm, parent.pk, deep);
          WalletLib.DirectSellingPass(parent.pk, (Decimal) source.reward, Convert.ToInt32(data.yymm.Substring(0, 2)), Convert.ToInt32(data.yymm.Substring(2)));
          RecommendBusiness.AssignProfit(deep + 1, parent.pk, data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendBusiness][AssignProfit]" + ex.Message);
      }
    }

    private static void SaveRecommendReward(string yymm, int member, Decimal reward)
    {
      if (RecommendRewardService.FindByMemberAndYYMM(member, yymm) == null)
      {
        int int32 = Convert.ToInt32(yymm);
        int num1 = 2000 + int32 / 100;
        int num2 = int32 % 100;
        RecommendRewardService.FindPkAfterInsert(new RecommendRewardDto()
        {
          member_fk = member,
          yymm = yymm,
          year = num1,
          month = num2,
          currency = ConfigLib.Get("wallet_currency"),
          total_reward = reward,
          state = -1,
          create_time = DateTime.UtcNow
        });
      }
      else
        RecommendRewardService.UpdateReward(member, yymm);
    }

    private static void UpdateSummary(string yymm, int pk, int deep)
    {
      RecommendRewardSummaryDto byYymmAndDeep = RecommendRewardSummaryService.FindByYymmAndDeep(yymm, pk, deep);
      if (byYymmAndDeep == null)
      {
        RecommendRewardSummaryService.InsertAgent(yymm);
      }
      else
      {
        MonthProfitModel profit = RecommendRewardDetailService.FindProfit(yymm, pk, deep);
        if (profit == null)
          return;
        RecommendRewardSummaryService.UpdateByYymmAndDeep(profit, byYymmAndDeep.pk);
      }
    }

    public static void Withdraw(int member_fk, string yymm)
    {
      RecommendRewardDto byYearMonth = RecommendRewardService.FindByYearMonth(member_fk, yymm);
      if (byYearMonth == null)
        throw new AppException(3110, "none_promotional_income");
      switch (byYearMonth.state)
      {
        case -1:
          if (DateTime.Now.ToString("yyMM") == yymm)
            throw new AppException(3120, "month_promotional_not_complete");
          break;
        case 2:
          throw new AppException(3130, "promotional_have_withdrawn");
      }
      if (!WalletLib.DirectSellingPass(member_fk, byYearMonth.total_reward, byYearMonth.year, byYearMonth.month))
        return;
      RecommendRewardService.UpdateStatus(member_fk, yymm);
      string str1 = ConfigLib.Get("wallet_currency");
      object[] objArray1 = new object[4];
      object[] objArray2 = objArray1;
      int num = byYearMonth.year;
      string str2 = num.ToString();
      objArray2[0] = (object) str2;
      object[] objArray3 = objArray1;
      num = byYearMonth.month;
      string str3 = num.ToString();
      objArray3[1] = (object) str3;
      objArray1[2] = (object) byYearMonth.total_reward;
      objArray1[3] = (object) str1;
      SendMessageLib.Send(member_fk, 25, objArray1);
    }
  }
}
