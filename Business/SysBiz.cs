// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.SysBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System.Collections.Generic;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Config;
using tradeapi.Models.Member;
using tradeapi.Models.Name;
using tradeapi.Models.Sys;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class SysBiz
  {
    public static string CheckMerchantKey(string key)
    {
      if (key == "testVNkey")
        return "id12345";
      throw new AppException(310, "biz_number_error");
    }

    public static ConfigResponse GetConfig(string id, string ip, string country)
    {
      List<string> stringList = new List<string>()
      {
        "VN",
        "US"
      };
      LogLib.Log("[GetConfig] IP: " + ip);
      return new ConfigResponse()
      {
        apiserver = ConfigLib.Get("apiserver"),
        fileserver = ConfigLib.Get("fileserver"),
        filesite = ConfigLib.Get("filesite"),
        defaul_lan = ConfigLib.Get("app_default_lang"),
        date_format = ConfigLib.Get("date_format"),
        wallet_currency = ConfigLib.Get("wallet_currency"),
        app_download_url = ConfigLib.Get("app_download_url"),
        app_test_url = "https://ap33.f519.com",
        ip_country = country == "" ? Tool.GetCountryByIp(ip) : country,
        show_lang_menu = ConfigLib.Get("show_lang_menu") == "true",
        web_site_url = ConfigLib.Get("web_site_url"),
        markets = stringList,
        management_fee_enable_coupon_use = Tool.ToBool(ConfigLib.Get("management_fee_enable_coupon_use"))
      };
    }

    public static NameResponse GetName(string ip)
    {
      LogLib.Log("[GetName] IP: " + ip);
      return new NameResponse()
      {
        name = ConfigLib.Get("operator_name")
      };
    }

    public static AppLogoResponse GetAppLogo()
    {
      List<AppLogoDto> appLogoList = AppLogoService.FindAppLogoList();
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = "";
      string str6 = "";
      string str7 = "";
      if (appLogoList != null)
      {
        foreach (AppLogoDto appLogoDto in appLogoList)
        {
          if (appLogoDto.type == 1)
            str1 = appLogoDto.url;
          else if (appLogoDto.type == 2)
            str2 = appLogoDto.url;
          else if (appLogoDto.type == 3)
            str3 = appLogoDto.url;
          else if (appLogoDto.type == 4)
            str4 = appLogoDto.url;
          else if (appLogoDto.type == 5)
            str5 = appLogoDto.url;
          else if (appLogoDto.type == 6)
            str6 = appLogoDto.url;
          else if (appLogoDto.type == 7)
            str7 = appLogoDto.url;
        }
      }
      return new AppLogoResponse()
      {
        icon_login_logo = str1,
        dwapp_logo = str2,
        dwap_bg = str3,
        logo = str4,
        bg_popularize = str5,
        dw_ios = str6,
        dw_android = str7
      };
    }

    public static AppThemeResponse GetAppTheme()
    {
      return new AppThemeResponse()
      {
        color = ConfigLib.Get("app_theme")
      };
    }

    public static SignInResponse SignIn(SysSignInRequest req)
    {
      return new SignInResponse()
      {
        Token = "TEST_XXXX",
        SubAccount = "",
        Status = 0
      };
    }
  }
}
