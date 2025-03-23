// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.WalletRecordLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using tradeapi.Models.Dto;
using tradeapi.Models.Wallet;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Libs
{
    public class WalletRecordLib
    {
        public static void Save(WalletRecordRequest req)
        {
            MemberDto memberDto = MemberServices.Find(req.member_pk);
            (string str1, string str2) = Tool.MakeWalletRecordInfo(req.temp_id, memberDto.lang, req.list);
            WalletRecordService.FindPkAfterInsert(new WalletRecordDto()
            {
                member_fk = req.member_pk,
                type = req.type,
                currency = req.currency,
                affect = req.affect,
                balance = req.balance,
                coupon = req.coupon,
                param = str2,
                templat_id = req.temp_id,
                info = str1,
                create_time = req.createtime,
                create_ip = req.create_ip ?? ""
            });
        }
    }
}