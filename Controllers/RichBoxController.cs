// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.RichBoxController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.RichBox;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class RichBoxController : ApiController
  {
    [HttpPost("book")]
    public APIResponse<BookResponse> Book(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<BookResponse>.Ok(token == null ? RichBoxBiz.GetBook() : RichBoxBiz.GetBook(token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RichBoxController][Book]" + ex.Message);
        return APIResponse<BookResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
      catch (Exception ex)
      {
        LogLib.Warn("[RichBoxController][Book]" + ex.Message);
        return APIResponse<BookResponse>.Error(-1, ex.Message);
      }
    }

    [HttpPost("recharge")]
    public APIResponse<BookResponse> Recharge(RechargeRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<BookResponse>.Ok(RichBoxBiz.Recharge(token.member_fk, req), RichBoxBiz.GetRechargeMessage(token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RichBoxController][Recharge]" + ex.Message);
        return APIResponse<BookResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
      catch (Exception ex)
      {
        LogLib.Warn("[RichBoxController][Recharge]" + ex.Message);
        return APIResponse<BookResponse>.Error(-1, ex.Message);
      }
    }

    [HttpPost("withdraw")]
    public APIResponse<BookResponse> Withdraw(RichWithdrawRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<BookResponse>.Ok(RichBoxBiz.Withdraw(token.member_fk, req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RichBoxController][Withdraw]" + ex.Message);
        return APIResponse<BookResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
      catch (Exception ex)
      {
        LogLib.Warn("[RichBoxController][Withdraw]" + ex.Message);
        return APIResponse<BookResponse>.Error(-1, ex.Message);
      }
    }

    [HttpPost("interest")]
    public APIResponse<BookResponse> Interest(InterestRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<BookResponse>.Ok(RichBoxBiz.Interest(token.member_fk, req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RichBoxController][Interest]" + ex.Message);
        return APIResponse<BookResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
      catch (Exception ex)
      {
        LogLib.Warn("[RichBoxController][Interest]" + ex.Message);
        return APIResponse<BookResponse>.Error(-1, ex.Message);
      }
    }

    [HttpPost("history")]
    public APIResponse<List<RichHistoryResponse>> History(RichHistoryRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<RichHistoryResponse>>.Ok(RichBoxBiz.GetHistory(token.member_fk, req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[RichBoxController][History]" + ex.Message);
        return APIResponse<List<RichHistoryResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
      catch (Exception ex)
      {
        LogLib.Warn("[RichBoxController][History]" + ex.Message);
        return APIResponse<List<RichHistoryResponse>>.Error(-1, ex.Message);
      }
    }
  }
}
