// Decompiled with JetBrains decompiler
// Type: tradeapi.Cache.RedisService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

#nullable enable
namespace tradeapi.Cache
{
    public class RedisService
    {
        private static ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect(new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build()["RedisCacheUrl"]);

        public static IDatabase GetDatabase(int dbNum = 0)
        {
            return RedisService._redis.GetDatabase(dbNum, (object) null);
        }

        public static IDatabase GetDatabase(string market)
        {
            int num;
            switch (market)
            {
                case "US":
                    num = 2;
                    break;
                case "VN":
                    num = 5;
                    break;
                default:
                    throw new Exception("Wrong Market: " + market);
            }
            int db = num;
            return RedisService._redis.GetDatabase(db, (object) null);
        }
    }
}