// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.SysController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Config;
using tradeapi.Models.Member;
using tradeapi.Models.Name;
using tradeapi.Models.Sys;
using tradeapi.Utility;
using tradeapi.Validates;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SysController : ApiController
  {
    [HttpGet("timeprotocol")]
    public APIResponse<long> TimeProtocol()
    {
      return APIResponse<long>.Ok(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    private string GetIP()
    {
      try
      {
        return this.Request.Headers.Keys.Contains("X-Forwarded-For") ? this.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0] : this.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
      }
      catch (Exception ex)
      {
        LogLib.Warn("[SysController][GetIP]" + ex.Message);
        return "";
      }
    }

    [HttpPost("getname")]
    public APIResponse<NameResponse> GetName(ConfigRequest req)
    {
      try
      {
        string str = "";
        if (this.Request.Headers.Keys.Contains("CF-IPCountry"))
          str = this.Request.Headers["CF-IPCountry"].ToString();
        SysBiz.CheckMerchantKey(req.appkey);
        return APIResponse<NameResponse>.Ok(SysBiz.GetName(this.GetIP()));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SysController][GetName]" + ex.Message);
        return APIResponse<NameResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("config")]
    public APIResponse<string> Config(ConfigRequest req)
    {
      try
      {
        string country = "";
        if (this.Request.Headers.Keys.Contains("CF-IPCountry"))
          country = this.Request.Headers["CF-IPCountry"].ToString();
        string input = JsonSerializer.Serialize<ConfigResponse>(SysBiz.GetConfig(SysBiz.CheckMerchantKey(req.appkey), this.GetIP(), country));
        LogLib.Debug("[SysController][Config]" + input);
        return APIResponse<string>.Ok(DecryptTool.EncryptByAES(input));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SysController][Config]" + ex.Message);
        return APIResponse<string>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("applogo")]
    public APIResponse<AppLogoResponse> AppLogo(AppLogoRequest req)
    {
      try
      {
        string str = "";
        if (this.Request.Headers.Keys.Contains("CF-IPCountry"))
          str = this.Request.Headers["CF-IPCountry"].ToString();
        SysBiz.CheckMerchantKey(req.appkey);
        return APIResponse<AppLogoResponse>.Ok(SysBiz.GetAppLogo());
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SysController][AppLogo]" + ex.Message);
        return APIResponse<AppLogoResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("apptheme")]
    public APIResponse<AppThemeResponse> AppTheme(AppThemeRequest req)
    {
      try
      {
        string str = "";
        if (this.Request.Headers.Keys.Contains("CF-IPCountry"))
          str = this.Request.Headers["CF-IPCountry"].ToString();
        SysBiz.CheckMerchantKey(req.appkey);
        return APIResponse<AppThemeResponse>.Ok(SysBiz.GetAppTheme());
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SysController][AppTheme]" + ex.Message);
        return APIResponse<AppThemeResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("signin")]
    public APIResponse<SignInResponse> signin(ReqString req_str)
    {
      try
      {
        SysSignInRequest instance = JsonSerializer.Deserialize<SysSignInRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (instance != null && !string.IsNullOrEmpty(instance.lang))
          this.lang = instance.lang;
        new SysSigninValidator().ValidateAndThrow<SysSignInRequest>(instance);
        int num = -1;
        if (instance.nonce == "QGj5454gd2dcngh8")
          num = 9;
        else if (instance.nonce == "QGj5454gd2dcngh3")
          num = 3;
        else if (instance.nonce == "QGj5454gd2dcngh5")
          num = 8;
        return APIResponse<SignInResponse>.Ok(AuthBiz.Login(new TokenModel()
        {
          member_fk = num,
          ip = this.GetIP()
        }), "登录成功");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SysController][signin]" + ex.Message);
        return APIResponse<SignInResponse>.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }
  }
}
