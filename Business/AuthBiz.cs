// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.AuthBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Models.Dto;
using System;
using System.Linq;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi.Models.Account;
using tradeapi.Models.Member;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Business
{
  public class AuthBiz
  {
    private static string GenNewToken() => MD5Service.Addmd5(Guid.NewGuid().ToString());

    private static string GetToken(ControllerBase controller)
    {
      StringValues source;
      if (controller.Request.Headers.TryGetValue("token", out source))
        return source.FirstOrDefault<string>();
      throw new AppException(1250, "token_invitation");
    }

    public static string CheckToken(ControllerBase controller)
    {
      string token = AuthBiz.GetToken(controller);
      return TokenCatch.IsTokenExist(token) ? token : throw new AppException(1210, "[1210] 用户没有登录");
    }

    public static TokenModel GetTokenInfo(string token) => TokenCatch.GetToken(token);

    public static SignInResponse Login(TokenModel tokenModel)
    {
      string token1 = MemberServices.GetToken(tokenModel.member_fk);
      if (!string.IsNullOrEmpty(token1))
        TokenCatch.RemoveToken(token1);
      SimpleAccountResponse defaultAccount = new AccountServices().FindDefaultAccount(tokenModel.member_fk);
      if (defaultAccount != null)
        tokenModel.sub_account = defaultAccount.sub_account;
      if (defaultAccount != null)
        tokenModel.status = defaultAccount.status;
      if (defaultAccount != null)
        tokenModel.market = defaultAccount.market;
      string token2 = AuthBiz.GenNewToken();
      TokenCatch.SetToken(token2, tokenModel);
      MemberServices.SetToken(tokenModel.member_fk, token2, tokenModel.ip);
      MemberDto member = MemberServices.GetMember(tokenModel.member_fk);
      return new SignInResponse()
      {
        Token = token2,
        SubAccount = tokenModel.sub_account,
        Status = tokenModel.status,
        Market = tokenModel.market,
        is_test_account = member.is_test_account,
        lang = member.lang
      };
    }

    public static void Logout(TokenModel token_model)
    {
      string token = MemberServices.GetToken(token_model.member_fk);
      TokenCatch.RemoveToken(token);
      MemberServices.ClearToken(token);
    }

    public static SignInResponse SwitchSubAccount(TokenModel newtoken)
    {
      string token = MemberServices.GetToken(newtoken.member_fk);
      TokenCatch.SetToken(token, newtoken);
      new AccountServices().SetDefaultAccount(newtoken.member_fk, newtoken.sub_account);
      return new SignInResponse()
      {
        Token = token,
        SubAccount = newtoken.sub_account,
        Status = newtoken.status
      };
    }
  }
}
