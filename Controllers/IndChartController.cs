// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.IndChartController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.IndChart;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class IndChartController : ApiController
  {
    [HttpPost("history")]
    public APIResponse<HistoryResponse> History(HistoryRequest req)
    {
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<HistoryResponse>.Ok(new HistoryResponse()
        {
          data = IndChartBiz.GetHistory(req.period, req.market, req.stock_code)
        });
      }
      catch (AppException ex)
      {
        LogLib.Error("[IndChartController][History]" + ex.Message);
        return new APIResponse<HistoryResponse>(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("today")]
    public APIResponse<List<TodayResponse>> Today(TodayRequest req)
    {
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<TodayResponse>>.Ok(IndChartBiz.GetToday(req.market, req.stock_code));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[IndChartController][Today]" + ex.Message);
        return APIResponse<List<TodayResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("set")]
    public APIResponse Set(SetIndicatorRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        IndChartBiz.SetIndicator(token.member_fk, req.name, req.param1, req.param2, req.param3, req.param4, req.param5);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[IndChartController][Set]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("get")]
    public APIResponse<GetIndicatorResponse> Get(GetIndicatorRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<GetIndicatorResponse>.Ok(IndChartBiz.GetIndicator(token.member_fk, req.name));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[IndChartController][Get]" + ex.Message);
        return APIResponse<GetIndicatorResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
