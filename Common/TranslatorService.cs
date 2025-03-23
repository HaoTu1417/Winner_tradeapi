// Decompiled with JetBrains decompiler
// Type: tradeapi.Common.TranslatorService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using tradeapi.Cache;

#nullable enable
namespace tradeapi.Common
{
    public class TranslatorService
    {
        public static string ConvertByKey(string lang, string key)
        {
            if (key == "redis_exception")
                return "redis connection failed";
            CacheQuery.SelectDB(CacheEnum.language);
            lang = lang == null ? "" : lang.ToLower();
            string redisKey = lang + "_error";
            return !CacheQuery.HashExists(redisKey, key) ? key + "(no translation found)" : (string) CacheQuery.HashGet(redisKey, key);
        }
    }
}