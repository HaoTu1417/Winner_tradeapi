// Decompiled with JetBrains decompiler
// Type: DB.Services.MemberNotifyServices
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
    public class MemberNotifyServices
    {
        public static int Insert(Member_notifyDto member_notifyDto)
        {
            string sql = "\nINSERT INTO `member_notify` \n(`member_fk`, `EmailNotify`, `SiteMessageNotify`, `AccountAlertNotify`, `AccountMarginCallNotify`, \n `StockTransactionNotify`, `AccountExpiryNotify`, `PromotionsNotify`, `DepositApprovedNotify`,\n `WithdrawalApprovedNotify`, `TradingAccountApprovedNotify`) \nVALUES \n(@member_fk, @EmailNotify, @SiteMessageNotify, @AccountAlertNotify, @AccountMarginCallNotify, \n @StockTransactionNotify, @AccountExpiryNotify, @PromotionsNotify, @DepositApprovedNotify, \n @WithdrawalApprovedNotify, @TradingAccountApprovedNotify);\nSELECT LAST_INSERT_ID();";
            try
            {
                using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
                    return writeConntion.ExecuteScalar<int>(sql, (object) member_notifyDto);
            }
            catch (Exception ex)
            {
                LogLib.Error("[MemberNotifyServices][Insert]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}