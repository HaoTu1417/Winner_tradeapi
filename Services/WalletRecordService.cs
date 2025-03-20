// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WalletRecordService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Wallet;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class WalletRecordService
  {
    public static int FindPkAfterInsert(WalletRecordDto source)
    {
      string sql = "INSERT INTO `wallet_record` (\n                `member_fk`, `type`, `currency`, `affect`, `freeze`, `balance`, `coupon`, `param`, `templat_id`, `info`, `create_time`, `create_ip`)\n                VALUES (@member_fk, @type, @currency, @affect, @freeze, @balance, @coupon, @param, @templat_id, @info, @create_time, @create_ip);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRecordService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<GetRecordHistoryResponse> FindAll(int member_fk, string lang)
    {
      string sql = "SELECT *, wallet_template.name as type FROM `wallet_record`\n                           INNER JOIN wallet_template ON wallet_record.templat_id = wallet_template.temp_id AND wallet_template.lang = @lang\n                           WHERE wallet_record.member_fk = @member_fk ORDER by wallet_record.create_time DESC";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            lang = lang
          });
          return writeConntion.Query<GetRecordHistoryResponse>(sql, (object) parameters).AsList<GetRecordHistoryResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRecordService][FindAll]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<GetRecordHistoryResponse> FindTradeHistory(int member_fk, string lang)
    {
      string sql = "SELECT *, wallet_template.name as type FROM `wallet_record`\n                           INNER JOIN wallet_template ON wallet_record.type = wallet_template.temp_id AND wallet_template.lang = @lang\n                           WHERE wallet_record.member_fk = @member_fk ORDER by wallet_record.create_time DESC";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            lang = lang
          });
          return writeConntion.Query<GetRecordHistoryResponse>(sql, (object) parameters).AsList<GetRecordHistoryResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRecordService][FindTradeHistory]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<CouponrecordResponse> FindAllCoupon(int member_fk, string lang = "VN")
    {
      string sql = "\nSELECT T2.name AS `Category`, \nT1.info AS `Title`, \nT1.create_time AS `Date`, \nT1.affect AS `TransactionAmount`, \nT1.coupon_balance AS `CouponBalance`\nFROM `wallet_coupon_record` T1\nLEFT OUTER JOIN `wallet_template` T2 ON (T1.type = T2.temp_id)\nWHERE `member_fk` = @member_fk AND T2.lang = @lang\nAND T1.`type` IN (21, 23, 26, 123)\nORDER BY `Date` DESC\n";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            lang = lang
          });
          return writeConntion.Query<CouponrecordResponse>(sql, (object) parameters).AsList<CouponrecordResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRecordService][FindAllCoupon]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<GetRecordHistoryResponse> FindAllRichBoxRecord(
      int member_fk,
      string lang,
      int buyFundTempId,
      int sellFundTempId,
      int fundInterest)
    {
      string sql = "SELECT *, wallet_template.name as type FROM `wallet_record`\n                           INNER JOIN wallet_template ON wallet_record.type = wallet_template.temp_id AND wallet_template.lang = @lang\n                           WHERE wallet_record.type IN (@buyFundTempId, @sellFundTempId, @fundInterest)\n                            AND wallet_record.member_fk = @member_fk ORDER by wallet_record.create_time DESC";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            buyFundTempId = buyFundTempId,
            sellFundTempId = sellFundTempId,
            fundInterest = fundInterest,
            member_fk = member_fk,
            lang = lang
          });
          return writeConntion.Query<GetRecordHistoryResponse>(sql, (object) parameters).AsList<GetRecordHistoryResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRecordService][FindAllRichBoxRecord]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static DateTime FindLastRecordTime(int member_fk)
    {
      string sql = "SELECT create_time FROM `wallet_record`\n                           WHERE member_fk = @member_fk ORDER by create_time DESC";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return writeConntion.QueryFirst<DateTime>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRecordService][FindLastRecordTime]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
