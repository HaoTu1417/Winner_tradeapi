// Decompiled with JetBrains decompiler
// Type: tradeapi.Middleware.ApiKeyMiddleware
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;
using tradeapi2.Common;

#nullable enable
namespace tradeapi2.Middleware
{
  public class ApiKeyMiddleware : IMiddleware
  {
    private const string APIKEY = "token";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      if (!this.IsExclude((string) context.Request.Path))
      {
        StringValues token;
        if (!context.Request.Headers.TryGetValue("token", out token))
        {
          await context.Response.WriteAsync(Tool.ToJson((object) APIResponse.Error(1260, TranslatorService.ConvertByKey(ConfigLib.Get("app_default_lang"), "token_not_exist"))));
          return;
        }
        if (!TokenCatch.IsTokenExist((string) token))
        {
          await context.Response.WriteAsync(Tool.ToJson((object) APIResponse.Error(1250, TranslatorService.ConvertByKey(ConfigLib.Get("app_default_lang"), "token_invitation"))));
          return;
        }
      }
      await next(context);
    }

    private bool IsExclude(string path)
    {
      return new List<string>()
      {
        "/test/stockquotes",
        "/test/stockinfo",
        "/sys/timeprotocol",
        "/sys/getname",
        "/sys/config",
        "/sys/applogo",
        "/sys/signin",
        "/sys/apptheme",
        "/member/verifymailcode",
        "/member/resetpwdverifymailcode",
        "/member/sendverifycode",
        "/member/register",
        "/member/verifyidentityfinish",
        "/member/signin",
        "/member/signin1",
        "/member/vertrfycode",
        "/member/gettask",
        "/member/passwordapply",
        "/member/sendsmsverify",
        "/member/verifysmscode",
        "/member/checkphoneauth",
        "/info/banner",
        "/info/promotionlist",
        "/info/promotioncontent",
        "/info/questioncatalog",
        "/info/questionlist",
        "/info/answer",
        "/info/exchange",
        "/info/bulletin",
        "/info/bulletincontent",
        "/info/doc",
        "/info/docbycid",
        "/info/service",
        "/info/servicequestion",
        "/info/serviceanswer",
        "/info/ad",
        "/info/advertise",
        "/indchart/history",
        "/indchart/today",
        "/quotate/filterstock",
        "/quotate/likecode",
        "/quotate/openmarket",
        "/quotate/market",
        "/quotate/rankincrease",
        "/quotate/rankdecline",
        "/quotate/rankvolume",
        "/quotate/lastprice",
        "/quotate/defaultstock",
        "/recommend/getrecommendedmembers",
        "/recommend/getrecommendregister",
        "/recommend/getallrecommendrewardinfo",
        "/recommend/getrecommendinfo",
        "/recommend/getrewarddetails",
        "/member/getinvitationinfo",
        "/system/getlanglist",
        "/system/setlang",
        "/system/countrylist",
        "/finance/getitems",
        "/wallet/wallet",
        "/thirdpay/rechargenotify",
        "/thirdpay/genrsakey",
        "/app/appdownloadurl",
        "/richbox/book",
        "/stock/info",
        "/stock/historydaily",
        "/stock/lastday",
        "/stock/tradinginfo",
        "/stock/quote",
        "/wallet/rechargeapplycallback",
        "/wallet/abc"
      }.Exists((Predicate<string>) (t => t == path.ToLower()));
    }
  }
}
