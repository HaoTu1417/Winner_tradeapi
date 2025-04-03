// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WalletRechargeService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Wallet;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class WalletRechargeService
  {
    public static WalletRechargeDto Find(int pk)
    {
      string sql = string.Format("SELECT * FROM `wallet_recharge` WHERE member_fk = {0}", (object) pk);
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<WalletRechargeDto>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][Find(int pk)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static WalletRechargeDto Find(string order_no)
    {
      string sql = string.Format("SELECT * FROM `wallet_recharge` WHERE order_no = '{0}'", (object) order_no);
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<WalletRechargeDto>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][Find(string order_no)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<RechargeHistoryResponse> FindHistory(int member_fk)
    {
      string sql = "SELECT * FROM `wallet_recharge` WHERE `member_fk` = @member_fk ORDER by create_time DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<RechargeHistoryResponse>(sql, (object) parameters).AsList<RechargeHistoryResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][FindHistory]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void Insert(WalletRechargeDto wallet_recharge)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "INSERT INTO `wallet_recharge` (`member_fk`, `admin_bank_fk`, `order_no`, `type`, `currency`, `money`, `exchange`, `wallet_amount`, `fee`, `create_time`,\n                                    `create_ip`, `line_bank`, `status`, `charge_type_id`, `form_name`, `last_five`, `platform_order_no`)\n                                   VALUES (@member_fk, @admin_bank_fk, @order_no, @type, @currency, @money, @exchange, @wallet_amount, @fee, @create_time,\n                                    @create_ip, @line_bank, @status, @charge_type_id, @form_name, @last_five, @platform_order_no);";
          writeConntion.Execute(sql, (object) wallet_recharge);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int AccecptRecharge(string order_no)
    {
      string sql = "UPDATE `wallet_recharge` SET \n                `verify_time` = UTC_TIMESTAMP(),\n                `status` = 1    \n                 WHERE `order_no` = '" + order_no + "'";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][AccecptRecharge]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
    
    public static int UpdateRechargeStatus(string order_no)
    {
      string sql = "UPDATE `wallet_recharge` SET \n                `verify_time` = UTC_TIMESTAMP(),\n                `status` = 1    \n                 WHERE `order_no` = '" + order_no + "'";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][AccecptRecharge]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int RejectRecharge(string order_no, string rejectResult)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(184, 2);
      interpolatedStringHandler.AppendLiteral("UPDATE `wallet_recharge` SET \n                `verify_time` = UTC_TIMESTAMP(),\n                `status` = 2,\n                `reject_result` = '");
      interpolatedStringHandler.AppendFormatted(rejectResult);
      interpolatedStringHandler.AppendLiteral("'\n                 WHERE `order_no` = '");
      interpolatedStringHandler.AppendFormatted(order_no);
      interpolatedStringHandler.AppendLiteral("'");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(stringAndClear, (object) new
          {
            order_no = order_no,
            rejectResult = rejectResult
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][RejectRecharge]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetRechargeApplyCount()
    {
      string sql = "SELECT COUNT(*) FROM `wallet_recharge` INNER JOIN `member` ON (wallet_recharge.member_fk = `member`.pk AND member.is_del = 0) where wallet_recharge.status = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][GetRechargeApplyCount]" + ex.Message);
        return 0;
      }
    }

    public static int GetRechargeNeedVerify()
    {
      string sql = "SELECT count(*) FROM `wallet_recharge`\n                INNER JOIN `member` ON member.pk = wallet_recharge.member_fk\n                WHERE `wallet_recharge`.`status` = 0 AND member.is_test_account = 0  AND member.is_del = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRechargeService][GetRechargeNeedVerify]" + ex.Message);
        return 0;
      }
    }
  }
}
