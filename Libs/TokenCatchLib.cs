// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.TokenCatchLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using tradeapi.Cache;
using tradeapi.Models;

#nullable enable
namespace tradeapi.Libs
{
    public class TokenCatchLib
    {
        public static TokenModel? GetToken(string key)
        {
            CacheQuery.SelectDB(CacheEnum.token);
            return CacheQuery.StringGet<TokenModel>(key);
        }

        public static void SetToken(string token, TokenModel data)
        {
            CacheQuery.SelectDB(CacheEnum.token);
            CacheQuery.StringSet<TokenModel>(token, data, new TimeSpan?(TimeSpan.FromMinutes(30.0)));
        }

        public static bool IsTokenExist(string token)
        {
            CacheQuery.SelectDB(CacheEnum.token);
            return CacheQuery.KeyExpire(token, new TimeSpan?(TimeSpan.FromMinutes(30.0)));
        }

        public static void RemoveToken(string token)
        {
            CacheQuery.SelectDB(CacheEnum.token);
            CacheQuery.KeyDelete(token);
        }
    }
}