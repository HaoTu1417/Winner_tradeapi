// Decompiled with JetBrains decompiler
// Type: DB.Services.WalletWithdrawService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Wallet;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class WalletWithdrawService
  {
    public static WalletWithdrawDto Find(int pk)
    {
      string sql = "SELECT * FROM `wallet_withdraw` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<WalletWithdrawDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindWaitWithdrawCount(int pk)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 1);
      interpolatedStringHandler.AppendLiteral("SELECT count(*) FROM `wallet_withdraw` WHERE `member_fk` = ");
      interpolatedStringHandler.AppendFormatted<int>(pk);
      interpolatedStringHandler.AppendLiteral(" AND status = 0");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<int>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][FindWaitWithdrawCount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static WalletWithdrawDto Find(string order_no)
    {
      string sql = "SELECT * FROM `wallet_withdraw` WHERE `order_no` = @order_no";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            order_no = order_no
          });
          return readConnection.QueryFirstOrDefault<WalletWithdrawDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static GetLastWithdrawResponse FindLastWithdraw(int member_fk)
    {
      string sql = "SELECT wallet_withdraw.order_no, wallet_withdraw.currency, wallet_withdraw.create_time, member_bank.bank, member_bank.card, member_bank.card_type, wallet_withdraw.money\n                           FROM `wallet_withdraw`\n                           JOIN member_bank ON wallet_withdraw.member_bank_fk = member_bank.card_pk\n                           WHERE member_bank.`member_fk` = @member_fk AND wallet_withdraw.`status` = 0\n                           ORDER BY pk DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QueryFirstOrDefault<GetLastWithdrawResponse>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<WithdrawHistoryResponse> FindHistory(int member_fk)
    {
      string sql = "SELECT * FROM `wallet_withdraw` WHERE `member_fk` = @member_fk ORDER by create_time DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<WithdrawHistoryResponse>(sql, (object) parameters).AsList<WithdrawHistoryResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<WalletWithdrawDto> FindAll()
    {
      string sql = "SELECT * FROM `wallet_withdraw`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletWithdrawDto>(sql).AsList<WalletWithdrawDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(WalletWithdrawDto source)
    {
      string sql = "INSERT INTO `wallet_withdraw` (\n                `member_fk`, `member_bank_fk`, `order_no`, `wallet_amount`, `exchange`, `currency`, `money`, `fee`, `status`, `note`, `create_time`, `create_ip`, `verify_admin_pk`, `verify_time`, `reject_result`, `id_selfie`)\n                VALUES (@member_fk, @member_bank_fk, @order_no, @wallet_amount, @exchange, @currency, @money, @fee, @status, @note, @create_time, @create_ip, @verify_admin_pk, @verify_time, @reject_result, @id_selfie);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(WalletWithdrawDto model)
    {
      string sql = "UPDATE `wallet_withdraw` SET \n                `member_fk` = @member_fk,\n                `member_bank_fk` = @member_bank_fk,\n                `order_no` = @order_no,\n                `wallet_amount` = @wallet_amount,\n                `exchange` = @exchange,\n                `currency` = @currency,\n                `money` = @money,\n                `fee` = @fee,\n                `status` = @status,\n                `note` = @note,\n                `create_time` = @create_time,\n                `create_ip` = @create_ip,\n                `verify_admin_pk` = @verify_admin_pk,\n                `verify_time` = @verify_time,\n                `reject_result` = @reject_result\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `wallet_withdraw` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetWithdrawAmountToday(int member_fk)
    {
      string sql = "SELECT IFNULL(SUM(money), 0) FROM `wallet_withdraw`\n                           WHERE `member_fk` = @member_fk AND (status = 0 OR status = 1) AND DATE(create_time) = CURDATE()";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QuerySingle<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][GetWithdrawAmountToday]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetWithdrawAmountThisMonth(int member_fk)
    {
      string sql = "SELECT IFNULL(SUM(money), 0) FROM `wallet_withdraw`\n                           WHERE `member_fk` = @member_fk AND (status = 0 OR status = 1) AND YEAR(create_time) = YEAR(CURDATE()) AND MONTH(create_time) = MONTH(CURDATE())";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QuerySingle<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][GetWithdrawAmountThisMonth]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int CancelWithdraw(int member_fk, string order_no)
    {
      string sql = "UPDATE `wallet_withdraw` SET \n                `status` = 3\n                 WHERE `member_fk` = @member_fk AND `order_no` = @order_no";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            order_no = order_no
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetWithdrawApplyCount()
    {
      string sql = "SELECT COUNT(*) FROM `wallet_withdraw` INNER JOIN `member` ON (wallet_withdraw.member_fk = `member`.pk AND member.is_del = 0) where wallet_withdraw.status = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][GetRechargeApplyCount]" + ex.Message);
        return 0;
      }
    }

    public static Decimal GetWithdrawNeedVerify()
    {
      try
      {
        string sql = "SELECT SUM(wallet_amount) FROM `wallet_withdraw`\n                            INNER JOIN `member` ON (member.pk = wallet_withdraw.member_fk AND member.is_test_account = 0 AND member.is_del = 0) WHERE wallet_withdraw.status = 0";
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<Decimal>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletWithdrawService][GetWithdrawNeedVerify]" + ex.Message);
        return 0M;
      }
    }
  }
}
