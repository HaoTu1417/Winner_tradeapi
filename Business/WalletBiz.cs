// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.WalletBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using DB.Services;
using Microsoft.Extensions.Configuration;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdParty.App;
using ThirdParty.Models;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.ThirdPay;
using tradeapi.Models.Wallet;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class WalletBiz
  {
    private static string GenPayOrderId()
    {
      return "RT" + DateTime.Now.ToString("yyyyMMddhhmmss") + (new Random().Next() % 1000).ToString("000");
    }

    public static Dictionary<string, List<string>> GetSupportCurrency()
    {
      Dictionary<string, List<string>> supportCurrency = new Dictionary<string, List<string>>();
      for (int type = 1; type <= 2; ++type)
        supportCurrency[type.ToString()] = WithdrawSupportCurrencyService.FindSupportCurrency(type);
      return supportCurrency;
    }

    public static WalletResponse GetWalletById(int memberId)
    {
      WalletResponse walletResponse = WalletService.FindWalletResponse(memberId);
      walletResponse.currency = ConfigLib.Get("wallet_currency");
      walletResponse.account = Tool.ReplaceWithSpecialChar(walletResponse.account, 3, 4);
      int unreadMessageCount = MessageServices.GetUnreadMessageCount(1, memberId);
      int unreadPublicCount = MessageServices.GetUnreadPublicCount(memberId);
      walletResponse.total_unread = unreadMessageCount + unreadPublicCount;
      return walletResponse;
    }

    public static MarketWalletResponse GetMarketWalletById(int memberId, string market)
    {
      if (string.IsNullOrEmpty(market))
        throw new AppException(1417, "unkown_market");
      MarketWalletResponse marketWalletById = new MarketWalletResponse();
      WalletResponse walletResponse = WalletService.FindWalletResponse(memberId);
      marketWalletById.currency = Tool.GetCurrencyByMarket(market);
      marketWalletById.balance = ExchangeHelper.Convert(walletResponse.balance, ConfigLib.Get("wallet_currency"), marketWalletById.currency);
      marketWalletById.available = ExchangeHelper.Convert(walletResponse.available, ConfigLib.Get("wallet_currency"), marketWalletById.currency);
      return marketWalletById;
    }

    public static List<GetBankCardResponse> GetBankCard(int member_fk)
    {
      List<GetBankCardResponse> bankCard = MemberBankService.Find(member_fk);
      foreach (GetBankCardResponse bankCardResponse in bankCard)
      {
        bankCardResponse.card = Tool.ReplaceWithSpecialChar(bankCardResponse.card);
        bankCardResponse.exchange = ExchangeHelper.GetRate(ConfigLib.Get("wallet_currency"), bankCardResponse.currency);
      }
      return bankCard;
    }

    public static async void AddBankCard(AddBankCardRequest req, int member_fk, string ip)
    {
      DateTime utcNow = DateTime.UtcNow;
      string str = "C" + utcNow.ToString("yyyyMMddhhmmss") + (new Random().Next() % 1000).ToString("000");
      MemberBankService.Insert(new MemberBankDto()
      {
        member_fk = member_fk,
        card_pk = str,
        currency = req.currency,
        card_type = req.card_type,
        country = req.country,
        bank = req.bank_name,
        branch = req.bank_branch,
        card = req.card,
        account = req.name,
        is_delete = false,
        is_confirm = true,
        create_ip = ip,
        create_time = utcNow
      });
    }

    public static List<GetBankCardTypeResponse> GetBankCardType(string lang, int member_fk)
    {
      return MemberBankService.GetCardType(member_fk);
    }

    public static List<GetBankCardTypeResponse> GetAdminBankCardType(string lang)
    {
      List<GetBankCardTypeResponse> cardType1 = AdminBankService.GetCardType(lang);
      List<GetBankCardTypeResponse> cardType2 = tradeapi.Services.WalletPaymentService.GetCardType();
      foreach (GetBankCardTypeResponse cardTypeResponse in cardType2)
        cardTypeResponse.card_type = 3;
      cardType2.AddRange((IEnumerable<GetBankCardTypeResponse>) cardType1);
      return cardType2.OrderBy<GetBankCardTypeResponse, int>((Func<GetBankCardTypeResponse, int>) (getBankCardTypeResponse => getBankCardTypeResponse.card_type)).ToList<GetBankCardTypeResponse>();
    }

    public static GetLastWithdrawResponse GetLastWithdrawById(TokenModel tokenModel)
    {
      return WalletWithdrawService.FindLastWithdraw(tokenModel.member_fk);
    }

    public static WithdrawApplyResponse GetWithdrawApply(
      WithdrawApplyRequest req,
      string ip,
      int member_fk)
    {
      if (WalletWithdrawService.FindWaitWithdrawCount(member_fk) > 0)
        throw new AppException(1180, "already_reviewing");
      DateTime utcNow = DateTime.UtcNow;
      string formId = "wd" + utcNow.ToString("yyyyMMddhhmmss") + (new Random().Next() % 1000).ToString("000");
      MemberBankDto memberBankDto = MemberBankService.Find(req.card_pk);
      Decimal amount = req.amount;
      WalletWithdrawDto source = new WalletWithdrawDto()
      {
        member_bank_fk = req.card_pk,
        member_fk = member_fk,
        order_no = formId,
        wallet_amount = req.amount,
        exchange = ExchangeHelper.GetRate(ConfigLib.Get("wallet_currency"), req.currency),
        currency = req.currency,
        money = amount,
        fee = 0M,
        status = 0,
        create_time = utcNow,
        create_ip = ip,
        id_selfie = req.id_selfie
      };
      if (!WalletLib.WithdrawSubmit(member_fk, amount, formId))
        throw new AppException(2380, "withdraw_submit_failed");
      WalletWithdrawService.FindPkAfterInsert(source);
      object[] objArray = new object[3]
      {
        (object) source.order_no,
        (object) Tool.AddNumberSeparation(new Decimal?(source.money), source.currency),
        (object) source.currency
      };
      SendMessageLib.Send(source.member_fk, 102, objArray);
      return new WithdrawApplyResponse()
      {
        order_no = formId,
        request_time = utcNow,
        card = Tool.ReplaceWithSpecialChar(memberBankDto.card),
        amount = req.amount
      };
    }

    public static bool CancelWithdrawById(int member_fk, WithdrawCancelRequest req)
    {
      WalletWithdrawDto walletWithdrawDto = WalletWithdrawService.Find(req.order_no);
      WalletService.FreezeMoneyChange(walletWithdrawDto.member_fk, -walletWithdrawDto.wallet_amount);
      if (WalletWithdrawService.CancelWithdraw(member_fk, req.order_no) != 1)
        throw new AppException(2384, "withdraw_cancel_failed");
      return true;
    }

    public static void GetDepositApply(WalletRecordRequest req) => WalletRecordLib.Save(req);

    public static List<GetRecordHistoryResponse> GetRecordHistory(TokenModel tokenModel)
    {
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      return WalletRecordService.FindAll(tokenModel.member_fk, memberDto.lang);
    }

    public static List<GetRecordHistoryResponse> GetTradeHistory(TokenModel tokenModel)
    {
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      return WalletRecordService.FindTradeHistory(tokenModel.member_fk, memberDto.lang);
    }

    public static List<GetRecordHistoryResponse> GetDepositHistory(TokenModel tokenModel)
    {
      int buyFundTempId = 71;
      int sellFundTempId = 72;
      int fundInterest = 73;
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      return WalletRecordService.FindAllRichBoxRecord(tokenModel.member_fk, memberDto.lang, buyFundTempId, sellFundTempId, fundInterest);
    }

    public static List<WithdrawHistoryResponse> GetWithdrawHistory(TokenModel tokenModel)
    {
      List<WithdrawHistoryResponse> history = WalletWithdrawService.FindHistory(tokenModel.member_fk);
      foreach (WithdrawHistoryResponse withdrawHistoryResponse in history)
        withdrawHistoryResponse.currency = ConfigLib.Get("wallet_currency");
      return history;
    }

    public static List<RechargeHistoryResponse> GetRechargeHistory(TokenModel tokenModel)
    {
      List<RechargeHistoryResponse> history = WalletRechargeService.FindHistory(tokenModel.member_fk);
      foreach (RechargeHistoryResponse rechargeHistoryResponse in history)
        rechargeHistoryResponse.wallet_currency = ConfigLib.Get("wallet_currency");
      return history;
    }

    public static List<FreezehistoryResponse> GetFreezeHistory(TokenModel tokenModel)
    {
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      List<FreezehistoryResponse> freezeHistory = WalletFreezeService.FindFreezeHistory(tokenModel.member_fk, memberDto.lang);
      foreach (FreezehistoryResponse freezehistoryResponse in freezeHistory)
        freezehistoryResponse.currency = ConfigLib.Get("wallet_currency");
      return freezeHistory;
    }

    public static List<CouponrecordResponse> GetCouponrecord(TokenModel tokenModel)
    {
      MemberDto memberDto = MemberServices.Find(tokenModel.member_fk);
      return WalletRecordService.FindAllCoupon(tokenModel.member_fk, memberDto.lang);
    }

    public static async Task<ThirdPayRechargeResponse> ThirtyPay(
      int member_fk,
      ThirdPayRechargeRequest req,
      string ip,
      IConfiguration config)
    {
      ThirdPayRechargeResponse rechargeResponse1;
      try
      {
        string str1 = config.GetValue<string>("ConnectionStrings:ReadConnectionString");
        string str2 = config.GetValue<string>("ConnectionStrings:WriteConnectionString");
        DateTime createTime = DateTime.UtcNow;
        MemberDto member = MemberServices.Find(member_fk);
        tradeapi.Models.Dto.WalletPaymentDto third_party = tradeapi.Services.WalletPaymentService.FindByPayCode(req.pay_code);
        string order_no = WalletBiz.GenPayOrderId();
        PayService payService = new PayService();
        PartyRechargeRequest req1 = new PartyRechargeRequest();
        req1.member_id = member_fk;
        req1.order_no = order_no;
        req1.pay_code = req.pay_code;
        req1.currency = req.currency;
        req1.amount = req.amount;
        string read_string = str1;
        string write_string = str2;
        PartyRechargeResponse rechargeResponse2 = await payService.PartyRecharge(req1, read_string, write_string);
        if (!rechargeResponse2.result)
          throw new AppException(2393, "order_pay_get_url_fail");
        ThirdPayRechargeResponse rechargeResponse3 = new ThirdPayRechargeResponse()
        {
          pay_url = rechargeResponse2.pay_url,
          validity_period = rechargeResponse2.create_time.AddMinutes(5.0)
        };
        WalletRechargeDto recharge = new WalletRechargeDto()
        {
          type = "third_party",
          member_fk = member_fk,
          admin_bank_fk = third_party.pk,
          order_no = order_no,
          currency = req.currency,
          money = req.amount,
          exchange = ExchangeHelper.GetRate(req.currency, ConfigLib.Get("wallet_currency")),
          wallet_amount = ExchangeHelper.Convert(req.amount, req.currency, ConfigLib.Get("wallet_currency")),
          fee = 0M,
          create_time = createTime,
          create_ip = ip,
          line_bank = third_party.pay_name,
          form_name = member.account,
          status = 0,
          platform_order_no = rechargeResponse2.order_no
        };
        if (WalletLib.RechargeSubmit(recharge))
        {
          object[] objArray = new object[4]
          {
            (object) recharge.order_no,
            (object) Tool.AddNumberSeparation(new Decimal?(recharge.money), recharge.currency),
            (object) recharge.currency,
            (object) recharge.exchange
          };
          SendMessageLib.Send(recharge.member_fk, 101, objArray);
        }
        rechargeResponse1 = rechargeResponse3;
      }
      catch (AppException ex)
      {
        throw new AppException(ex.GetStatus(), ex.Message);
      }
      catch (Exception ex)
      {
        LogLib.Log("[WalletBiz][ThirtyPay]" + ex.Message);
        throw new AppException(2310, "invlid_recharge_method");
      }
      return rechargeResponse1;
    }

    public static string RechargeApply(TokenModel tokenModel, RechargeapplyRequest req, string ip)
    {
      DateTime utcNow = DateTime.UtcNow;
      string str = WalletBiz.GenPayOrderId();
      MemberDto member = MemberServices.GetMember(tokenModel.member_fk);
      int pkByAccountNumber = AdminBankService.FindPkByAccountNumber(req.AccountNumber);
      string accountNumber = req.AccountNumber;
      WalletRechargeDto recharge = new WalletRechargeDto()
      {
        type = req.RechargeMethod,
        member_fk = tokenModel.member_fk,
        admin_bank_fk = pkByAccountNumber,
        order_no = str,
        currency = req.Currency,
        money = req.Amount,
        exchange = req.ExchangeRate,
        wallet_amount = ExchangeHelper.Convert(req.Amount, req.Currency, ConfigLib.Get("wallet_currency")),
        fee = 0M,
        create_time = utcNow,
        create_ip = ip,
        line_bank = accountNumber,
        form_name = member.account,
        status = 0,
        last_five = req.WalletLast5Digits
      };
      if (WalletLib.RechargeSubmit(recharge))
      {
        object[] objArray = new object[4]
        {
          (object) recharge.order_no,
          (object) Tool.AddNumberSeparation(new Decimal?(recharge.money), recharge.currency),
          (object) recharge.currency,
          (object) recharge.exchange
        };
        SendMessageLib.Send(recharge.member_fk, 101, objArray);
      }
      return str;
    }

    public static string RechargeApplyJPPay(TokenModel tokenModel, RechargeapplyRequest req, string ip)
    {
      DateTime utcNow = DateTime.UtcNow;
      string str = WalletBiz.GenPayOrderId();
      MemberDto member = MemberServices.GetMember(tokenModel.member_fk);
      // tìm pk của tài khoản nhận tiền
      int pkByAccountNumber = AdminBankService.FindPkByAccountNumber(req.AccountNumber);
      string accountNumber = req.AccountNumber;
      WalletRechargeDto recharge = new WalletRechargeDto()
      {
        type = req.RechargeMethod,
        member_fk = tokenModel.member_fk,
        admin_bank_fk = pkByAccountNumber,
        order_no = str,
        currency = req.Currency,
        money = req.Amount,
        exchange = req.ExchangeRate,
        // wallet_amount = ExchangeHelper.Convert(req.Amount, """, ConfigLib.Get("wallet_currency")),
        fee = 0M,
        create_time = utcNow,
        create_ip = ip,
        line_bank = accountNumber,
        form_name = member.account,
        status = 0,
        last_five = req.WalletLast5Digits
      };
      if (WalletLib.RechargeSubmit(recharge))
      {
        object[] objArray = new object[4]
        {
          (object) recharge.order_no,
          (object) Tool.AddNumberSeparation(new Decimal?(recharge.money), recharge.currency),
          (object) recharge.currency,
          (object) recharge.exchange
        };
        SendMessageLib.Send(recharge.member_fk, 101, objArray);
      }
      return str;
    }
    
    
    public static List<CollectInfoResponse> GetCollectInfo(string lang)
    {
      List<CollectInfoResponse> activeAccount = AdminBankService.FindActiveAccount();
      foreach (CollectInfoResponse collectInfoResponse in activeAccount)
      {
        collectInfoResponse.exchange_rate = ExchangeHelper.GetRate(collectInfoResponse.currency, ConfigLib.Get("wallet_currency"));
        collectInfoResponse.fastbtns = collectInfoResponse.fastbtn != null ? ((IEnumerable<string>) collectInfoResponse.fastbtn.Split(",")).Select<string, string>((Func<string, string>) (x => x = x.Trim())).AsList<string>() : new List<string>();
      }
      List<CollectInfoResponse> collectInfo = tradeapi.Services.WalletPaymentService.GetCollectInfo();
      foreach (CollectInfoResponse collectInfoResponse in collectInfo)
      {
        collectInfoResponse.type = 3;
        collectInfoResponse.fastbtns = collectInfoResponse.fastbtn != null ? ((IEnumerable<string>) collectInfoResponse.fastbtn.Split(",")).Select<string, string>((Func<string, string>) (x => x = x.Trim())).AsList<string>() : new List<string>();
        collectInfoResponse.exchange_rate = ExchangeHelper.GetRate(collectInfoResponse.currency, ConfigLib.Get("wallet_currency"));
      }
      activeAccount.AddRange((IEnumerable<CollectInfoResponse>) collectInfo);
      return activeAccount;
    }

    public static List<WithdrawInfoResponse> GetWithdrawInfo(int member_fk)
    {
      List<WalletDto> wallet = WalletService.FindWallet(member_fk);
      List<WithdrawInfoResponse> withdrawInfo = new List<WithdrawInfoResponse>();
      foreach (WalletDto walletDto in wallet)
      {
        foreach (MoneyDailyExchangeDto dailyExchangeDto in MoneyDailyExchangeService.GetExchange(walletDto.currency))
          withdrawInfo.Add(new WithdrawInfoResponse()
          {
            currency = walletDto.currency,
            balance = walletDto.balance,
            exchange_currency = dailyExchangeDto.currency_symbol,
            rate = dailyExchangeDto.outward_rate
          });
      }
      return withdrawInfo;
    }

    public static string[] GetRsaKey() => new PayService().GenRsaKey();

    public static void UpdateRechargeApplyCount()
    {
      int rechargeApplyCount = WalletRechargeService.GetRechargeApplyCount();
      CacheQuery.SelectDB(CacheEnum.admin);
      HomeVm redisValue = CacheQuery.StringGet<HomeVm>("HomeVm");
      redisValue.RechargeApplyCount = rechargeApplyCount.ToString();
      CacheQuery.StringSet<HomeVm>("HomeVm", redisValue);
    }

    public static void UpdateRechargeNeedVerify()
    {
      int rechargeNeedVerify = WalletRechargeService.GetRechargeNeedVerify();
      CacheQuery.SelectDB(CacheEnum.admin);
      HomeVm redisValue = CacheQuery.StringGet<HomeVm>("HomeVm");
      redisValue.RechargeNeedVerify = (Decimal) rechargeNeedVerify;
      CacheQuery.StringSet<HomeVm>("HomeVm", redisValue);
    }

    public static void UpdateWithdrawApplyCount()
    {
      int withdrawApplyCount = WalletWithdrawService.GetWithdrawApplyCount();
      CacheQuery.SelectDB(CacheEnum.admin);
      HomeVm redisValue = CacheQuery.StringGet<HomeVm>("HomeVm");
      redisValue.WithdrawApplyCount = withdrawApplyCount.ToString();
      CacheQuery.StringSet<HomeVm>("HomeVm", redisValue);
    }

    public static void UpdateWithdrawNeedVerify()
    {
      Decimal withdrawNeedVerify = WalletWithdrawService.GetWithdrawNeedVerify();
      CacheQuery.SelectDB(CacheEnum.admin);
      HomeVm redisValue = CacheQuery.StringGet<HomeVm>("HomeVm");
      redisValue.WithdrawNeedVerify = withdrawNeedVerify;
      CacheQuery.StringSet<HomeVm>("HomeVm", redisValue);
    }
  }
}
