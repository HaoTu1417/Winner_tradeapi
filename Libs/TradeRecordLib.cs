// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.TradeRecordLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Models.Dto;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Libs
{
  public class TradeRecordLib
  {
    public static void Save(TradeMoneyRecoreRequest request, string lang = "")
    {
      MemberDto memberDto = MemberServices.Find(request.member_fk);
      if (lang == "")
        lang = memberDto.lang;
      TradeTemplateDto byTempId = TradeTemplateService.GetByTempId(request.temp_id, lang);
      if (byTempId == null)
        throw new AppException(2405, "missing_wallet_template");
      string str1 = byTempId?.template ?? "";
      string str2 = string.Join("|", ((IEnumerable<object>) request.list).Select<object, string>((Func<object, string>) (item => item.ToString())));
      for (int index = 0; index < request.list.Length; ++index)
      {
        string str3 = str1;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
        interpolatedStringHandler.AppendLiteral("#");
        interpolatedStringHandler.AppendFormatted<int>(index);
        interpolatedStringHandler.AppendLiteral("#");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        string newValue = Convert.ToString(request.list[index]);
        str1 = str3.Replace(stringAndClear, newValue);
      }
      TradeMoneyRecordService.Insert(new TradeMoneyRecordDto()
      {
        member_fk = request.member_fk,
        sub_account = request.sub_account,
        sn = request.sn,
        temp_id = request.temp_id,
        currency = request.currency,
        affect = request.affect,
        op = request.op,
        exchange = request.exchange,
        balance = request.balance,
        wallet_amount = request.wallet_amount,
        info = str1,
        param = str2,
        reviewer = request.reviewer,
        create_datetime = request.create_datetime
      });
    }
  }
}
