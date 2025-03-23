// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.RecommendController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Recommend;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class RecommendController : ApiController
  {
    [HttpPost("getrecommendinfo")]
    public APIResponse<RecommendInfoResponse> GetRecommendInfo()
    {
      TokenModel token = this.GetToken();
      try
      {
        if (token != null)
          return APIResponse<RecommendInfoResponse>.Ok(RecommendBusiness.GetRecommendInfo(token.member_fk));
        return APIResponse<RecommendInfoResponse>.Ok(new RecommendInfoResponse()
        {
          total_invitations = 0,
          total_reward = 0M
        });
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RecommendController][GetRecommendInfo]" + ex.Message);
        return APIResponse<RecommendInfoResponse>.Error(ex.GetStatus(), "Get Recommended Register failed");
      }
    }

    [HttpPost("getrecommendedmembers")]
    public APIResponse<List<RecommendRegisterResponse>> GetRecommendedMembers()
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<RecommendRegisterResponse>>.Ok(RecommendBusiness.GetRecommendRegisters(token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RecommendController][GetRecommendedMembers]" + ex.Message);
        return APIResponse<List<RecommendRegisterResponse>>.Error(ex.GetStatus(), "Get Recommended Members failed");
      }
    }

    [HttpPost("getallrecommendrewardinfo")]
    public APIResponse<List<AllRecommendRewradInfoResponse>> GetAllRecommendRewradInfo()
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<AllRecommendRewradInfoResponse>>.Ok(RecommendBusiness.GetAllRecommendRewradInfo(token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RecommendController][GetAllRecommendRewradInfo]" + ex.Message);
        return APIResponse<List<AllRecommendRewradInfoResponse>>.Error(ex.GetStatus(), "Get Recommend Rewrad failed");
      }
    }

    [HttpPost("getrewarddetails")]
    public APIResponse<List<RecommendRewardDetailResponse>> GetRecommendRewardDetails(
      RecommendRewardRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<RecommendRewardDetailResponse>>.Ok(RecommendBusiness.GetRecommendRewardDetails(token.member_fk, req.yymm));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RecommendController][GetRecommendRewardDetails]" + ex.Message);
        return APIResponse<List<RecommendRewardDetailResponse>>.Error(ex.GetStatus(), "Get recommend details failed");
      }
    }

    [HttpPost("withdraw")]
    public APIResponse Withdraw(RecommendRewardRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        RecommendBusiness.Withdraw(token.member_fk, req.yymm);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RecommendController][Withdraw]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.Message);
      }
    }
  }
}
