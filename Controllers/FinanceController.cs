// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.FinanceController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Finance;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class FinanceController : ApiController
  {
    [HttpPost("getitems")]
    public APIResponse<List<GetItemsResponse>> GetItems(BorrowPlanRequest req)
    {
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<GetItemsResponse>>.Ok(FinanceBiz.GetItems(req.market, req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[FinanceController][GetItems]" + ex.Message);
        return APIResponse<List<GetItemsResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("trial")]
    public APIResponse<BorrowPlanResponse> Trial(BorrowPlanRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<BorrowPlanResponse>.Ok(FinanceBiz.GetBorrowPlan(req, "trial", token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[FinanceController][Trial]" + ex.Message);
        return APIResponse<BorrowPlanResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("free")]
    public APIResponse<BorrowPlanResponse> Free(BorrowPlanRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<BorrowPlanResponse>.Ok(FinanceBiz.GetBorrowPlan(req, "free", token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[FinanceController][Free]" + ex.Message);
        return APIResponse<BorrowPlanResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("day")]
    public APIResponse<BorrowPlanResponse> Day(BorrowPlanRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<BorrowPlanResponse>.Ok(FinanceBiz.GetBorrowPlan(req, "day", token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[FinanceController][Day]" + ex.Message);
        return APIResponse<BorrowPlanResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("week")]
    public APIResponse<BorrowPlanResponse> Week(BorrowPlanRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<BorrowPlanResponse>.Ok(FinanceBiz.GetBorrowPlan(req, "week", token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[FinanceController][Week]" + ex.Message);
        return APIResponse<BorrowPlanResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("month")]
    public APIResponse<BorrowPlanResponse> Month(BorrowPlanRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<BorrowPlanResponse>.Ok(FinanceBiz.GetBorrowPlan(req, "month", token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[FinanceController][Month]" + ex.Message);
        return APIResponse<BorrowPlanResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("vip")]
    public APIResponse<BorrowPlanResponse> Vip(BorrowPlanRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<BorrowPlanResponse>.Ok(FinanceBiz.GetBorrowPlan(req, "vip", token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[FinanceController][Vip]" + ex.Message);
        return APIResponse<BorrowPlanResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
