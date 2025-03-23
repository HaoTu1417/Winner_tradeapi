// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.WalletController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Wallet;
using tradeapi.Utility;
using tradeapi.Validates;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class WalletController : ApiController
  {
    [HttpPost("wallet")]
    public APIResponse<WalletResponse> Wallet(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (token != null)
          return APIResponse<WalletResponse>.Ok(WalletBiz.GetWalletById(token.member_fk));
        return APIResponse<WalletResponse>.Ok(new WalletResponse()
        {
          balance = 0M,
          coupon = 0M,
          freeze = 0M,
          available = 0M
        });
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][Wallet]" + ex.Message);
        return APIResponse<WalletResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("marketwallet")]
    public APIResponse<MarketWalletResponse> MarketWallet(MarketWalletRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<MarketWalletResponse>.Ok(WalletBiz.GetMarketWalletById(token.member_fk, req.market));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][MarketWallet]" + ex.Message);
        return APIResponse<MarketWalletResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getbankcard")]
    public APIResponse<string> GetBankCard(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        List<GetBankCardResponse> bankCard = WalletBiz.GetBankCard(token.member_fk);
        return bankCard == null ? APIResponse<string>.Ok("") : APIResponse<string>.Ok(DecryptTool.EncryptByAES(JsonSerializer.Serialize<List<GetBankCardResponse>>(bankCard)));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetBankCard]" + ex.Message);
        return APIResponse<string>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("supportcurrency")]
    public APIResponse<Dictionary<string, List<string>>> SupportCurrency(LangRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<Dictionary<string, List<string>>>.Ok(WalletBiz.GetSupportCurrency());
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][SupportCurrency]" + ex.Message);
        return APIResponse<Dictionary<string, List<string>>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getbankcardtype")]
    public APIResponse<List<GetBankCardTypeResponse>> GetBankCardType(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<GetBankCardTypeResponse>>.Ok(WalletBiz.GetBankCardType(req.lang, token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetBankCardType]" + ex.Message);
        return APIResponse<List<GetBankCardTypeResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("addbankcard")]
    public APIResponse AddBankCard(ReqString req_str)
    {
      try
      {
        TokenModel token = this.GetToken();
        AddBankCardRequest addBankCardRequest = JsonSerializer.Deserialize<AddBankCardRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (addBankCardRequest != null && !string.IsNullOrEmpty(addBankCardRequest.lang))
          this.lang = addBankCardRequest.lang;
        new AddBankCardValidator().ValidateAndThrow<AddBankCardRequest>(addBankCardRequest);
        WalletBiz.AddBankCard(addBankCardRequest, token.member_fk, this.GetIp());
        MemberTaskLib.MemberTaskFinish(token.member_fk, 7);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][AddBankCard]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("delbankcard")]
    public APIResponse Delbankcard(ReqString req_str)
    {
      try
      {
        this.GetToken();
        DelBankCardRequest instance = JsonSerializer.Deserialize<DelBankCardRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (instance != null && !string.IsNullOrEmpty(instance.lang))
          this.lang = instance.lang;
        new DelBankCardValidator().ValidateAndThrow<DelBankCardRequest>(instance);
        return APIResponse.Ok((object) MemberBankService.Remove(instance.card_pk), "rows_deleted");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][Delbankcard]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("getlastwithdraw")]
    public APIResponse<string> GetLastWithdraw(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        GetLastWithdrawResponse lastWithdrawById = WalletBiz.GetLastWithdrawById(token);
        return lastWithdrawById == null ? APIResponse<string>.Ok("") : APIResponse<string>.Ok(DecryptTool.EncryptByAES(JsonSerializer.Serialize<GetLastWithdrawResponse>(lastWithdrawById)));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetLastWithdraw]" + ex.Message);
        return APIResponse<string>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("withdrawinfo")]
    public APIResponse<List<WithdrawInfoResponse>> WithdrawInfo(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<WithdrawInfoResponse>>.Ok(WalletBiz.GetWithdrawInfo(token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][WithdrawInfo]" + ex.Message);
        return APIResponse<List<WithdrawInfoResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("withdrawapply")]
    public APIResponse<WithdrawApplyResponse> WithdrawApply(ReqString req_str)
    {
      try
      {
        TokenModel token = this.GetToken();
        WithdrawApplyRequest withdrawApplyRequest = JsonSerializer.Deserialize<WithdrawApplyRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (withdrawApplyRequest != null && !string.IsNullOrEmpty(withdrawApplyRequest.lang))
          this.lang = withdrawApplyRequest.lang;
        new WalletWithdrawValidator(token.member_fk).ValidateAndThrow<WithdrawApplyRequest>(withdrawApplyRequest);
        if (Tool.ToBool(ConfigLib.Get("enable_id_auth")) && MemberServices.Find(token.member_fk).id_auth != 1)
          throw new AppException(1303, "no_id_auth_yet");
        WithdrawApplyResponse withdrawApply = WalletBiz.GetWithdrawApply(withdrawApplyRequest, this.GetIp(), token.member_fk);
        WalletBiz.UpdateWithdrawApplyCount();
        WalletBiz.UpdateWithdrawNeedVerify();
        return APIResponse<WithdrawApplyResponse>.Ok(withdrawApply);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][WithdrawApply]" + ex.Message);
        return APIResponse<WithdrawApplyResponse>.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("withdrawcancel")]
    public APIResponse WithdrawCancel(WithdrawCancelRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        WalletBiz.CancelWithdrawById(token.member_fk, req);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][WithdrawCancel]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("depositapply")]
    public void DepositApply(ReqString req_str)
    {
      try
      {
        this.GetToken();
        WalletRecordRequest walletRecordRequest = JsonSerializer.Deserialize<WalletRecordRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (walletRecordRequest != null && !string.IsNullOrEmpty(walletRecordRequest.lang))
          this.lang = walletRecordRequest.lang;
        new DepositApplyValidator().ValidateAndThrow<WalletRecordRequest>(walletRecordRequest);
        WalletBiz.GetDepositApply(walletRecordRequest);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][DepositApply]" + ex.Message);
      }
    }

    [HttpPost("recordhistory")]
    public APIResponse<List<GetRecordHistoryResponse>> GetRecordHistory(LangRequest lang)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<GetRecordHistoryResponse>>.Ok(WalletBiz.GetRecordHistory(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetRecordHistory]" + ex.Message);
        return APIResponse<List<GetRecordHistoryResponse>>.Error(ex.GetStatus(), ex.GetMessage(lang.lang));
      }
    }

    [HttpPost("tradehistory")]
    public APIResponse<List<GetRecordHistoryResponse>> GetTradehistoryHistory(LangRequest lang)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<GetRecordHistoryResponse>>.Ok(WalletBiz.GetTradeHistory(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetTradehistoryHistory]" + ex.Message);
        return APIResponse<List<GetRecordHistoryResponse>>.Error(ex.GetStatus(), ex.GetMessage(lang.lang));
      }
    }

    [HttpPost("deposithistory")]
    public APIResponse<List<GetRecordHistoryResponse>> GetDepositHistory(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<GetRecordHistoryResponse>>.Ok(WalletBiz.GetDepositHistory(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetDepositHistory]" + ex.Message);
        return APIResponse<List<GetRecordHistoryResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("withdrawhistory")]
    public APIResponse<List<WithdrawHistoryResponse>> GetWithdrawHistory(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<WithdrawHistoryResponse>>.Ok(WalletBiz.GetWithdrawHistory(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetWithdrawHistory]" + ex.Message);
        return APIResponse<List<WithdrawHistoryResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("rechargehistory")]
    public APIResponse<List<RechargeHistoryResponse>> GetRechargeHistory(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<RechargeHistoryResponse>>.Ok(WalletBiz.GetRechargeHistory(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetRechargeHistory]" + ex.Message);
        return APIResponse<List<RechargeHistoryResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("freezehistory")]
    public APIResponse<List<FreezehistoryResponse>> FreezeHistory(LangRequest lang)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<FreezehistoryResponse>>.Ok(WalletBiz.GetFreezeHistory(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][FreezeHistory]" + ex.Message);
        return APIResponse<List<FreezehistoryResponse>>.Error(ex.GetStatus(), ex.GetMessage(lang.lang));
      }
    }

    [HttpPost("couponrecord")]
    public APIResponse<List<CouponrecordResponse>> Couponrecord(LangRequest lang)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<CouponrecordResponse>>.Ok(WalletBiz.GetCouponrecord(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][Couponrecord]" + ex.Message);
        return APIResponse<List<CouponrecordResponse>>.Error(ex.GetStatus(), ex.GetMessage(lang.lang));
      }
    }

    [HttpPost("rechargeapply")]
    public APIResponse<RechargeapplyResponse> Rechargeapply(ReqString req_str)
    {
      try
      {
        TokenModel token = this.GetToken();
        RechargeapplyRequest rechargeapplyRequest = JsonSerializer.Deserialize<RechargeapplyRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (rechargeapplyRequest != null && !string.IsNullOrEmpty(rechargeapplyRequest.lang))
          this.lang = rechargeapplyRequest.lang;
        if (MemberServices.Find(token.member_fk).id_auth != 1 && Tool.ToBool(ConfigLib.Get("enable_id_auth")))
          throw new AppException(1303, "no_id_auth_yet");
        new RechargeApplyValidator().ValidateAndThrow<RechargeapplyRequest>(rechargeapplyRequest);
        string str = WalletBiz.RechargeApply(token, rechargeapplyRequest, this.GetIp());
        WalletBiz.UpdateRechargeApplyCount();
        WalletBiz.UpdateRechargeNeedVerify();
        return APIResponse<RechargeapplyResponse>.Ok(new RechargeapplyResponse()
        {
          success = str != "",
          order_no = str
        });
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][Rechargeapply]" + ex.Message);
        return APIResponse<RechargeapplyResponse>.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("getadmincardtype")]
    public APIResponse<List<GetBankCardTypeResponse>> GetAdminBankCardType(LangRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<List<GetBankCardTypeResponse>>.Ok(WalletBiz.GetAdminBankCardType(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][GetAdminBankCardType]" + ex.Message);
        return APIResponse<List<GetBankCardTypeResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("collectinfo")]
    public APIResponse<string> CollectInfo(LangRequest req)
    {
      try
      {
        List<CollectInfoResponse> collectInfo = WalletBiz.GetCollectInfo(req.lang);
        return collectInfo == null ? APIResponse<string>.Ok("") : APIResponse<string>.Ok(DecryptTool.EncryptByAES(JsonSerializer.Serialize<List<CollectInfoResponse>>(collectInfo)));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[WalletController][CollectInfo]" + ex.Message);
        return APIResponse<string>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
