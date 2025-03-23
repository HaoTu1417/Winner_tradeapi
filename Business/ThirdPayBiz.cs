// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.ThirdPayBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.ThirdPay;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class ThirdPayBiz
  {
    public static ThirdPayListResponse GetThirdPaySupportList()
    {
      List<ThirdPayType> list = WalletPaymentService.FindAll().Select<WalletPaymentDto, ThirdPayType>((Func<WalletPaymentDto, ThirdPayType>) (thirdPayDto => new ThirdPayType()
      {
        pay_name = thirdPayDto.pay_name,
        pay_code = thirdPayDto.pay_code,
        pay_type = thirdPayDto.pay_type
      })).ToList<ThirdPayType>();
      return new ThirdPayListResponse()
      {
        thirdpay_list = list
      };
    }

    public static void RechargeNotify(RechargeNotifyRequest req)
    {
      WalletRechargeDto walletRechargeDto = WalletRechargeService.Find(req.mer_order_no);
      string merOrderNo = req.mer_order_no;
      if (walletRechargeDto == null || walletRechargeDto.status != 0)
        return;
      if (req.status == "SUCCESS")
      {
        WalletLib.RechargePass(walletRechargeDto.member_fk, walletRechargeDto.wallet_amount, walletRechargeDto.order_no, walletRechargeDto.money, walletRechargeDto.currency);
        WalletRechargeService.AccecptRecharge(merOrderNo);
        MemberTaskLib.MemberTaskFinish(walletRechargeDto.member_fk, 3);
        object[] objArray = new object[4]
        {
          (object) walletRechargeDto.order_no,
          (object) Tool.AddNumberSeparation(new Decimal?(walletRechargeDto.wallet_amount), ConfigLib.Get("wallet_currency")),
          (object) ConfigLib.Get("wallet_currency"),
          (object) walletRechargeDto.exchange
        };
        SendMessageLib.Send(walletRechargeDto.member_fk, 1, objArray);
      }
      else
      {
        if (!(req.status == "FAIL"))
          return;
        string errMsg = req.err_msg;
        WalletRechargeService.RejectRecharge(merOrderNo, errMsg);
        object[] objArray = new object[5]
        {
          (object) walletRechargeDto.order_no,
          (object) Tool.AddNumberSeparation(new Decimal?(walletRechargeDto.wallet_amount), ConfigLib.Get("wallet_currency")),
          (object) ConfigLib.Get("wallet_currency"),
          (object) walletRechargeDto.exchange,
          (object) errMsg
        };
        SendMessageLib.Send(walletRechargeDto.member_fk, 2, objArray);
      }
    }

    public static int PaySuccess(PayStatusRequest req)
    {
      return (WalletRechargeService.Find(req.order_no) ?? throw new AppException(1700, "order_does_not_exist")).status;
    }
  }
}
