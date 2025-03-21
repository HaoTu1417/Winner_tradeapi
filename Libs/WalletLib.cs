// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.WalletLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.Wallet;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Libs
{
  public class WalletLib
  {
    public static (WalletDto, bool) TestWallet(int member)
    {
      WalletDto walletDto = WalletService.Find(member);
      if (walletDto == null)
        throw new AppException(3061, "wallet_balance_error");
      if (walletDto.balance < 0M)
        throw new AppException(3061, "wallet_balance_error");
      if (!walletDto.status)
        return (walletDto, false);
      if (!WalletService.DebugBalance(member))
        throw new AppException(3061, "wallet_balance_error");
      return (walletDto, true);
    }

    public static bool RechargePass(
      int member,
      Decimal change,
      string id,
      Decimal org_money,
      string org_currency)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto walletDto1 = tuple.Item1;
      if (!tuple.Item2)
        return false;
      WalletService.BalanceMoneyChange(member, change);
      WalletDto walletDto2 = WalletService.Find(member);
      object[] objArray = new object[5]
      {
        (object) id,
        (object) change,
        (object) ConfigLib.Get("wallet_currency"),
        (object) org_money,
        (object) org_currency
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 1,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 1,
        affect = change,
        balance = walletDto2.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool RewardNoviceQuests(int member, Decimal change)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, 0M))
        return false;
      WalletService.GmoneyMoneyChange(member, change);
      return true;
    }

    public static bool PromotRewardCoupon(int member, Decimal change)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto walletDto = tuple.Item1;
      if (!tuple.Item2)
        return false;
      WalletService.GmoneyMoneyChange(member, change);
      return true;
    }

    public static bool DirectSellingPass(int member_fk, Decimal change, int yy, int mm)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member_fk);
      WalletDto walletDto1 = tuple.Item1;
      if (!tuple.Item2)
        return false;
      WalletService.BalanceMoneyChange(member_fk, change);
      string str = ConfigLib.Get("wallet_currency");
      object[] objArray = new object[4]
      {
        (object) yy,
        (object) mm,
        (object) change,
        (object) str
      };
      WalletDto walletDto2 = WalletService.Find(member_fk);
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member_fk,
        type = 25,
        currency = str,
        temp_id = 25,
        affect = change,
        balance = walletDto2.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool NewBorrowPass(
      int member,
      Decimal change,
      string id,
      string account,
      Decimal org_money,
      string org_currency)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, change))
        return false;
      WalletService.BalanceMoneyChange(member, -change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[6]
      {
        (object) id,
        (object) account,
        (object) change,
        (object) ConfigLib.Get("wallet_currency"),
        (object) org_money,
        (object) org_currency
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 32,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 32,
        affect = -change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool BorrowManagementFee(
      int member,
      Decimal change,
      string account,
      Decimal org_money,
      string org_currency)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, change))
        return false;
      WalletService.BalanceMoneyChange(member, -change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[5]
      {
        (object) account,
        (object) change,
        (object) ConfigLib.Get("wallet_currency"),
        (object) org_money,
        (object) org_currency
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 34,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 34,
        affect = -change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool MaginCallPass(
      int member,
      Decimal change,
      string id,
      string account,
      Decimal org_money,
      string org_currency)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, change))
        return false;
      WalletService.BalanceMoneyChange(member, -change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[6]
      {
        (object) id,
        (object) account,
        (object) change,
        (object) ConfigLib.Get("wallet_currency"),
        (object) org_money,
        (object) org_currency
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 38,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 38,
        affect = -change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool BorrowSettle(int member, Decimal change)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, 0M))
        return false;
      WalletService.BalanceMoneyChange(member, change);
      return true;
    }

    public static bool BorrowRenewalApply(int member, Decimal change, string id)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, change))
        return false;
      WalletLib.FreezeMoney(member, change, id, 41);
      WalletService.FreezeMoneyChange(member, change);
      return true;
    }

    public static bool ExpandBorrowPass(
      int member,
      Decimal change,
      string id,
      string account,
      Decimal org_money,
      string org_currency)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, change))
        return false;
      WalletService.BalanceMoneyChange(member, -change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[6]
      {
        (object) id,
        (object) account,
        (object) change,
        (object) ConfigLib.Get("wallet_currency"),
        (object) org_money,
        (object) org_currency
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 45,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 45,
        affect = -change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static (bool, WalletDto) ManagementFeeUseCoupon(
      int member,
      Decimal change,
      string account,
      Decimal org_money,
      string org_currency)
    {
      WalletService.BalanceMoneyChange(member, change);
      WalletService.GmoneyMoneyChange(member, -change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[5]
      {
        (object) account,
        (object) change,
        (object) ConfigLib.Get("wallet_currency"),
        (object) org_money,
        (object) org_currency
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 123,
        subtype = 2,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 123,
        affect = change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return (true, walletDto);
    }

    public static bool IsRequestrMoneyOk(int member, Decimal change)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto walletDto = tuple.Item1;
      return tuple.Item2 && walletDto.balance - walletDto.freeze >= change;
    }

    private static bool CheckFreeMoney(WalletDto wallet, Decimal change)
    {
      return wallet.balance - wallet.freeze >= change;
    }

    private static void FreezeMoney(int member, Decimal change, string id, int subtype)
    {
      WalletFreezeService.Insert(new WalletFreezeDto()
      {
        member_fk = member,
        sn = id,
        freeze = change,
        subtype = subtype,
        create_time = DateTime.UtcNow
      });
    }

    public static bool RechargeSubmit(WalletRechargeDto recharge)
    {
      WalletRechargeService.Insert(recharge);
      return true;
    }

    public static bool WithdrawSubmit(int member, Decimal money, string formId)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto walletDto = tuple.Item1;
      if (!tuple.Item2)
        return false;
      WalletService.FreezeMoneyChange(member, money);
      WalletFreezeService.Insert(new WalletFreezeDto()
      {
        member_fk = member,
        sn = formId,
        freeze = money,
        create_time = DateTime.UtcNow,
        subtype = 11
      });
      return true;
    }

    public static bool IsRequestCouponOk(int member, Decimal amount)
    {
      WalletDto wallet = WalletService.Find(member);
      return wallet != null && WalletLib.CheckFreeCoupon(wallet, amount);
    }

    private static bool CheckFreeCoupon(WalletDto wallet, Decimal amount)
    {
      return wallet.status && wallet.coupon >= amount;
    }

    public static bool BuyFund(int member, Decimal change)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto walletDto1 = tuple.Item1;
      if (!tuple.Item2)
        return false;
      WalletService.BalanceMoneyChange(member, -change);
      WalletService.RichBoxBalanceMoneyChange(member, change);
      WalletDto walletDto2 = WalletService.Find(member);
      object[] objArray = new object[2]
      {
        (object) change,
        (object) ConfigLib.Get("wallet_currency")
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 71,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 71,
        affect = -change,
        balance = walletDto2.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool SellFund(int member, Decimal change)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, 0M))
        return false;
      WalletService.BalanceMoneyChange(member, change);
      WalletService.RichBoxBalanceMoneyChange(member, -change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[2]
      {
        (object) change,
        (object) ConfigLib.Get("wallet_currency")
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 72,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 72,
        affect = change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool WithdrawTradePass(
      int member,
      string sn,
      Decimal change,
      string id,
      Decimal org_money,
      string org_currency)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, 0M))
        return false;
      WalletService.BalanceMoneyChange(member, change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[6]
      {
        (object) id,
        (object) sn,
        (object) change,
        (object) ConfigLib.Get("wallet_currency"),
        (object) org_money,
        (object) org_currency
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 49,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 49,
        affect = change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool ExpandBorrowManagementFee(
      int member,
      Decimal change,
      string id,
      string account)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto walletDto1 = tuple.Item1;
      if (!tuple.Item2)
        return false;
      WalletService.BalanceMoneyChange(member, -change);
      WalletDto walletDto2 = WalletService.Find(member);
      object[] objArray = new object[4]
      {
        (object) id,
        (object) account,
        (object) change,
        (object) ConfigLib.Get("wallet_currency")
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 105,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 105,
        affect = -change,
        balance = walletDto2.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }

    public static bool FundInterest(int member, Decimal change)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto walletDto = tuple.Item1;
      if (!tuple.Item2)
        return false;
      WalletService.RichBoxBalanceMoneyChange(member, change);
      WalletService.RichBoxInterestChange(member, change);
      return true;
    }

    public static bool BuyStockOption(int member, Decimal change, string stock_code)
    {
      (WalletDto, bool) tuple = WalletLib.TestWallet(member);
      WalletDto wallet = tuple.Item1;
      if (!tuple.Item2 || !WalletLib.CheckFreeMoney(wallet, change))
        return false;
      WalletService.BalanceMoneyChange(member, -change);
      WalletDto walletDto = WalletService.Find(member);
      object[] objArray = new object[3]
      {
        (object) stock_code,
        (object) change,
        (object) ConfigLib.Get("wallet_currency")
      };
      WalletRecordLib.Save(new WalletRecordRequest()
      {
        member_pk = member,
        type = 80,
        currency = ConfigLib.Get("wallet_currency"),
        temp_id = 80,
        affect = -change,
        balance = walletDto.balance,
        coupon = 0M,
        createtime = DateTime.UtcNow,
        list = objArray
      });
      return true;
    }
  }
}
