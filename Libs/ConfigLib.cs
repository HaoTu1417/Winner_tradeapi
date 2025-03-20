// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.ConfigLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Models.Dto;
using System;
using System.Collections.Generic;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Libs
{
    public class ConfigLib
    {
        private static string ConfigCacheTable = "AdminConfig";

        public static string Get(string key)
        {
            try
            {
                CacheQuery.SelectDB(CacheEnum.admin);
                if (!CacheQuery.KeyExists(ConfigLib.ConfigCacheTable))
                {
                    List<AdminConfigDto> all = AdminConfigService.FindAll();
                    if (all.Count <= 0)
                        throw new AppException(790, "undefine_redis_key");
                    foreach (AdminConfigDto adminConfigDto in all)
                        CacheQuery.HashSet(ConfigLib.ConfigCacheTable, adminConfigDto.name, adminConfigDto.value);
                }
                if (!CacheQuery.HashExists(ConfigLib.ConfigCacheTable, key))
                {
                    AdminConfigDto adminConfigDto = AdminConfigService.Find(key);
                    if (adminConfigDto == null)
                        throw new AppException(790, "undefine_redis_key");
                    CacheQuery.HashSet(ConfigLib.ConfigCacheTable, adminConfigDto.name, adminConfigDto.value);
                }
            }
            catch (Exception ex)
            {
                LogLib.Log("[ConfigLib][Get]=>" + key.ToString() + ", " + ex.Message);
                throw new AppException(800, "illegal_read_redis_key");
            }
            return (string) CacheQuery.HashGet(ConfigLib.ConfigCacheTable, key);
        }
    }
}