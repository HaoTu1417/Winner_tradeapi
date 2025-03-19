// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.RedisEntity
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using tradeapi.Common;

#nullable enable
namespace tradeapi.Utility
{
    public class RedisEntity
    {
        private static IConfiguration _config = (IConfiguration) JsonConfigurationExtensions.AddJsonFile((IConfigurationBuilder) new ConfigurationBuilder(), "appsettings.json").Build();
        public static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>((Func<ConnectionMultiplexer>) (() =>
        {
            try
            {
                return ConnectionMultiplexer.Connect(RedisEntity._config["RedisCacheUrl"]);
            }
            catch (RedisConnectionException ex)
            {
                throw new AppException(1020, "redis_exception");
            }
        }));

        public static IDatabase SelectDb(int dbNum = 0)
        {
            return RedisEntity.lazyConnection.Value.GetDatabase(dbNum, (object) null);
        }
    }
}