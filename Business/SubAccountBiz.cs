// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.SubAccountBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using Services;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.Enums;
using tradeapi.Models.SubAccount;
using tradeapi.Models.Wallet;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class SubAccountBiz
  {
    public static bool CheckSubAccount(int member_fk, string sub_account)
    {
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(sub_account);
      return tradeAccountDto != null && tradeAccountDto.member_fk == member_fk;
    }

    public static System.Collections.Generic.List<HoldingResponse> GetHolding(TokenModel tokenModel)
    {
      System.Collections.Generic.List<HoldingResponse> holding = new System.Collections.Generic.List<HoldingResponse>();
      foreach (TradePositionDto tradePositionDto in TradePositionService.FindBySubAccount(tokenModel.sub_account))
      {
        Decimal num = (Decimal) (tradePositionDto.stock_type * tradePositionDto.holding_volume) * (tradePositionDto.lastprice - tradePositionDto.cost_price);
        HoldingResponse holdingResponse = new HoldingResponse()
        {
          stock_code = tradePositionDto.stock_code,
          stock_name = tradePositionDto.stock_name,
          dir = tradePositionDto.stock_type == 1 ? OrderSide.OrderSide_Buy : (tradePositionDto.stock_type == -1 ? OrderSide.OrderSide_Sell : OrderSide.OrderSide_Unknown),
          profit = num,
          profit_percentage = tradePositionDto.cost_purchase > 0M ? num / tradePositionDto.cost_purchase * 100M : 0M,
          holding = tradePositionDto.holding_volume,
          frozen = tradePositionDto.new_pos + tradePositionDto.close_pos,
          lastprice = tradePositionDto.lastprice,
          cost_price = tradePositionDto.cost_price,
          market = tradePositionDto.market
        };
        holding.Add(holdingResponse);
      }
      return holding;
    }

    public static System.Collections.Generic.List<TodayOrdersResponse> GetTodayOrders(
      TokenModel tokenModel)
    {
      System.Collections.Generic.List<TodayOrdersResponse> todayOrders = new System.Collections.Generic.List<TodayOrdersResponse>();
      string subAccount = tokenModel.sub_account;
      if (subAccount != null)
      {
        string market = TradeAccountService.Find(subAccount).market;
        DateTime utc = Tool.ConvertTimeToUtc(market, Tool.GetTime(market).Date);
        DateTime to = utc.AddDays(1.0);
        foreach (TradeOrderDto order in TradeOrderService.GetOrders(subAccount, utc, to))
        {
          TodayOrdersResponse todayOrdersResponse = new TodayOrdersResponse()
          {
            trade_order_sn = order.sn,
            stock_code = order.stock_code,
            stock_name = order.stock_name,
            dir = order.dir,
            price_type = order.price_type,
            price = order.price,
            status = order.status,
            volume = order.volume,
            free_volume = order.free_volume,
            succeed_volume = order.succeed_volume,
            order_type = order.order_type,
            order_time = order.order_time
          };
          System.Collections.Generic.List<TradeDealDto> byTradeOrderSn = TradeDealService.FindByTradeOrderSN(order.sn);
          Decimal num1 = 0M;
          Decimal num2 = 0M;
          foreach (TradeDealDto tradeDealDto in byTradeOrderSn)
          {
            num1 += tradeDealDto.final_price * (Decimal) tradeDealDto.final_volume;
            num2 += (Decimal) tradeDealDto.final_volume;
          }
          todayOrdersResponse.final_price = num2 > 0M ? num1 / num2 : 0M;
          todayOrders.Add(todayOrdersResponse);
        }
      }
      return todayOrders;
    }

    public static System.Collections.Generic.List<HisAccountRecordResponse> HisAccountRecord(
      HisAccountRecordRequest req)
    {
      return TradeMoneyRecordService.FindByTradeAccount(req.sub_account);
    }

    public static System.Collections.Generic.List<TodayDealResponse> GetTodayDeal(
      TokenModel tokenModel)
    {
      System.Collections.Generic.List<TradeDealDto> todayBySubAccount = TradeDealService.FindTodayBySubAccount(tokenModel.sub_account);
      System.Collections.Generic.List<TodayDealResponse> todayDeal = new System.Collections.Generic.List<TodayDealResponse>();
      foreach (TradeDealDto tradeDealDto in todayBySubAccount)
      {
        TodayDealResponse todayDealResponse = new TodayDealResponse()
        {
          deal_id = tradeDealDto.deal_id,
          trade_order_sn = tradeDealDto.trade_order_sn,
          trade_time = tradeDealDto.create_datetime,
          stock_code = tradeDealDto.stock_code,
          stock_name = tradeDealDto.stock_name,
          dir = tradeDealDto.dir,
          final_price = tradeDealDto.final_price,
          final_volume = tradeDealDto.final_volume,
          total_amount = tradeDealDto.total_amount
        };
        todayDeal.Add(todayDealResponse);
      }
      return todayDeal;
    }

    public static OrderDetailResponse GetOrderDetail(TokenModel tokenModel, string trade_order_sn)
    {
      TradeOrderDto bySn = TradeOrderService.FindBySN(trade_order_sn);
      System.Collections.Generic.List<TradeDealDto> byTradeOrderSn = TradeDealService.FindByTradeOrderSN(trade_order_sn);
      Decimal num1 = 0M;
      int num2 = 0;
      foreach (TradeDealDto tradeDealDto in byTradeOrderSn)
      {
        num1 += tradeDealDto.final_price * (Decimal) tradeDealDto.final_volume;
        num2 += tradeDealDto.final_volume;
      }
      Decimal num3 = 0M;
      if (num2 > 0)
        num3 = Math.Round(num1 / (Decimal) num2, 2);
      return new OrderDetailResponse()
      {
        trade_order_sn = trade_order_sn,
        order_time = bySn.order_time,
        stock_code = bySn.stock_code,
        stock_name = bySn.stock_name,
        price_type = bySn.price_type,
        order_type = bySn.order_type,
        price = bySn.price,
        volume = bySn.volume,
        avg_price = num3,
        succeed_volume = bySn.succeed_volume,
        free_volume = bySn.free_volume,
        dir = bySn.dir,
        status = bySn.status
      };
    }

    public static TradeAccountResponse Get(int member_id, string sub_account)
    {
      ViewTradeAccountDto viewTradeAccount = ViewTradeAccountService.GetViewTradeAccount(sub_account);
      MemberDto member = MemberServices.GetMember(member_id);
      Decimal num1 = viewTradeAccount.margin + viewTradeAccount.loan_money;
      Decimal num2 = Math.Min(viewTradeAccount.balance - num1, viewTradeAccount.mem_money);
      Decimal num3 = num2 > 0M ? num2 : 0M;
      return new TradeAccountResponse()
      {
        sub_account = viewTradeAccount.sub_account,
        is_trading = sub_account.Equals(member.sub_account),
        begin_time = viewTradeAccount.begin_time,
        end_time = viewTradeAccount.end_time,
        loan_type = viewTradeAccount.loan_type,
        balance = viewTradeAccount.balance,
        mem_money = viewTradeAccount.mem_money,
        margin = viewTradeAccount.margin,
        margin_float = viewTradeAccount.margin_float,
        market = viewTradeAccount.market,
        init_money = num1,
        position_value = viewTradeAccount.position_value,
        warningline = viewTradeAccount.warningline,
        warning_value = viewTradeAccount.warning_value,
        breakline = viewTradeAccount.breakline,
        break_value = viewTradeAccount.break_value,
        currency = viewTradeAccount.currency,
        withdrawable_amount = num3
      };
    }

    public static void SetAutoRenewal(SetAutoRenewalRequest req)
    {
      if (!BorrowPlanService.GetBorrowPlan(BorrowService.GetBorrow(req.sub_account).borrow_plan_fk).renewal)
        throw new AppException(1805, "renewal_is_not_allowed");
      BorrowService.SetAutoRenewal(req.sub_account, req.enable);
    }

    public static void Setworking(TokenModel token_model, SetworkingRequest req)
    {
      MemberServices.SetSubAccount(token_model.member_fk, req.enable ? req.sub_account : string.Empty);
      string token = MemberServices.GetToken(token_model.member_fk);
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(req.sub_account);
      token_model.market = req.enable ? tradeAccountDto.market : (string) null;
      token_model.sub_account = req.enable ? req.sub_account : (string) null;
      TokenCatch.SetToken(token, token_model);
    }

    public static System.Collections.Generic.List<HisdealResponse> Hisdeal(HisdealRequest req)
    {
      return TradeDealService.GetDeals(req.sub_account, req.date_start, req.date_end, req.keyword);
    }

    public static DealResponse Deal(string deal_id) => TradeDealService.GetDeal(deal_id);

    public static System.Collections.Generic.List<MoneyRecordResponse> MoneyRecord(
      string sub_account)
    {
      return TradeMoneyRecordService.GetMoneyRecords(sub_account);
    }

    public static void Terminate(TokenModel tokenModel, string sub_account, string lang)
    {
      int memberFk = tokenModel.member_fk;
      BorrowDto borrow = BorrowService.GetBorrow(sub_account);
      if (TradePositionService.GetPositionValue(sub_account) > 0M)
        throw new AppException(1816, "account_has_position");
      if (BorrowRequestService.FindTerminateRequest(sub_account) != null)
        throw new AppException(1180, "already_reviewing");
      using (TransactionScope transactionScope = new TransactionScope())
      {
        TradeOrderService.FindOpenOrders(sub_account).ForEach((Action<TradeOrderDto>) (order => OrderHelper.CancelOrder(order.pk, 2, "system")));
        SubAccountBiz.TerminateVerify(BorrowRequestService.Insert(new BorrowRequestDto()
        {
          sub_account = sub_account,
          borrow_fk = borrow.pk,
          member_fk = memberFk,
          type = 2,
          status = 0,
          add_time = DateTime.UtcNow
        }));
        SubAccountBiz.BalanceTransfer(tokenModel, sub_account, lang);
        transactionScope.Complete();
      }
    }

    public static void TerminateVerify(int borrowRequestId)
    {
      BorrowRequestDto borrowRequestDto = BorrowRequestService.Find(borrowRequestId);
      TradeAccountDto tradeAccountDto = borrowRequestDto.status == 0 ? TradeAccountService.Find(borrowRequestDto.sub_account) : throw new AppException(3032, "verify_status_incorrect");
      if (tradeAccountDto.status == 3)
      {
        BorrowRequestService.UpdateState(borrowRequestId, true);
        throw new AppException(3042, "account_already_close");
      }
      if (TradePositionService.GetPositionBySubAccount(borrowRequestDto.sub_account) > 0)
        throw new AppException(3051, "stock_not_position");
      if (tradeAccountDto.loan_type == "trial")
        throw new AppException(3052, "trial_no_early_terminate");
      TradeAccountService.InAdvanceClose(borrowRequestDto.sub_account, 1);
      BorrowService.InAdvanceReset(borrowRequestDto.borrow_fk, borrowRequestDto.sub_account);
      BorrowService.UpdateStatus(borrowRequestDto.borrow_fk, BorrowStatus.End);
      BorrowService.DisableAutoRenewal(borrowRequestDto.borrow_fk);
      BorrowRequestService.UpdateState(borrowRequestId, true);
      int temp_id = 53;
      SendMessageLib.Send(borrowRequestDto.member_fk, temp_id, (object) borrowRequestDto.pk);
    }

    public static PreMarginCallResponse PreMarginCall(int member_id, string sub_account)
    {
      ViewTradeAccountDto viewTradeAccount = ViewTradeAccountService.GetViewTradeAccount(sub_account);
      WalletDto walletDto = WalletService.Find(member_id);
      return new PreMarginCallResponse()
      {
        sub_account = sub_account,
        init_money = viewTradeAccount.margin + viewTradeAccount.loan_money,
        balance = viewTradeAccount.balance,
        warningline = viewTradeAccount.warningline,
        breakline = viewTradeAccount.breakline,
        max_add_money = viewTradeAccount.margin + viewTradeAccount.loan_money - viewTradeAccount.balance,
        wallet_balance = walletDto.balance,
        currency = viewTradeAccount.currency,
        exchange_rate = ExchangeHelper.GetRate(viewTradeAccount.currency, ConfigLib.Get("wallet_currency"))
      };
    }

    public static void MarginCall(
      int member_id,
      string sub_account,
      Decimal add_money,
      string lang)
    {
      ViewTradeAccountDto viewTradeAccount = ViewTradeAccountService.GetViewTradeAccount(sub_account);
      Decimal num = viewTradeAccount.margin + viewTradeAccount.loan_money - viewTradeAccount.balance;
      if (add_money <= 0M)
        throw new AppException(1819, "add_money_must_greater_than_zero");
      if (add_money > num)
        throw new AppException(1817, "exceeds_the_maximum_add_money_amount");
      if (BorrowAddmoneyService.Find(sub_account) != null)
        throw new AppException(1180, "already_reviewing");
      string base_symbol = ConfigLib.Get("wallet_currency");
      Decimal rate = ExchangeHelper.GetRate(viewTradeAccount.currency, base_symbol);
      Decimal change = ExchangeHelper.Convert(add_money, viewTradeAccount.currency, base_symbol);
      if (!WalletLib.IsRequestrMoneyOk(member_id, change))
        throw new AppException(1803, "insufficient_balance");
      using (TransactionScope transactionScope = new TransactionScope())
      {
        SubAccountBiz.ReviewMarginCall(BorrowAddmoneyService.FindPkAfterInsert(new BorrowAddmoneyDto()
        {
          sub_account = sub_account,
          member_fk = member_id,
          currency = viewTradeAccount.currency,
          exchange = rate,
          money = add_money,
          status = 0,
          add_time = DateTime.UtcNow,
          freeze = change
        }), lang);
        transactionScope.Complete();
      }
    }

    public static bool ReviewMarginCall(int pk, string lang)
    {
      BorrowAddmoneyDto borrowAddmoneyDto = BorrowAddmoneyService.Find(pk);
      VwTradeAccountDto accountBySubAccount = TradeMoneyCheckService.GetVwTradeAccountBySubAccount(borrowAddmoneyDto.sub_account);
      BorrowAddmoneyService.UpdateStatus(pk, true);
      int temp_id = 38;
      WalletDto walletDto = WalletService.Find(accountBySubAccount.member_fk);
      Decimal rate = ExchangeLib.GetRate(borrowAddmoneyDto.currency, walletDto.currency);
      int memberFk = accountBySubAccount.member_fk;
      Decimal freeze = borrowAddmoneyDto.freeze;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler.AppendFormatted<int>(borrowAddmoneyDto.pk);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      string subAccount = accountBySubAccount.sub_account;
      Decimal money = borrowAddmoneyDto.money;
      string currency = borrowAddmoneyDto.currency;
      WalletLib.MaginCallPass(memberFk, freeze, stringAndClear, subAccount, money, currency);
      TradeMoneyRecordDto byAccount = TradeMoneyRecordService.GetByAccount(borrowAddmoneyDto.sub_account);
      object[] objArray = new object[4]
      {
        (object) borrowAddmoneyDto.money,
        (object) borrowAddmoneyDto.currency,
        (object) borrowAddmoneyDto.freeze,
        (object) walletDto.currency
      };
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        member_fk = borrowAddmoneyDto.member_fk,
        sub_account = borrowAddmoneyDto.sub_account,
        temp_id = temp_id,
        sn = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") ?? "",
        currency = borrowAddmoneyDto.currency,
        affect = borrowAddmoneyDto.money,
        balance = byAccount.balance + borrowAddmoneyDto.money,
        exchange = rate,
        wallet_amount = borrowAddmoneyDto.freeze,
        op = 1,
        list = objArray,
        create_datetime = DateTime.UtcNow
      }, lang);
      TradeAccountService.UpdateMoney(borrowAddmoneyDto.sub_account, borrowAddmoneyDto.money);
      MemberTaskLib.MemberTaskFinish(borrowAddmoneyDto.member_fk, 6);
      SendMessageLib.Send(accountBySubAccount.member_fk, temp_id, (object) borrowAddmoneyDto.pk, (object) accountBySubAccount.sub_account, (object) Tool.AddNumberSeparation(new Decimal?(borrowAddmoneyDto.money), borrowAddmoneyDto.currency), (object) borrowAddmoneyDto.currency, (object) Tool.AddNumberSeparation(new Decimal?(borrowAddmoneyDto.freeze), walletDto.currency), (object) walletDto.currency);
      return true;
    }

    public static void Renewal(
      int member_id,
      string sub_account,
      int borrow_duration,
      Decimal use_coupon)
    {
      BorrowPlanDto borrowPlan = BorrowPlanService.GetBorrowPlan(sub_account);
      string borrowType = borrowPlan.borrow_type;
      if (borrowType.Equals("trial") || borrowType.Equals("free"))
        throw new AppException(1805, "plan_cannot_be_renewed");
      BorrowDto borrow = BorrowService.GetBorrow(sub_account);
      if (borrow.auto_renewal)
        throw new AppException(1813, "account_is_auto_renewal");
      Decimal amount = BorrowHelper.Round(borrow.borrow_money * (Decimal) borrow_duration * borrow.rate / 100M, borrow.currency);
      string base_symbol = ConfigLib.Get("wallet_currency");
      if (!WalletLib.IsRequestrMoneyOk(member_id, ExchangeHelper.Convert(amount, borrow.currency, base_symbol)))
        throw new AppException(1803, "insufficient_balance");
      if (!WalletLib.IsRequestCouponOk(member_id, ExchangeHelper.Convert(use_coupon, borrow.currency, base_symbol)))
        throw new AppException(1804, "insufficient_coupon");
      if (use_coupon > amount)
        throw new AppException(1807, "use_coupon_exceeds_management_fee");
      if (BorrowRequestService.IsExistRecord(sub_account))
        throw new AppException(1806, "renewal_application_already_exists");
      DateTime endTime = BorrowHelper.GetBeginAndEndTime(borrow.end_time, borrow.market, 1, borrow_duration, borrowType).end_time;
      using (TransactionScope transactionScope = new TransactionScope())
      {
        int num = BorrowRequestService.Insert(new BorrowRequestDto()
        {
          borrow_plan_fk = borrowPlan.pk,
          sub_account = sub_account,
          borrow_fk = borrow.pk,
          member_fk = member_id,
          type = 1,
          borrow_fee = amount,
          use_coupon = use_coupon,
          fee_received = amount - use_coupon,
          borrow_duration = borrow_duration,
          new_end_time = endTime,
          status = 0,
          add_time = DateTime.UtcNow
        });
        int member = member_id;
        Decimal change = amount;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
        interpolatedStringHandler.AppendLiteral("br_");
        interpolatedStringHandler.AppendFormatted<int>(num);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        WalletLib.BorrowRenewalApply(member, change, stringAndClear);
        SendMessageLib.Send(member_id, 41);
        transactionScope.Complete();
      }
    }

    public static RenewalFeeResponse RenewalFee(
      int member_id,
      string sub_account,
      int borrow_duration)
    {
      WalletDto walletDto = WalletService.Find(member_id);
      BorrowDto borrow = BorrowService.GetBorrow(sub_account);
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(sub_account);
      return new RenewalFeeResponse()
      {
        borrow_fee = BorrowHelper.Round(borrow.borrow_money * (Decimal) borrow_duration * borrow.rate / 100M, tradeAccountDto.currency),
        coupon = walletDto.coupon
      };
    }

    public static ExpandFeeResponse ExpandFee(ExpandFeeRequest req)
    {
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(req.sub_account);
      BorrowPlanDto borrowPlan = BorrowPlanService.GetBorrowPlan(tradeAccountDto.borrow_plan_fk);
      Decimal borrowFee = BorrowHelper.BorrowFee(borrowPlan, tradeAccountDto.margin, tradeAccountDto.multiple, tradeAccountDto.borrow_duration, tradeAccountDto.currency).borrow_fee;
      Decimal num1 = BorrowHelper.ExpandFee(req.deposit_money, tradeAccountDto.margin, tradeAccountDto.loan_type, tradeAccountDto.market, borrowFee, tradeAccountDto.begin_time, tradeAccountDto.end_time, tradeAccountDto.currency);
      Decimal num2 = req.deposit_money * (Decimal) tradeAccountDto.multiple;
      return new ExpandFeeResponse()
      {
        borrow_fee = num1,
        borrow_money = num2,
        new_trading_quota = tradeAccountDto.margin + tradeAccountDto.loan_money + req.deposit_money + num2,
        currency = Tool.GetCurrencyByMarket(borrowPlan.market)
      };
    }

    public static void ExpandFunding(int member_id, ExpandFundingRequest req)
    {
      BorrowDto borrow = BorrowService.GetBorrow(req.sub_account);
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(req.sub_account);
      BorrowPlanDto borrowPlan = BorrowPlanService.GetBorrowPlan(tradeAccountDto.borrow_plan_fk);
      if (DateTime.UtcNow.Date > tradeAccountDto.end_time)
        throw new AppException(1840, "account_expired");
      if (req.deposit_money + tradeAccountDto.margin > borrowPlan.money_range_max)
        throw new AppException(1817, "exceeds_the_maximum_add_money_amount");
      if (tradeAccountDto.status != 0)
        throw new AppException(1860, "account_status_cannot_margin_call");
      string base_symbol = ConfigLib.Get("wallet_currency");
      Decimal borrowFee = BorrowHelper.BorrowFee(borrowPlan, tradeAccountDto.margin, tradeAccountDto.multiple, tradeAccountDto.borrow_duration, borrow.currency).borrow_fee;
      Decimal amount = BorrowHelper.ExpandFee(req.deposit_money, tradeAccountDto.margin, tradeAccountDto.loan_type, tradeAccountDto.market, borrowFee, tradeAccountDto.begin_time, tradeAccountDto.end_time, tradeAccountDto.currency);
      Decimal num1 = ExchangeHelper.Convert(req.deposit_money, tradeAccountDto.currency, base_symbol);
      Decimal num2 = ExchangeHelper.Convert(amount, tradeAccountDto.currency, base_symbol);
      Decimal change = num1 + num2;
      if (!WalletLib.IsRequestrMoneyOk(member_id, change))
        throw new AppException(1803, "insufficient_balance");
      if (BorrowAddfinancingService.Find(req.sub_account) != null)
        throw new AppException(1180, "already_reviewing");
      using (TransactionScope transactionScope = new TransactionScope())
      {
        SubAccountBiz.ExpandFundingVerify(BorrowAddfinancingService.FindPkAfterInsert(new BorrowAddfinancingDto()
        {
          sub_account = req.sub_account,
          borrow_fk = borrow == null ? 0 : borrow.pk,
          member_fk = member_id,
          currency = tradeAccountDto.currency,
          money = req.deposit_money,
          exchange = ExchangeHelper.GetRate(tradeAccountDto.currency, base_symbol),
          freeze = num1,
          multiple = (Decimal) tradeAccountDto.multiple,
          borrow_interest = num2,
          last_deposit_money = tradeAccountDto.margin,
          last_borrow_money = tradeAccountDto.loan_money,
          status = 0,
          add_time = DateTime.UtcNow
        }), req.lang);
        transactionScope.Complete();
      }
    }

    public static void ExpandFundingVerify(int id, string lang)
    {
      BorrowAddfinancingDto borrowAddfinancingDto = BorrowAddfinancingService.Find(id);
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(borrowAddfinancingDto.sub_account);
      BorrowAddfinancingService.UpdateState(id, true);
      WalletDto walletDto = WalletService.Find(borrowAddfinancingDto.member_fk);
      Decimal rate = ExchangeLib.GetRate(borrowAddfinancingDto.currency, walletDto.currency);
      int temp_id = 45;
      string str = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") ?? "";
      BorrowPlanDto byBorrowType = BorrowPlanService.FindByBorrowType(tradeAccountDto.loan_type, tradeAccountDto.market);
      TradeAccountService.UpdateExpandBorrow(borrowAddfinancingDto.sub_account, borrowAddfinancingDto.money, borrowAddfinancingDto.multiple, Convert.ToDecimal(byBorrowType.warning_line / 100M), Convert.ToDecimal(byBorrowType.break_line / 100M));
      TradeMoneyRecordDto byAccount = TradeMoneyRecordService.GetByAccount(borrowAddfinancingDto.sub_account);
      object[] objArray1 = new object[4]
      {
        (object) borrowAddfinancingDto.money,
        (object) borrowAddfinancingDto.currency,
        (object) borrowAddfinancingDto.freeze,
        (object) walletDto.currency
      };
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        member_fk = borrowAddfinancingDto.member_fk,
        sub_account = borrowAddfinancingDto.sub_account,
        temp_id = temp_id,
        sn = str,
        currency = borrowAddfinancingDto.currency,
        affect = borrowAddfinancingDto.money,
        balance = (byAccount != null ? byAccount.balance : 0M) + borrowAddfinancingDto.money,
        exchange = rate,
        wallet_amount = borrowAddfinancingDto.freeze,
        op = 1,
        list = objArray1,
        create_datetime = DateTime.UtcNow
      }, lang);
      Decimal num = borrowAddfinancingDto.money * borrowAddfinancingDto.multiple;
      object[] objArray2 = new object[4]
      {
        (object) num,
        (object) borrowAddfinancingDto.currency,
        (object) 0,
        (object) walletDto.currency
      };
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        sub_account = borrowAddfinancingDto.sub_account,
        member_fk = borrowAddfinancingDto.member_fk,
        sn = str,
        temp_id = 203,
        op = 1,
        currency = borrowAddfinancingDto.currency,
        balance = (byAccount != null ? byAccount.balance : 0M) + borrowAddfinancingDto.money + borrowAddfinancingDto.money * borrowAddfinancingDto.multiple,
        affect = num,
        exchange = 0M,
        wallet_amount = 0M,
        list = objArray2,
        create_datetime = DateTime.UtcNow
      }, lang);
      WalletLib.ExpandBorrowManagementFee(borrowAddfinancingDto.member_fk, borrowAddfinancingDto.borrow_interest, Convert.ToString(borrowAddfinancingDto.pk), borrowAddfinancingDto.sub_account);
      WalletLib.ExpandBorrowPass(borrowAddfinancingDto.member_fk, borrowAddfinancingDto.freeze, Convert.ToString(borrowAddfinancingDto.pk), borrowAddfinancingDto.sub_account, borrowAddfinancingDto.money, borrowAddfinancingDto.currency);
      MemberTaskLib.MemberTaskFinish(borrowAddfinancingDto.member_fk, 5);
      SendMessageLib.Send(borrowAddfinancingDto.member_fk, temp_id, (object) borrowAddfinancingDto.pk, (object) borrowAddfinancingDto.sub_account, (object) Tool.AddNumberSeparation(new Decimal?(borrowAddfinancingDto.freeze), walletDto.currency), (object) walletDto.currency, (object) Tool.AddNumberSeparation(new Decimal?(borrowAddfinancingDto.money), borrowAddfinancingDto.currency), (object) borrowAddfinancingDto.currency);
    }

    public static void Withdraw(WithdrawRequest req)
    {
      ViewTradeAccountDto viewTradeAccount = ViewTradeAccountService.GetViewTradeAccount(req.sub_account);
      Decimal num1 = viewTradeAccount.margin + viewTradeAccount.loan_money;
      Decimal num2 = Math.Min(viewTradeAccount.mem_money + viewTradeAccount.position_value - num1, viewTradeAccount.mem_money);
      if (BorrowPlanService.GetBorrowPlan(TradeAccountService.Find(req.sub_account).borrow_plan_fk).borrow_type == "trial")
        throw new AppException(1891, "trial_account_not_allow_withdraw");
      if (req.withdraw_amount <= 0M)
        throw new AppException(1818, "withdraw_amount_must_greater_than_zero");
      if (req.withdraw_amount > num2)
        throw new AppException(1811, "exceeds_withdrawable_amount");
      if (TradeMoneyCheckService.Find(req.sub_account) != null)
        throw new AppException(1180, "already_reviewing");
      string str = ConfigLib.Get("wallet_currency");
      Decimal rate = ExchangeHelper.GetRate(viewTradeAccount.currency, str);
      Decimal num3 = ExchangeHelper.Convert(req.withdraw_amount, str, viewTradeAccount.currency);
      using (TransactionScope transactionScope = new TransactionScope())
      {
        int pkAfterInsert = TradeMoneyCheckService.FindPkAfterInsert(new TradeMoneyCheckDto()
        {
          sub_account = req.sub_account,
          sn = SubAccountBiz.MakeTradeMoneyCheckSn(),
          type = 0,
          state = 0,
          frozen = req.withdraw_amount,
          exchange = rate,
          currency = viewTradeAccount.currency,
          amount = num3,
          request_time = DateTime.UtcNow
        });
        viewTradeAccount.frozen_money += req.withdraw_amount;
        TradeAccountService.UpdateFrozenMoney(viewTradeAccount.sub_account, viewTradeAccount.frozen_money);
        SubAccountBiz.WithdrawVerify(pkAfterInsert, req.lang);
        transactionScope.Complete();
      }
    }

    public static bool WithdrawVerify(int pk, string lang)
    {
      TradeMoneyCheckDto tradeMoneyCheckDto = TradeMoneyCheckService.Find(pk);
      VwTradeAccountDto accountBySubAccount = TradeMoneyCheckService.GetVwTradeAccountBySubAccount(tradeMoneyCheckDto.sub_account);
      if (accountBySubAccount.balance - tradeMoneyCheckDto.frozen <= (accountBySubAccount.warningline + accountBySubAccount.breakline) / 2M)
        throw new AppException(3020, "not_enough_trade_money");
      if (tradeMoneyCheckDto.state != 0)
        throw new AppException(3021, "verify_withdraw_status_incorrect");
      TradeAccountService.UpdateTradeAccountVolume(true, tradeMoneyCheckDto.sub_account, tradeMoneyCheckDto.frozen);
      TradeFrozenService.DelTradeFrozen(tradeMoneyCheckDto.pk);
      TradeMoneyCheckService.UpdateWithdraw(Convert.ToInt32(pk), 1);
      TradeMoneyRecordDto byAccount = TradeMoneyRecordService.GetByAccount(tradeMoneyCheckDto.sub_account);
      object[] objArray = new object[4]
      {
        (object) tradeMoneyCheckDto.frozen,
        (object) tradeMoneyCheckDto.currency,
        (object) tradeMoneyCheckDto.amount,
        (object) ConfigLib.Get("wallet_currency")
      };
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        member_fk = accountBySubAccount.member_fk,
        sub_account = tradeMoneyCheckDto.sub_account,
        temp_id = 49,
        sn = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") ?? "",
        currency = tradeMoneyCheckDto.currency,
        affect = -tradeMoneyCheckDto.frozen,
        balance = (byAccount != null ? byAccount.balance : 0M) - tradeMoneyCheckDto.frozen,
        exchange = ExchangeHelper.GetRate(ConfigLib.Get("Wallet_Currency"), tradeMoneyCheckDto.currency),
        wallet_amount = -tradeMoneyCheckDto.amount,
        op = 1,
        list = objArray,
        create_datetime = DateTime.UtcNow
      });
      WalletDto walletDto = WalletService.Find(accountBySubAccount.member_fk);
      WalletLib.WithdrawTradePass(accountBySubAccount.member_fk, tradeMoneyCheckDto.sn, tradeMoneyCheckDto.amount, tradeMoneyCheckDto.sub_account, tradeMoneyCheckDto.frozen, tradeMoneyCheckDto.currency);
      MemberTaskLib.MemberTaskFinish(accountBySubAccount.member_fk, 8);
      SendMessageLib.Send(accountBySubAccount.member_fk, 49, (object) tradeMoneyCheckDto.sn, (object) tradeMoneyCheckDto.sub_account, (object) Tool.AddNumberSeparation(new Decimal?(tradeMoneyCheckDto.frozen), tradeMoneyCheckDto.currency), (object) tradeMoneyCheckDto.currency, (object) Tool.AddNumberSeparation(new Decimal?(tradeMoneyCheckDto.amount), walletDto.currency), (object) walletDto.currency);
      return true;
    }

    private static string MakeTradeMoneyCheckSn()
    {
      string sn = "";
      for (bool flag = true; flag; flag = TradeMoneyCheckService.IsDuplicateSN(sn))
        sn = Tool.MakeRandomNo(20);
      return sn;
    }

    public static System.Collections.Generic.List<TradeAccountResponse> Validity(
      int member_id,
      string lang)
    {
      lang = lang == null ? ConfigLib.Get("app_default_lang") : lang.ToUpper();
      System.Collections.Generic.List<TradeAccountResponse> validTradeAccounts = ViewTradeAccountService.GetValidTradeAccounts(member_id);
      MemberDto member = MemberServices.GetMember(member_id);
      foreach (TradeAccountResponse tradeAccountResponse in validTradeAccounts)
      {
        TradeAccountResponse item = tradeAccountResponse;
        if (item.sub_account.Equals(member.sub_account))
          item.is_trading = true;
        System.Collections.Generic.List<MutilangTableDto> all = MutilangTableService.FindAll();
        MutilangTableDto mutilangTableDto1 = all.Where<MutilangTableDto>((Func<MutilangTableDto, bool>) (x => x.dbtable.Equals("sys_market") && x.field.Equals("name") && x.key.Equals(item.market) && x.lang.Equals(lang))).FirstOrDefault<MutilangTableDto>();
        item.market_name = mutilangTableDto1 == null ? item.market : mutilangTableDto1.value;
        string loan_type = TradeAccountService.Find(item.sub_account).loan_type;
        item.loan_type = loan_type;
        MutilangTableDto mutilangTableDto2 = all.Where<MutilangTableDto>((Func<MutilangTableDto, bool>) (x => x.dbtable.Equals("borrow_plan") && x.field.Equals("borrow_type") && x.key.Equals(loan_type) && x.lang.Equals(lang))).FirstOrDefault<MutilangTableDto>();
        item.loan_name = mutilangTableDto2 == null ? item.loan_type : mutilangTableDto2.value;
      }
      return validTradeAccounts;
    }

    public static System.Collections.Generic.List<TradeAccountResponse> List(
      int member_id,
      string lang)
    {
      lang = lang == null ? ConfigLib.Get("app_default_lang") : lang.ToUpper();
      System.Collections.Generic.List<TradeAccountResponse> tradeAccountResponseList = ViewTradeAccountService.List(member_id);
      MemberDto member = MemberServices.GetMember(member_id);
      foreach (TradeAccountResponse tradeAccountResponse in tradeAccountResponseList)
      {
        TradeAccountResponse item = tradeAccountResponse;
        if (item.sub_account.Equals(member.sub_account))
          item.is_trading = true;
        System.Collections.Generic.List<MutilangTableDto> all = MutilangTableService.FindAll();
        MutilangTableDto mutilangTableDto1 = all.Where<MutilangTableDto>((Func<MutilangTableDto, bool>) (x => x.dbtable.Equals("sys_market") && x.field.Equals("name") && x.key.Equals(item.market) && x.lang.Equals(lang))).FirstOrDefault<MutilangTableDto>();
        item.market_name = mutilangTableDto1 == null ? item.market : mutilangTableDto1.value;
        string loan_type = TradeAccountService.Find(item.sub_account).loan_type;
        item.loan_type = loan_type;
        MutilangTableDto mutilangTableDto2 = all.Where<MutilangTableDto>((Func<MutilangTableDto, bool>) (x => x.dbtable.Equals("borrow_plan") && x.field.Equals("borrow_type") && x.key.Equals(loan_type) && x.lang.Equals(lang))).FirstOrDefault<MutilangTableDto>();
        item.loan_name = mutilangTableDto2 == null ? item.loan_type : mutilangTableDto2.value;
      }
      return tradeAccountResponseList;
    }

    public static ReviewBorrowResponse ReviewBorrow(ReviewBorrowRequest req)
    {
      int borrowPlanPk = req.borrow_plan_pk;
      BorrowPlanDto borrowPlan = BorrowPlanService.GetBorrowPlan(borrowPlanPk);
      string currency = SysMarketService.Find(borrowPlan.market).currency;
      (int multiple, int borrow_duration, Decimal deposit_money, Decimal borrow_money, Decimal _, Decimal borrow_fee) = BorrowHelper.BorrowFee(borrowPlan, req.deposit_money, req.multiple, req.borrow_duration, currency);
      return new ReviewBorrowResponse()
      {
        borrow_plan_pk = borrowPlanPk,
        deposit_money = deposit_money,
        borrow_duration = borrow_duration,
        multiple = multiple,
        borrow_money = borrow_money,
        warningline = borrow_money + BorrowHelper.Round(deposit_money * borrowPlan.warning_line / 100M, currency),
        breakline = borrow_money + BorrowHelper.Round(deposit_money * borrowPlan.break_line / 100M, currency),
        borrow_fee = borrow_fee,
        note = borrowPlan.note,
        currency = currency
      };
    }

    public static CreateResponse Create(CreateRequest req, int member_id)
    {
      MemberDto member = MemberServices.GetMember(member_id);
      if (Tool.ToBool(ConfigLib.Get("enable_id_auth")) && member.id_auth != 1)
        throw new AppException(1303, "no_id_auth_yet");
      int borrowPlanPk = req.borrow_plan_pk;
      BorrowPlanDto borrowPlan = BorrowPlanService.GetBorrowPlan(borrowPlanPk);
      System.Collections.Generic.List<BorrowDto> borrows = BorrowService.GetBorrows(member_id);
      if ("trial".Equals(borrowPlan.borrow_type) && borrows.Where<BorrowDto>((Func<BorrowDto, bool>) (x => x.borrow_type.Equals("trial") && x.status != 0)).Any<BorrowDto>())
        throw new AppException(1802, "trial_plan_is_already_applied");
      if (borrowPlan.borrow_type != "trial")
      {
        if (req.deposit_money < borrowPlan.money_range_min)
          throw new AppException(1814, "deposit_money_is_less_than_minimum");
        if (req.deposit_money > borrowPlan.money_range_max)
          throw new AppException(1815, "deposit_money_is_greater_than_maximum");
      }
      string borrowType = borrowPlan.borrow_type;
      string market = borrowPlan.market;
      DateTime utcNow = DateTime.UtcNow;
      SysMarketDto sysMarketDto = SysMarketService.Find(market);
      (int multiple, int borrow_duration, Decimal deposit_money, Decimal borrow_money, Decimal rate, Decimal borrow_fee) = BorrowHelper.BorrowFee(borrowPlan, req.deposit_money, req.multiple, req.borrow_duration, sysMarketDto.currency);
      string base_symbol = ConfigLib.Get("wallet_currency");
      ExchangeHelper.GetRate(sysMarketDto.currency, base_symbol);
      Decimal change = ExchangeHelper.Convert(deposit_money + borrow_fee, sysMarketDto.currency, base_symbol);
      if (!WalletLib.IsRequestrMoneyOk(member_id, change))
        throw new AppException(1803, "insufficient_balance");
      int tradingTime = req.trading_time;
      (DateTime begin_time, DateTime end_time) = BorrowHelper.GetBeginAndEndTime(utcNow, market, tradingTime, borrow_duration, borrowType);
      if (DateTime.UtcNow.Date > begin_time.Date)
        throw new AppException(3000, "begindate_has_expired");
      string str1 = SubAccountBiz.MakeOrderId();
      using (TransactionScope transactionScope = new TransactionScope())
      {
        BorrowDto borrowDto = new BorrowDto()
        {
          sub_account = (string) null,
          borrow_plan_fk = borrowPlanPk,
          member_fk = member_id,
          order_id = str1,
          status = -1,
          market = market,
          borrow_type = borrowType,
          currency = sysMarketDto.currency,
          deposit_money = deposit_money,
          init_money = deposit_money + borrow_money,
          multiple = multiple,
          auto_renewal = borrowPlan.renewal,
          borrow_money = borrow_money,
          borrow_interest = borrow_fee,
          borrow_duration = borrow_duration,
          position = 0,
          rate = rate,
          total = 1,
          trading_time = tradingTime,
          loss_warn_sms_send = 0,
          stock_money = 0M,
          total_coupon = 0M,
          total_fee = borrow_fee,
          total_interest = borrow_fee,
          create_time = utcNow,
          begin_time = begin_time,
          end_time = end_time
        };
        borrowDto.pk = BorrowService.FindPkAfterInsert(borrowDto);
        string str2 = SubAccountBiz.BorrowApplyVerify(borrowDto, req.lang);
        transactionScope.Complete();
        return new CreateResponse() { sub_account = str2 };
      }
    }

    private static string CreateNewTradeAccount(string market)
    {
      string sub_account;
      do
      {
        sub_account = string.Format("{0}{1}", (object) market, (object) Tool.GetRandomNum(8));
      }
      while (TradeAccountService.Find(sub_account) != null);
      return sub_account;
    }

    private static void CreateTradeAccount(int orderId, string subAccount)
    {
      BorrowApply borrowApply = BorrowService.GetBorrowApply(orderId);
      MemberDto memberDto = MemberServices.Find(borrowApply.member_fk);
      Decimal num1 = borrowApply.deposit_money * (Convert.ToDecimal((object) borrowApply.warning_line) / 100M) + borrowApply.borrow_money;
      Decimal num2 = borrowApply.deposit_money * (Convert.ToDecimal((object) borrowApply.break_line) / 100M) + borrowApply.borrow_money;
      TradeAccountService.Insert(new TradeAccountDto()
      {
        sub_account = subAccount,
        member_fk = borrowApply.member_fk,
        borrow_plan_fk = borrowApply.borrow_plan_fk,
        type = memberDto.is_test_account || borrowApply.borrow_type == "trial" ? 0 : 1,
        market = borrowApply.market,
        loan_type = borrowApply.borrow_type,
        currency = borrowApply.currency,
        mem_money = borrowApply.init_money,
        frozen_money = 0M,
        margin = borrowApply.deposit_money,
        margin_float = borrowApply.deposit_money,
        loan_money = borrowApply.borrow_money,
        time_zone = borrowApply.time_zone,
        begin_time = borrowApply.begin_time,
        end_time = borrowApply.end_time,
        close_time = new DateTime?(),
        status = 0,
        warningline = num1,
        breakline = num2,
        notice_warning = new DateTime?(),
        notice_close = new DateTime?(),
        multiple = (int) borrowApply.multiple,
        borrow_duration = borrowApply.borrow_duration
      });
    }

    private static void SetDefaultTradeAccount(int id, string subAccount, string market)
    {
      MemberDto memberDto = MemberServices.Find(id);
      if (!string.IsNullOrWhiteSpace(memberDto.sub_account))
        return;
      MemberServices.UpdateSubAccount(memberDto.pk, subAccount);
      TokenModel token = TokenCatchLib.GetToken(memberDto.token);
      if (token == null)
        return;
      token.sub_account = subAccount;
      token.market = market;
      TokenCatchLib.SetToken(memberDto.token, token);
    }

    public static string BorrowApplyVerify(BorrowDto order, string lang)
    {
      string newTradeAccount = SubAccountBiz.CreateNewTradeAccount(order.market);
      string str1 = ConfigLib.Get("wallet_currency");
      string currency = order.currency;
      Decimal change1 = ExchangeLib.Convert(order.borrow_interest, currency, str1);
      Decimal change2 = ExchangeLib.Convert(order.deposit_money, currency, str1);
      Decimal rate = ExchangeLib.GetRate(currency, str1);
      DateTime utcNow = DateTime.UtcNow;
      string str2 = utcNow.ToString("yyyyMMdd") + Tool.GetRandomNum(6);
      WalletLib.NewBorrowPass(order.member_fk, change2, order.pk.ToString(), newTradeAccount, order.deposit_money, currency);
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        sub_account = newTradeAccount,
        member_fk = order.member_fk,
        sn = str2,
        currency = currency,
        create_datetime = utcNow,
        wallet_amount = change2,
        exchange = rate,
        balance = order.deposit_money,
        affect = order.deposit_money,
        op = 1,
        temp_id = 32,
        list = new object[4]
        {
          (object) order.deposit_money,
          (object) currency,
          (object) change2,
          (object) str1
        }
      }, lang);
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        sub_account = newTradeAccount,
        member_fk = order.member_fk,
        sn = str2,
        currency = currency,
        create_datetime = utcNow,
        wallet_amount = 0M,
        affect = order.borrow_money,
        exchange = 0M,
        balance = order.borrow_money + order.deposit_money,
        temp_id = 201,
        op = 1,
        list = new object[4]
        {
          (object) order.borrow_money,
          (object) currency,
          (object) 0,
          (object) str1
        }
      }, lang);
      Decimal num = 0M;
      MemberServices.GetMember(order.member_fk);
      if (order.borrow_type != "free" && order.borrow_type != "trial")
      {
        WalletLib.BorrowManagementFee(order.member_fk, change1, newTradeAccount, order.total_interest, currency);
        num = SubAccountBiz.GetBorrowUseCoupon(order.member_fk, order.borrow_interest, currency);
        Decimal change3 = ExchangeLib.Convert(num, currency, str1);
        if (num != 0M)
        {
          (bool _, WalletDto walletDto) = WalletLib.ManagementFeeUseCoupon(order.member_fk, change3, newTradeAccount, num, currency);
          (string str3, string str4) = Tool.MakeWalletRecordInfo(123, lang, new object[3]
          {
            (object) newTradeAccount,
            (object) change3,
            (object) walletDto.currency
          });
          WalletCouponRecordService.FindPkAfterInsert(new WalletCouponRecordDto()
          {
            coupon_balance = walletDto.coupon,
            member_fk = order.member_fk,
            currency = str1,
            affect = -change3,
            money_type = 2,
            type = 123,
            exchange = 1M,
            wallet_amount = walletDto.balance,
            sub_type = 0,
            info = str3,
            sended = true,
            create_time = DateTime.UtcNow,
            param = str4
          });
        }
        RecommendBusiness.Profit(BorrowFeeService.FindPkAfterInsert(new BorrowFeeDto()
        {
          member_fk = order.member_fk,
          sub_account = newTradeAccount,
          borrow_fk = order.pk,
          type = 1,
          borrow_fee = change1,
          use_coupon = change3,
          fee_received = change1 - change3,
          borrow_duration = order.borrow_duration,
          create_time = utcNow
        }));
      }
      int countWithoudTrial = TradeAccountService.GetAccountCountWithoudTrial(order.member_fk);
      SubAccountBiz.CreateTradeAccount(order.pk, newTradeAccount);
      SubAccountBiz.SetDefaultTradeAccount(order.member_fk, newTradeAccount, order.market);
      BorrowService.AcceptOrder(order.pk, newTradeAccount, num);
      object[] objArray = new object[6]
      {
        (object) order.pk,
        (object) newTradeAccount,
        (object) Tool.AddNumberSeparation(new Decimal?(order.deposit_money), currency),
        (object) currency,
        (object) Tool.AddNumberSeparation(new Decimal?(change2), str1),
        (object) str1
      };
      int temp_id = 32;
      SendMessageLib.Send(order.member_fk, temp_id, objArray);
      if (order.borrow_type != "trial" && countWithoudTrial == 0)
      {
        MemberTaskLib.MemberTaskFinish(order.member_fk, 4);
        RecommendRegisterService.UpdateFirstBorrowDate(order.member_fk, utcNow);
      }
      return newTradeAccount;
    }

    private static string MakeOrderId()
    {
      string order_id = "";
      for (bool flag = true; flag; flag = BorrowService.IsDuplicateOrderId(order_id))
        order_id = Tool.MakeRandomNo(15);
      return order_id;
    }

    public static System.Collections.Generic.List<BorrowResponse> GetBorrows(
      int member_id,
      string? status)
    {
      return SubAccountBiz.ConvertBorrowDtoToBorrowReponse(!string.IsNullOrEmpty(status) ? BorrowService.GetBorrows(member_id, Convert.ToInt32(status)) : BorrowService.GetBorrows(member_id));
    }

    private static System.Collections.Generic.List<BorrowResponse> ConvertBorrowDtoToBorrowReponse(
      System.Collections.Generic.List<BorrowDto> borrows)
    {
      System.Collections.Generic.List<BorrowResponse> borrowsResponse = new System.Collections.Generic.List<BorrowResponse>();
      borrows.ForEach((Action<BorrowDto>) (x =>
      {
        BorrowResponse borrowResponse = new BorrowResponse()
        {
          order_id = x.order_id,
          status = x.status,
          borrow_type = x.borrow_type,
          capital = x.status == -1 ? x.init_money : 0M,
          balance = x.status == -1 ? x.init_money : 0M,
          profit_and_loss = 0M,
          begin_time = x.begin_time,
          end_time = x.end_time
        };
        ViewTradeAccountDto viewTradeAccount = ViewTradeAccountService.GetViewTradeAccount(x.sub_account ?? "");
        if (viewTradeAccount != null)
        {
          Decimal num = viewTradeAccount.margin + viewTradeAccount.loan_money;
          borrowResponse.begin_time = viewTradeAccount.begin_time;
          borrowResponse.end_time = viewTradeAccount.end_time;
          borrowResponse.capital = num;
          borrowResponse.balance = viewTradeAccount.mem_money - viewTradeAccount.frozen_money;
          borrowResponse.profit_and_loss = viewTradeAccount.balance - num;
        }
        borrowsResponse.Add(borrowResponse);
      }));
      return borrowsResponse;
    }

    public static void BalanceTransfer(TokenModel tokenModel, string sub_account, string lang)
    {
      int memberFk = tokenModel.member_fk;
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(sub_account);
      if (tradeAccountDto.status != 2)
        throw new AppException(1812, "balance_transfer_is_not_allowed");
      Decimal amount = Math.Max(tradeAccountDto.mem_money - tradeAccountDto.loan_money, 0M);
      WalletDto walletDto = WalletService.Find(memberFk);
      MemberDto member = MemberServices.GetMember(memberFk);
      Decimal rate = ExchangeHelper.GetRate(tradeAccountDto.currency, walletDto.currency);
      if ("trial".Equals(tradeAccountDto.loan_type))
      {
        if (amount > tradeAccountDto.margin)
        {
          Decimal change = ExchangeHelper.Convert(amount - tradeAccountDto.margin, tradeAccountDto.currency, walletDto.currency);
          WalletLib.PromotRewardCoupon(memberFk, change);
          BorrowPlanDto borrowPlan = BorrowPlanService.GetBorrowPlan(tradeAccountDto.borrow_plan_fk);
          (string str1, string str2) = Tool.MakeWalletRecordInfo(23, member.lang, new object[3]
          {
            (object) borrowPlan.name,
            (object) change,
            (object) walletDto.currency
          });
          WalletCouponRecordService.FindPkAfterInsert(new WalletCouponRecordDto()
          {
            coupon_balance = walletDto.coupon + change,
            member_fk = memberFk,
            currency = tradeAccountDto.currency,
            affect = change,
            money_type = 2,
            type = 23,
            sub_type = 23,
            info = str1,
            sended = true,
            create_time = DateTime.UtcNow,
            param = str2
          });
        }
        amount = tradeAccountDto.margin;
      }
      else if ("free".Equals(tradeAccountDto.loan_type))
      {
        Decimal num1 = amount - tradeAccountDto.margin;
        if (num1 > 0M)
        {
          Decimal num2 = Convert.ToDecimal(BorrowPlanService.GetBorrowPlan(tradeAccountDto.borrow_plan_fk).unique_set.Split('|')[2]);
          Decimal num3 = Tool.Round(num1 * num2 / 100M, tradeAccountDto.currency);
          amount = tradeAccountDto.margin + num3;
          AdminConfigDto adminConfigDto = tradeapi.Services.AdminConfigService.Find("interest_free_profit_sharing");
          if (adminConfigDto != null && adminConfigDto.value == "1")
          {
            Decimal num4 = ExchangeHelper.Convert(num1 - num3, tradeAccountDto.currency, walletDto.currency);
            BorrowDto borrow = BorrowService.GetBorrow(sub_account);
            RecommendBusiness.Profit(BorrowFeeService.FindPkAfterInsert(new BorrowFeeDto()
            {
              member_fk = memberFk,
              sub_account = sub_account,
              borrow_fk = borrow.pk,
              type = 3,
              borrow_fee = num4,
              use_coupon = 0M,
              fee_received = num4,
              borrow_duration = 0,
              create_time = DateTime.UtcNow
            }));
          }
        }
      }
      TradeAccountService.CloseTradeAccount(sub_account);
      Decimal change1 = ExchangeHelper.Convert(amount, tradeAccountDto.currency, walletDto.currency);
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        member_fk = memberFk,
        sub_account = sub_account,
        temp_id = 40,
        sn = SubAccountBiz.MakeTradeMoneyRecordSn(),
        currency = tradeAccountDto.currency,
        affect = -amount,
        balance = tradeAccountDto.mem_money - amount,
        exchange = rate,
        op = -1,
        wallet_amount = -change1,
        reviewer = member.nickname,
        list = new object[4]
        {
          (object) amount,
          (object) tradeAccountDto.currency,
          (object) change1,
          (object) walletDto.currency
        },
        create_datetime = DateTime.UtcNow
      }, lang);
      WalletLib.BorrowSettle(memberFk, change1);
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = memberFk,
        type = 40,
        subtype = 2,
        currency = walletDto.currency,
        temp_id = 40,
        affect = change1,
        coupon = 0M,
        balance = walletDto.balance + change1,
        list = new object[5]
        {
          (object) tradeAccountDto.sub_account,
          (object) change1,
          (object) walletDto.currency,
          (object) amount,
          (object) tradeAccountDto.currency
        },
        createtime = DateTime.UtcNow,
        create_ip = tokenModel.ip
      });
      string str = SubAccountBiz.MakeTradeMoneyRecordSn();
      TradeRecordLib.Save(new TradeMoneyRecoreRequest()
      {
        member_fk = memberFk,
        sub_account = sub_account,
        temp_id = 202,
        sn = str,
        currency = tradeAccountDto.currency,
        affect = -(tradeAccountDto.mem_money - amount),
        balance = 0M,
        exchange = rate,
        op = -1,
        wallet_amount = 0M,
        reviewer = member.nickname,
        list = new object[4]
        {
          (object) (tradeAccountDto.mem_money - amount),
          (object) tradeAccountDto.currency,
          (object) ExchangeHelper.Convert(tradeAccountDto.mem_money - amount, tradeAccountDto.currency, walletDto.currency),
          (object) walletDto.currency
        },
        create_datetime = DateTime.UtcNow
      }, lang);
      SendMessageLib.Send(memberFk, 40, (object) str, (object) sub_account, (object) (tradeAccountDto.mem_money - amount), (object) tradeAccountDto.currency, (object) ExchangeHelper.Convert(tradeAccountDto.mem_money - amount, tradeAccountDto.currency, walletDto.currency), (object) walletDto.currency);
    }

    private static string MakeTradeMoneyRecordSn()
    {
      string sn = "";
      for (bool flag = true; flag; flag = TradeMoneyRecordService.IsDuplicateSN(sn))
        sn = Tool.MakeRandomNo(20);
      return sn;
    }

    private static Decimal GetBorrowUseCoupon(
      int memberPk,
      Decimal borrow_fee,
      string market_currency)
    {
      Decimal borrowUseCoupon = 0M;
      if (Tool.ToBool(tradeapi.Services.AdminConfigService.Find("management_fee_enable_coupon_use").value))
      {
        Decimal num1 = Decimal.Parse(tradeapi.Services.AdminConfigService.Find("coupon_use_rate").value);
        borrowUseCoupon = borrow_fee * num1;
        Decimal num2 = ExchangeHelper.Convert(WalletService.GetCoupon(memberPk), ConfigLib.Get("wallet_currency"), market_currency);
        if (num2 < borrowUseCoupon)
          borrowUseCoupon = num2;
      }
      return borrowUseCoupon;
    }
  }
}
