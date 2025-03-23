// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.QuotateController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Quotate;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class QuotateController : ApiController
  {
    [HttpPost("openmarket")]
    public APIResponse<List<MarketResponse>> OpenMarket(MarketIndexRequest req)
    {
      try
      {
        return APIResponse<List<MarketResponse>>.Ok(QuotateBiz.GetOpenMarket(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][OpenMarket]" + ex.Message);
        return APIResponse<List<MarketResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("market")]
    public APIResponse<List<MarketIndexResponse>> Market(MarketIndexRequest req)
    {
      try
      {
        return !req.market.IsEmpty() ? APIResponse<List<MarketIndexResponse>>.Ok(QuotateBiz.GetMarket(req.market)) : throw new AppException(1570, "market_not_exist");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][Market]" + ex.Message);
        return APIResponse<List<MarketIndexResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("rankincrease")]
    public APIResponse<List<RankResponse>> RankIncrease(RankRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token != null ? token.member_fk : 0;
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<RankResponse>>.Ok(QuotateBiz.GetRankIncrease(memberFk, req.market, req.exchange, 20));
      }
      catch (AppException ex)
      {
        LogLib.Error((Exception) ex);
        return APIResponse<List<RankResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("rankdecline")]
    public APIResponse<List<RankResponse>> RankDecline(RankRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token != null ? token.member_fk : 0;
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<RankResponse>>.Ok(QuotateBiz.GetRankDecline(memberFk, req.market, req.exchange, 20));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][RankDecline]" + ex.Message);
        return APIResponse<List<RankResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("rankvolume")]
    public APIResponse<List<RankVolumeResponse>> RankVolume(RankRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token != null ? token.member_fk : 0;
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<RankVolumeResponse>>.Ok(QuotateBiz.GetRankVolume(memberFk, req.market, req.exchange, 20));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][RankVolume]" + ex.Message);
        return APIResponse<List<RankVolumeResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("lastprice")]
    public APIResponse<LastPriceResponse> LastPrice(LastPriceRequest req)
    {
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<LastPriceResponse>.Ok(QuotateBiz.GetLastPrice(req.market, req.stock_code, req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][LastPrice]" + ex.Message);
        return APIResponse<LastPriceResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("likecode")]
    public APIResponse<List<LikeCodeResponse>> LikeCode(LikeCodeRequest req)
    {
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<LikeCodeResponse>>.Ok(QuotateBiz.GetLikeCode(req.market, req.keyword));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][LikeCode]" + ex.Message);
        return APIResponse<List<LikeCodeResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("filterstock")]
    public APIResponse<List<FilterStockResponse>> FilterStock(FilterStockRequest req)
    {
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<FilterStockResponse>>.Ok(QuotateBiz.FilterStock(req.market, req.first_code));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][FilterStock]" + ex.Message);
        return APIResponse<List<FilterStockResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("favourite")]
    public APIResponse<List<FavouriteResponse>> Favourite(FavouriteRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        return APIResponse<List<FavouriteResponse>>.Ok(QuotateBiz.Favourite(token.member_fk, req.market));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][Favourite]" + ex.Message);
        return APIResponse<List<FavouriteResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("favoriteadd")]
    public APIResponse FavoriteAdd(FavoriteAddRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        QuotateBiz.FavoriteAdd(token.member_fk, req.market, req.code);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][FavoriteAdd]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("favoritedelete")]
    public APIResponse FavoriteDelete(FavoriteDeleteRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        QuotateBiz.FavoriteDelete(token.member_fk, req.market, req.code);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][FavoriteDelete]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("defaultstock")]
    public APIResponse<DefaultStockResponse> DefaultStock(DefaultStockRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.market.IsEmpty())
          throw new AppException(1570, "market_not_exist");
        int member_id = -1;
        if (token != null)
          member_id = token.member_fk;
        return APIResponse<DefaultStockResponse>.Ok(QuotateBiz.DefaultStock(member_id, req.market));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[QuotateController][DefaultStock]" + ex.Message);
        return APIResponse<DefaultStockResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
