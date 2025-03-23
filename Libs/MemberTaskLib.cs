// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.MemberTaskLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using tradeapi.Common;
using tradeapi.Models.Dto;
using tradeapi.Services;
using tradeapi.Utility;

#nullable disable
namespace tradeapi.Libs
{
  public class MemberTaskLib
  {
    public static bool MemberTaskFinish(int member_pk, int sub_type)
    {
      string currency = ConfigLib.Get("wallet_currency");
      MemberDto memberDto = MemberServices.Find(member_pk);
      if (currency == "")
        throw new AppException(2402, "missing_wallet_currency");
      MemberTaskDto typeCurrencyLang1 = MemberTaskService.FindBySubType_Currency_Lang(sub_type, memberDto.lang, currency);
      if (typeCurrencyLang1 == null || typeCurrencyLang1.currency.ToLower() != currency.ToLower())
        throw new AppException(2401, "missing_membertask_config");
      MutilangSubjectDto adminDefault = MutilangSubjectService.FindAdminDefault();
      MemberTaskDto typeCurrencyLang2 = MemberTaskService.FindBySubType_Currency_Lang(sub_type, adminDefault.lang, currency);
      if (typeCurrencyLang2 == null)
        throw new AppException(2400, "missing_membertask_cn_config");
      if (WalletCouponRecordService.FindByMemberFK_Type_SubType(member_pk, 21, sub_type) != null)
        return false;
      WalletDto walletDto1 = WalletService.Find(member_pk);
      if (walletDto1 == null)
        throw new AppException(2403, "missing_wallet");
      if (walletDto1.currency.ToLower() != currency.ToLower())
        throw new AppException(2404, "wallet_currency_mismatch");
      string str1 = (WalletTemplateService.GetByTempId(21, memberDto.lang) ?? throw new AppException(2405, "missing_wallet_template")).template.Replace("#0#", typeCurrencyLang1.title).Replace("#1#", typeCurrencyLang1.coin.ToString());
      string str2 = typeCurrencyLang2.title + "|" + typeCurrencyLang1.coin.ToString();
      bool flag = WalletLib.RewardNoviceQuests(member_pk, typeCurrencyLang1.coin);
      WalletDto walletDto2 = WalletService.Find(member_pk);
      WalletCouponRecordService.FindPkAfterInsert(new WalletCouponRecordDto()
      {
        coupon_balance = walletDto2.coupon,
        member_fk = member_pk,
        currency = walletDto2.currency,
        affect = typeCurrencyLang1.coin,
        money_type = 2,
        type = 21,
        sub_type = sub_type,
        info = str1,
        sended = true,
        create_time = DateTime.UtcNow,
        param = str2
      });
      object[] objArray = new object[3]
      {
        (object) Tool.AddNumberSeparation(new Decimal?(typeCurrencyLang1.coin), walletDto2.currency),
        (object) walletDto2.currency,
        (object) typeCurrencyLang1.title
      };
      SendMessageLib.Send(member_pk, 112, objArray);
      return flag;
    }
  }
}
