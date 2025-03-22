using Microsoft.AspNetCore.Mvc;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Order;
using tradeapi2.Common;

// namespace tradeapi.Controllers;

public class OrderController
{
    
}// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.OrderController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

// using Microsoft.AspNetCore.Mvc;
// using tradeapi.Business;
// using tradeapi.Common;
// using tradeapi.Libs;
// using tradeapi.Models;
// using tradeapi.Models.Order;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class OrderController : ApiController
  {
    [HttpPost("presell")]
    public APIResponse<PreSellResponse> PreSell(PreSellRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (token.sub_account.IsEmpty() || token.market.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        return APIResponse<PreSellResponse>.Ok(OrderBiz.GetPreSell(token.market, token.sub_account, req.price_type, req.stock_code, req.price, req.volume));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[OrderController][PreSell]" + ex.Message);
        return APIResponse<PreSellResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("prebuy")]
    public APIResponse<PreBuyResponse> PreBuy(PreBuyRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (token.sub_account.IsEmpty() || token.market.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        return APIResponse<PreBuyResponse>.Ok(OrderBiz.GetPreBuy(token.market, token.sub_account, req.price_type, req.stock_code, req.price, req.volume));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[OrderController][PreBuy]" + ex.Message);
        return APIResponse<PreBuyResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("cancel")]
    public APIResponse Cancel(CancelRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        OrderBiz.Cancel(token.member_fk, this.GetIp(), req.trade_order_sn);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[OrderController][Cancel]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("order")]
    public APIResponse<OrderResponse> Order(OrderRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (token.sub_account.IsEmpty() || token.market.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        return APIResponse<OrderResponse>.Ok(OrderBiz.Order(token.member_fk, token.market, token.sub_account, req.price_type, req.stock_code, req.dir, req.price, req.volume, token.ip));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[OrderController][Order]" + ex.Message);
        return APIResponse<OrderResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("holding")]
    public APIResponse<OrderHoldingResponse> Holding(OrderHoldingRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (token.sub_account.IsEmpty() || token.market.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        return APIResponse<OrderHoldingResponse>.Ok(OrderBiz.GetHolding(token.market, token.sub_account, req.stock_code, req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[OrderController][Holding]" + ex.Message);
        return APIResponse<OrderHoldingResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
