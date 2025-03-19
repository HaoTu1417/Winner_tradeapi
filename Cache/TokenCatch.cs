// Decompiled with JetBrains decompiler
// Type: tradeapi.Cache.TokenCatch
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using System;
using tradeapi.Models;

#nullable enable
namespace tradeapi.Cache
{
    public class TokenCatch
    {
        public static TokenModel? GetToken(string key)
        {
            CacheQuery.SelectDB(3);
            return CacheQuery.StringGet<TokenModel>(key);
        }

        public static void SetToken(string token, TokenModel data)
        {
            CacheQuery.SelectDB(3);
            CacheQuery.StringSet<TokenModel>(token, data, new TimeSpan?(TimeSpan.FromHours(12.0)));
        }

        public static bool IsTokenExist(string token)
        {
            CacheQuery.SelectDB(3);
            return CacheQuery.KeyExpire(token, new TimeSpan?(TimeSpan.FromHours(12.0)));
        }

        public static void RemoveToken(string token)
        {
            CacheQuery.SelectDB(3);
            CacheQuery.KeyDelete(token);
        }
    }
}