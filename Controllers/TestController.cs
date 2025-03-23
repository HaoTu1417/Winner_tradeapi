// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.TestController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using System.Collections.Generic;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;

#nullable enable
namespace tradeapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ApiController
    {
        [HttpGet("stockquotes")]
        public APIResponse<string> StockSuotes()
        {
            try
            {
                CacheQuery.SelectDB(5);
                Dictionary<string, Quote> dictionary = CacheQuery.StringGet<Dictionary<string, Quote>>("@stock_quotes");
                return dictionary != null ? APIResponse<string>.Ok(dictionary.Json()) : APIResponse<string>.Ok("@stock_quotes is null!");
            }
            catch (AppException ex)
            {
                LogLib.Warn("[TestController][StockSuotes]" + ex.Message);
                return APIResponse<string>.Error(ex.GetStatus(), ex.ToString());
            }
        }

        [HttpGet("stockinfo")]
        public APIResponse<string> StockInfo()
        {
            try
            {
                CacheQuery.SelectDB(5);
                Dictionary<string, us_stockupdate.Models.ViewModels.StockInfo> dictionary = CacheQuery.StringGet<Dictionary<string, us_stockupdate.Models.ViewModels.StockInfo>>("@stock_info");
                return dictionary != null ? APIResponse<string>.Ok(dictionary.Json()) : APIResponse<string>.Ok("@stock_info is null!");
            }
            catch (AppException ex)
            {
                LogLib.Warn("[TestController][StockInfo]" + ex.Message);
                return APIResponse<string>.Error(ex.GetStatus(), ex.ToString());
            }
        }
    }
}