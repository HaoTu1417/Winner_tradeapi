// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.QuotateBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.Quotate;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class QuotateBiz
  {
    public static string GetDefaultStock(string market, string stock_code)
    {
      if (stock_code.IsEmpty())
      {
        stock_code = SysMarketService.FindAll().FirstOrDefault<SysMarketDto>((Func<SysMarketDto, bool>) (x => x.code.Equals(market, StringComparison.CurrentCultureIgnoreCase)))?.default_stock_code ?? "";
        if (stock_code.IsEmpty())
          throw new AppException(1570, "market_not_exist");
      }
      return stock_code;
    }

    public static List<MarketResponse> GetOpenMarket(string lang)
    {
      List<MarketResponse> openMarket = new List<MarketResponse>();
      List<SysMarketDto> openMarkets = SysMarketService.GetOpenMarkets(lang);
      if (openMarkets != null)
      {
        foreach (SysMarketDto sysMarketDto in openMarkets)
        {
          MarketResponse marketResponse = new MarketResponse()
          {
            exchanges = ((IEnumerable<string>) sysMarketDto.exchange.Split("\r\n")).AsList<string>(),
            market = sysMarketDto.code,
            label = sysMarketDto.label
          };
          openMarket.Add(marketResponse);
        }
      }
      return openMarket;
    }

    public static List<MarketIndexResponse> GetMarket(string market)
    {
      try
      {
        string key = "@market_indices";
        return Tool.FromJson<List<MarketIndexResponse>>(StockRedisService.GetDatabase(market).StringGet((RedisKey) key).ToString());
      }
      catch
      {
        return new List<MarketIndexResponse>();
      }
    }

    public static List<RankResponse> GetRankIncrease(
      int member_fk,
      string market,
      string? exchange,
      int count)
    {
      HashSet<string> favorites = StockFavoriteService.FindFavourite(member_fk, market).ToHashSet<string>();
      return QuotateBiz.GetRankingListByPercentageChange(market, exchange, 1, count).Select<RankResponse, RankResponse>((Func<RankResponse, RankResponse>) (x =>
      {
        x.is_favorite = favorites.Contains(x.code);
        return x;
      })).ToList<RankResponse>();
    }

    public static List<RankResponse> GetRankDecline(
      int member_fk,
      string market,
      string? exchange,
      int count)
    {
      HashSet<string> favorites = StockFavoriteService.FindFavourite(member_fk, market).ToHashSet<string>();
      return QuotateBiz.GetRankingListByPercentageChange(market, exchange, 0, count).Select<RankResponse, RankResponse>((Func<RankResponse, RankResponse>) (x =>
      {
        x.is_favorite = favorites.Contains(x.code);
        return x;
      })).ToList<RankResponse>();
    }

    public static List<RankVolumeResponse> GetRankVolume(
      int member_fk,
      string market,
      string? exchange,
      int count)
    {
      HashSet<string> favorites = StockFavoriteService.FindFavourite(member_fk, market).ToHashSet<string>();
      return QuotateBiz.GetRankingListByVolume(market, exchange, count).Select<RankVolumeResponse, RankVolumeResponse>((Func<RankVolumeResponse, RankVolumeResponse>) (x =>
      {
        x.is_favorite = favorites.Contains(x.code);
        return x;
      })).ToList<RankVolumeResponse>();
    }

    public static LastPriceResponse GetLastPrice(string market, string code, string lang)
    {
      lang = lang == null ? ConfigLib.Get("app_default_lang") : lang.ToUpper();
      List<MutilangTableDto> all = MutilangTableService.FindAll();
      MutilangTableDto mutilangTableDto1 = all.Where<MutilangTableDto>((Func<MutilangTableDto, bool>) (x => x.dbtable.Equals("quotate") && x.field.Equals("bid") && x.lang.Equals(lang))).FirstOrDefault<MutilangTableDto>();
      string bid_name = mutilangTableDto1 == null ? "Bid" : mutilangTableDto1.value;
      MutilangTableDto mutilangTableDto2 = all.Where<MutilangTableDto>((Func<MutilangTableDto, bool>) (x => x.dbtable.Equals("quotate") && x.field.Equals("ask") && x.lang.Equals(lang))).FirstOrDefault<MutilangTableDto>();
      string ask_name = mutilangTableDto2 == null ? "Ask" : mutilangTableDto2.value;
      code = QuotateBiz.GetDefaultStock(market, code);
      StockQuote quote = StockQuoteService.GetStockQuote(market, code);
      if (quote == null)
        return new LastPriceResponse();
      return new LastPriceResponse()
      {
        code = quote.stock_code,
        name = quote.stock_name,
        prev_close = quote.prev_day_c,
        open = quote.day_o,
        high = quote.day_h,
        low = quote.day_l,
        price = quote.price,
        price_change = quote.prev_day_c != 0M ? Math.Round(quote.price - quote.prev_day_c, 2) : 0M,
        percentage_change = quote.prev_day_c != 0M ? Math.Round((quote.price - quote.prev_day_c) * 100M / quote.prev_day_c, 2) : 0M,
        volume = quote.day_v,
        bid = quote.bids,
        bid_size = quote.bid_sizes,
        ask = quote.asks,
        ask_size = quote.ask_sizes,
        bids = quote.bids.Length != 0 ? ((IEnumerable<Decimal>) quote.bids).Select<Decimal, Dictionary<string, string>>((Func<Decimal, int, Dictionary<string, string>>) ((x, i) =>
        {
          Dictionary<string, string> lastPrice = new Dictionary<string, string>();
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(bid_name);
          interpolatedStringHandler.AppendFormatted<int>(i + 1);
          lastPrice.Add("name", interpolatedStringHandler.ToStringAndClear());
          lastPrice.Add("price", x.ToString());
          lastPrice.Add("volume", quote.bid_sizes[i].ToString());
          return lastPrice;
        })).ToList<Dictionary<string, string>>() : new List<Dictionary<string, string>>(),
        asks = quote.asks.Length != 0 ? ((IEnumerable<Decimal>) quote.asks).Select<Decimal, Dictionary<string, string>>((Func<Decimal, int, Dictionary<string, string>>) ((x, i) =>
        {
          Dictionary<string, string> lastPrice = new Dictionary<string, string>();
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(ask_name);
          interpolatedStringHandler.AppendFormatted<int>(i + 1);
          lastPrice.Add("name", interpolatedStringHandler.ToStringAndClear());
          lastPrice.Add("price", x.ToString());
          lastPrice.Add("volume", quote.ask_sizes[i].ToString());
          return lastPrice;
        })).ToList<Dictionary<string, string>>() : new List<Dictionary<string, string>>(),
        ceiling = quote.ceiling,
        floor = quote.floor
      };
    }

    public static List<LikeCodeResponse> GetLikeCode(string market, string keyword)
    {
      return StockServices.LikeSearch(market, keyword).Select<StockDto, LikeCodeResponse>((Func<StockDto, LikeCodeResponse>) (x => new LikeCodeResponse()
      {
        market = market,
        code = x.stock_code,
        name = x.stock_name
      })).ToList<LikeCodeResponse>();
    }

    public static List<FilterStockResponse> FilterStock(string market, string first_code)
    {
      return StockServices.Filter(market, first_code).Select<StockDto, FilterStockResponse>((Func<StockDto, FilterStockResponse>) (x => new FilterStockResponse()
      {
        market = market,
        code = x.stock_code,
        name = x.stock_name
      })).ToList<FilterStockResponse>();
    }

    public static List<FavouriteResponse> Favourite(int member_fk, string market)
    {
      return StockFavoriteService.FindFavourite(member_fk, market).Select<string, FavouriteResponse>((Func<string, FavouriteResponse>) (code =>
      {
        StockQuote stockQuote = StockQuoteService.GetStockQuote(market, code);
        return new FavouriteResponse()
        {
          market = market,
          code = code,
          name = stockQuote.stock_name,
          price = stockQuote.price,
          price_change = stockQuote.price - stockQuote.prev_day_c,
          percentage_change = stockQuote.prev_day_c != 0M ? Math.Round((stockQuote.price - stockQuote.prev_day_c) * 100M / stockQuote.prev_day_c, 2) : 0M,
          volume = stockQuote.day_v
        };
      })).ToList<FavouriteResponse>();
    }

    public static void FavoriteAdd(int member_fk, string market, string code)
    {
      if (StockFavoriteService.IsExists(member_fk, market, code))
        return;
      StockFavoriteService.Insert(new StockFavoriteDto()
      {
        member_fk = member_fk,
        market = market,
        stock_code = code
      });
    }

    public static void FavoriteDelete(int member_fk, string market, string code)
    {
      if (!StockFavoriteService.IsExists(member_fk, market, code))
        return;
      StockFavoriteService.Delete(member_fk, market, code);
    }

    public static DefaultStockResponse DefaultStock(int member_id, string market)
    {
      SysMarketDto sysMarketDto = SysMarketService.Find(market);
      StockQuote stockQuote = StockQuoteService.GetStockQuote(market, sysMarketDto.default_stock_code);
      if (stockQuote == null)
        return new DefaultStockResponse();
      return new DefaultStockResponse()
      {
        market = market,
        code = stockQuote.stock_code,
        name = stockQuote.stock_name,
        price = stockQuote.price,
        price_change = stockQuote.price - stockQuote.prev_day_c,
        percentage_change = stockQuote.prev_day_c == 0M ? 0M : (stockQuote.price - stockQuote.prev_day_c) / stockQuote.prev_day_c,
        volume = stockQuote.day_v,
        is_favourite = StockFavoriteService.IsExists(member_id, market, stockQuote.stock_code)
      };
    }

    private static List<RankResponse> GetRankingListByPercentageChange(
      string market,
      string? exchange,
      int order,
      int count)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 3);
      interpolatedStringHandler.AppendLiteral("@prank_");
      interpolatedStringHandler.AppendFormatted(market);
      interpolatedStringHandler.AppendLiteral("_");
      interpolatedStringHandler.AppendFormatted(exchange ?? "");
      interpolatedStringHandler.AppendLiteral("_");
      interpolatedStringHandler.AppendFormatted<int>(order);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      return Tool.FromJson<List<RankResponse>>(RedisService.GetDatabase(market).StringGet((RedisKey) stringAndClear).ToString());
    }

    private static List<RankVolumeResponse> GetRankingListByVolume(
      string market,
      string? exchange,
      int count)
    {
      string key = "@vrank_" + market + "_" + exchange;
      return Tool.FromJson<List<RankVolumeResponse>>(RedisService.GetDatabase(market).StringGet((RedisKey) key).ToString());
    }
  }
}
