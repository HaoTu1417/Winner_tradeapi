// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.LanguageLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Collections.Generic;
using tradeapi.Cache;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Libs
{
    public class LanguageLib
    {
        public static Dictionary<string, string> GetViewTranslate(string lang, string page)
        {
            lang = lang.ToLower();
            page = page.ToLower();
            return Tool.FromJson<Dictionary<string, string>>("{\n\t\t\t        \"login_title\" : \"Backend System\",\n\t\t\t        \"hint_account\" : \"input your account\",\n\t\t\t        \"hint_password\" : \"input your password\"\n\t\t        }");
        }

        public static string GetErrorTranslate(string lang, string key)
        {
            if (key == "redis_exception")
                return "翻譯檔不存在";
            lang = lang.ToLower();
            CacheQuery.SelectDB(1);
            string redisKey = lang + "_error";
            return !CacheQuery.HashExists(redisKey, key) ? key + "(找不到翻译档)" : (string) CacheQuery.HashGet(redisKey, key);
        }
    }
}