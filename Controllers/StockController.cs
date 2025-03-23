// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.StockController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi.Models.Quotate;
using tradeapi.Models.Stock;
using tradeapi.Services;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class StockController : ApiController
  {
    [HttpPost("info")]
    public APIResponse UpsertStockInfo(StockDto dto)
    {
      try
      {
        StockServices.Upsert(dto);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        return APIResponse.Error(ex.GetStatus(), "");
      }
    }

    [HttpPost("getallstockoption")]
    public APIResponse<List<StockOptionDto>> GetAllStockOption(MarketIndexRequest req)
    {
      try
      {
        return APIResponse<List<StockOptionDto>>.Ok(StockBiz.GetAllStockOption(req.market));
      }
      catch (AppException ex)
      {
        return APIResponse<List<StockOptionDto>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getavailablequantity")]
    public APIResponse<AvailableQuantityResponse> GetAvailableQuantity(AvailableQuantityRequest req)
    {
      try
      {
        int availableQuantity = StockBiz.GetAvailableQuantity(this.GetToken().member_fk, req.stock_option_fk);
        return APIResponse<AvailableQuantityResponse>.Ok(new AvailableQuantityResponse()
        {
          quantity = availableQuantity
        });
      }
      catch (AppException ex)
      {
        return APIResponse<AvailableQuantityResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("buystockoption")]
    public APIResponse BuyStockOption(BuyStockOptionRequest req)
    {
      try
      {
        StockBiz.BuyStockOption(this.GetToken().member_fk, req.stock_option_fk, req.quantity);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("sellstockoption")]
    public APIResponse SellStockOption(SellStockOptionRequest req)
    {
      try
      {
        StockBiz.SellStockOption(this.GetToken().member_fk, req.stock_code, req.quantity, req.market);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getuserstockoption")]
    public APIResponse<List<GetStockOptionPotitionResponse>> GetUserStockOption(LangRequest req)
    {
      try
      {
        return APIResponse<List<GetStockOptionPotitionResponse>>.Ok(StockBiz.GetAllStockOptionPosition(this.GetToken().member_fk));
      }
      catch (AppException ex)
      {
        return APIResponse<List<GetStockOptionPotitionResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getuserstockoptiondetails")]
    public APIResponse<List<GetStockOptionDetailResponse>> GetUserStockOptionDetail(
      GetUserStockOptionDetailRequest req)
    {
      try
      {
        return APIResponse<List<GetStockOptionDetailResponse>>.Ok(StockBiz.GetStockOptionRecordDetail(this.GetToken().member_fk, req.market, req.stock_code));
      }
      catch (AppException ex)
      {
        return APIResponse<List<GetStockOptionDetailResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
