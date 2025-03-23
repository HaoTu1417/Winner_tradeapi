// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.ThirdPayController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.ThirdPay;
using tradeapi.Validates;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ThirdPayController : ApiController
  {
    private readonly IConfiguration _configuration;

    public ThirdPayController(IConfiguration config) => this._configuration = config;

    [HttpPost("thirdpaylist")]
    public APIResponse<ThirdPayListResponse> ThirdPayList(LangRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<ThirdPayListResponse>.Ok(ThirdPayBiz.GetThirdPaySupportList());
      }
      catch (AppException ex)
      {
        LogLib.Warn("[ThirdPayController][ThirdPayList]" + ex.Message);
        return APIResponse<ThirdPayListResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("partyecharge")]
    public async Task<APIResponse<ThirdPayRechargeResponse>> PartyRecharge(
      ThirdPayRechargeRequest req)
    {
      ThirdPayController thirdPayController = this;
      TokenModel token = thirdPayController.GetToken();
      try
      {
        new ThirdPayRechargeValidator().ValidateAndThrow<ThirdPayRechargeRequest>(req);
        LogLib.Log("[ThirdPayController][partyecharge]" + JsonConvert.SerializeObject((object) req));
        return APIResponse<ThirdPayRechargeResponse>.Ok(await WalletBiz.ThirtyPay(token.member_fk, req, thirdPayController.GetIp(), thirdPayController._configuration));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[ThirdPayController][PartyRecharge]" + ex.Message);
        return APIResponse<ThirdPayRechargeResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("rechargenotify")]
    public string RechargeNotify([FromForm] RechargeNotifyRequest req)
    {
      try
      {
        LogLib.Log("[ThirdPayController][rechargenotify]" + JsonConvert.SerializeObject((object) req));
        ThirdPayBiz.RechargeNotify(req);
        return "SUCCESS";
      }
      catch (AppException ex)
      {
        LogLib.Warn("[ThirdPayController][RechargeNotify]" + ex.Message);
        return "FAIL";
      }
    }

    [HttpPost("paystatus")]
    public async Task<APIResponse<int>> PayStatus(PayStatusRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<int>.Ok(ThirdPayBiz.PaySuccess(req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[ThirdPayController][PayStatus]" + ex.Message);
        return APIResponse<int>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("genrsakey")]
    public async Task<APIResponse<string[]>> GenRsaKey()
    {
      return APIResponse<string[]>.Ok(WalletBiz.GetRsaKey());
    }
  }
}
