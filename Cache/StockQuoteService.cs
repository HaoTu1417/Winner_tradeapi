// Decompiled with JetBrains decompiler
// Type: tradeapi.Cache.StockQuoteService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using tradeapi.Models;

#nullable enable
namespace tradeapi.Cache
{
  public class StockQuoteService
  {
    private static readonly TimeSpan _expire_time = new TimeSpan(3, 0, 0, 0);
    private static readonly Dictionary<string, Dictionary<string, StockQuote>> _quotes = new Dictionary<string, Dictionary<string, StockQuote>>();

    public static StockQuote GetStockQuote(string market, string stock_code)
    {
      if (!StockQuoteService._quotes.ContainsKey(market))
      {
        lock (StockQuoteService._quotes)
          StockQuoteService._quotes[market] = new Dictionary<string, StockQuote>();
      }
      StockQuote stockQuote;
      if (!StockQuoteService._quotes[market].TryGetValue(stock_code, out stockQuote))
      {
        IDatabase database = StockQuoteService.GetDatabase(market);
        if (!database.KeyExists((RedisKey) stock_code))
          database.HashSet((RedisKey) stock_code, new HashEntry[17]
          {
            new HashEntry((RedisValue) "stock_name", (RedisValue) ""),
            new HashEntry((RedisValue) "exchange", (RedisValue) market),
            new HashEntry((RedisValue) "update_time", (RedisValue) DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
            new HashEntry((RedisValue) "prev_day_c", (RedisValue) 0),
            new HashEntry((RedisValue) "prev_day_v", (RedisValue) 0),
            new HashEntry((RedisValue) "day_o", (RedisValue) 0),
            new HashEntry((RedisValue) "day_h", (RedisValue) 0),
            new HashEntry((RedisValue) "day_l", (RedisValue) 0),
            new HashEntry((RedisValue) "day_c", (RedisValue) 0),
            new HashEntry((RedisValue) "day_v", (RedisValue) 0),
            new HashEntry((RedisValue) "price", (RedisValue) 0),
            new HashEntry((RedisValue) "bids", (RedisValue) "[]"),
            new HashEntry((RedisValue) "asks", (RedisValue) "[]"),
            new HashEntry((RedisValue) "bid_sizes", (RedisValue) "[]"),
            new HashEntry((RedisValue) "ask_sizes", (RedisValue) "[]"),
            new HashEntry((RedisValue) "ceiling", (RedisValue) 0),
            new HashEntry((RedisValue) "floor", (RedisValue) 0)
          });
        stockQuote = new StockQuote(database, stock_code);
        lock (StockQuoteService._quotes)
          StockQuoteService._quotes[market][stock_code] = stockQuote;
      }
      return stockQuote;
    }

    public static bool IsExists(string market, string stock_code)
    {
      return StockQuoteService.GetDatabase(market).KeyExists((RedisKey) stock_code);
    }

    private static IDatabase GetDatabase(string market) => StockRedisService.GetDatabase(market);
  }
}
