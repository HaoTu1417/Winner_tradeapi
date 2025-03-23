// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.BorrowAddfinancingService
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
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class BorrowAddfinancingService
  {
    public static BorrowAddfinancingDto Find(int pk)
    {
      string sql = "SELECT * FROM `borrow_addfinancing` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<BorrowAddfinancingDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddfinancingService][Find(int pk)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowAddfinancingDto Find(string sub_account)
    {
      string sql = "SELECT * FROM `borrow_addfinancing` WHERE `sub_account` = @sub_account AND status = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account
          });
          return readConnection.QueryFirstOrDefault<BorrowAddfinancingDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddfinancingService][Find(string sub_account)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<BorrowAddfinancingDto> FindAll()
    {
      string sql = "SELECT * FROM `borrow_addfinancing`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<BorrowAddfinancingDto>(sql).AsList<BorrowAddfinancingDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddfinancingService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(BorrowAddfinancingDto source)
    {
      string sql = "INSERT INTO `borrow_addfinancing` (\n                `sub_account`, `borrow_fk`, `member_fk`, `currency`, `money`, `exchange`, `freeze`, `multiple`, `borrow_interest`, `last_deposit_money`, `last_borrow_money`, `status`, `add_time`, `verify_time`, `target_uid`, `target_name`, `coupon`)\n                VALUES (@sub_account, @borrow_fk, @member_fk, @currency, @money, @exchange, @freeze, @multiple, @borrow_interest, @last_deposit_money, @last_borrow_money, @status, @add_time, @verify_time, @target_uid, @target_name, @coupon);\n\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddfinancingService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(BorrowAddfinancingDto model)
    {
      string sql = "UPDATE `borrow_addfinancing` SET \n                `sub_account` = @sub_account,\n                `borrow_fk` = @borrow_fk,\n                `member_fk` = @member_fk,\n                `currency` = @currency,\n                `money` = @money,\n                `exchange` = @exchange,\n                `freeze` = @freeze,\n                `multiple` = @multiple,\n                `borrow_interest` = @borrow_interest,\n                `last_deposit_money` = @last_deposit_money,\n                `last_borrow_money` = @last_borrow_money,\n                `status` = @status,\n                `add_time` = @add_time,\n                `verify_time` = @verify_time,\n                `target_uid` = @target_uid,\n                `target_name` = @target_name,\n                `coupon` = @coupon\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddfinancingService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `borrow_addfinancing` WHERE `pk` = @pk";
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
        LogLib.Error("[BorrowAddfinancingService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateState(int id, bool state)
    {
      string sql = "UPDATE `borrow_addfinancing` SET status = @status,verify_time=@verify_time WHERE pk=@pk;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            pk = id,
            status = (state ? 1 : 2),
            verify_time = DateTime.UtcNow
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddfinancingService][UpdateState]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
