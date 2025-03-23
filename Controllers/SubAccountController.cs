
// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.SubAccountController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.SubAccount;
using tradeapi.Validates;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SubAccountController : ApiController
  {
    [HttpPost("holding")]
    public APIResponse<System.Collections.Generic.List<HoldingResponse>> Holding(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<System.Collections.Generic.List<HoldingResponse>>.Ok(SubAccountBiz.GetHolding(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Holding]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<HoldingResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("todayorders")]
    public APIResponse<System.Collections.Generic.List<TodayOrdersResponse>> TodayOrders(
      LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<System.Collections.Generic.List<TodayOrdersResponse>>.Ok(SubAccountBiz.GetTodayOrders(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][TodayOrders]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<TodayOrdersResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("todaydeal")]
    public APIResponse<System.Collections.Generic.List<TodayDealResponse>> TodayDeal(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<System.Collections.Generic.List<TodayDealResponse>>.Ok(SubAccountBiz.GetTodayDeal(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][TodayDeal]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<TodayDealResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("orderdetail")]
    public APIResponse<OrderDetailResponse> OrderDetail(OrderDetailRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<OrderDetailResponse>.Ok(SubAccountBiz.GetOrderDetail(token, req.trade_order_sn));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][OrderDetail]" + ex.Message);
        return APIResponse<OrderDetailResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("get")]
    public APIResponse<TradeAccountResponse> Get(GetRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token.member_fk;
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        return APIResponse<TradeAccountResponse>.Ok(SubAccountBiz.Get(memberFk, req.sub_account));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Get]" + ex.Message);
        return APIResponse<TradeAccountResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("setautorenewal")]
    public APIResponse SetAutoRenewal(SetAutoRenewalRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        SubAccountBiz.SetAutoRenewal(req);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][SetAutoRenewal]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("setworking")]
    public APIResponse Setworking(SetworkingRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        SubAccountBiz.Setworking(token, req);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Setworking]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("hisaccountrecord")]
    public APIResponse<System.Collections.Generic.List<HisAccountRecordResponse>> HisAccountRecord(
      HisAccountRecordRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        return APIResponse<System.Collections.Generic.List<HisAccountRecordResponse>>.Ok(SubAccountBiz.HisAccountRecord(req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][HisAccountRecord]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<HisAccountRecordResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("hisdeal")]
    public APIResponse<System.Collections.Generic.List<HisdealResponse>> Hisdeal(HisdealRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        return APIResponse<System.Collections.Generic.List<HisdealResponse>>.Ok(SubAccountBiz.Hisdeal(req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Hisdeal]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<HisdealResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("deal")]
    public APIResponse<DealResponse> Deal(DealRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<DealResponse>.Ok(SubAccountBiz.Deal(req.deal_id));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Deal]" + ex.Message);
        return APIResponse<DealResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("moneyrecord")]
    public APIResponse<System.Collections.Generic.List<MoneyRecordResponse>> MoneyRecord(
      MoneyRecordRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        return APIResponse<System.Collections.Generic.List<MoneyRecordResponse>>.Ok(SubAccountBiz.MoneyRecord(req.sub_account));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][MoneyRecord]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<MoneyRecordResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("terminate")]
    public APIResponse Terminate(TerminateRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token.member_fk;
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        SubAccountBiz.Terminate(token, req.sub_account, req.lang);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Terminate]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("premargincall")]
    public APIResponse<PreMarginCallResponse> MarginCall(PreMarginCallRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token.member_fk;
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        return APIResponse<PreMarginCallResponse>.Ok(SubAccountBiz.PreMarginCall(memberFk, req.sub_account));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][MarginCall]" + ex.Message);
        return APIResponse<PreMarginCallResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("margincall")]
    public APIResponse MarginCall(MarginCallRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token.member_fk;
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        SubAccountBiz.MarginCall(memberFk, req.sub_account, req.add_money, req.lang);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][MarginCall]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("renewal")]
    public APIResponse Renewal(RenewalRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token.member_fk;
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        SubAccountBiz.Renewal(memberFk, req.sub_account, req.borrow_duration, req.use_coupon);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Renewal]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("renewalfee")]
    public APIResponse<RenewalFeeResponse> RenewalFee(RenewalFeeRequest req)
    {
      TokenModel token = this.GetToken();
      int memberFk = token.member_fk;
      try
      {
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        return APIResponse<RenewalFeeResponse>.Ok(SubAccountBiz.RenewalFee(memberFk, req.sub_account, req.borrow_duration));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][RenewalFee]" + ex.Message);
        return APIResponse<RenewalFeeResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("expandfee")]
    public APIResponse<ExpandFeeResponse> ExpandFee(ExpandFeeRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (string.IsNullOrEmpty(req.sub_account))
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        return APIResponse<ExpandFeeResponse>.Ok(SubAccountBiz.ExpandFee(req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][ExpandFee]" + ex.Message);
        return APIResponse<ExpandFeeResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("expandfunding")]
    public APIResponse ExpandFunding(ExpandFundingRequest req)
    {
      int memberFk = this.GetToken().member_fk;
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(memberFk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        SubAccountBiz.ExpandFunding(memberFk, req);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][ExpandFunding]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("withdraw")]
    public APIResponse Withdraw(WithdrawRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        if (req.sub_account.IsEmpty())
          throw new AppException(1401, "subaccount_not_exist");
        if (!SubAccountBiz.CheckSubAccount(token.member_fk, req.sub_account))
          throw new AppException(1405, "account_not_belong_to_user");
        if (MemberServices.Find(token.member_fk).id_auth != 1 && Convert.ToBoolean(ConfigLib.Get("enable_id_auth")))
          throw new AppException(1303, "no_id_auth_yet");
        new WithdrawValidator().ValidateAndThrow<WithdrawRequest>(req);
        SubAccountBiz.Withdraw(req);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Withdraw]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("validity")]
    public APIResponse<System.Collections.Generic.List<TradeAccountResponse>> Validity(
      LangRequest req)
    {
      int memberFk = this.GetToken().member_fk;
      try
      {
        return APIResponse<System.Collections.Generic.List<TradeAccountResponse>>.Ok(SubAccountBiz.Validity(memberFk, req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Validity]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<TradeAccountResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("list")]
    public APIResponse<System.Collections.Generic.List<TradeAccountResponse>> List(LangRequest req)
    {
      int memberFk = this.GetToken().member_fk;
      try
      {
        return APIResponse<System.Collections.Generic.List<TradeAccountResponse>>.Ok(SubAccountBiz.List(memberFk, req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][List]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<TradeAccountResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("reviewborrow")]
    public APIResponse<ReviewBorrowResponse> ReviewBorrow(ReviewBorrowRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<ReviewBorrowResponse>.Ok(SubAccountBiz.ReviewBorrow(req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][ReviewBorrow]" + ex.Message);
        return APIResponse<ReviewBorrowResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("create")]
    public APIResponse<CreateResponse> Create(CreateRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<CreateResponse>.Ok(SubAccountBiz.Create(req, token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Create]" + ex.Message);
        return APIResponse<CreateResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("borrows")]
    public APIResponse<System.Collections.Generic.List<BorrowResponse>> Borrows(BorrowsRequest req)
    {
      int memberFk = this.GetToken().member_fk;
      try
      {
        return APIResponse<System.Collections.Generic.List<BorrowResponse>>.Ok(SubAccountBiz.GetBorrows(memberFk, req.status));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[SubAccountController][Borrows]" + ex.Message);
        return APIResponse<System.Collections.Generic.List<BorrowResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
