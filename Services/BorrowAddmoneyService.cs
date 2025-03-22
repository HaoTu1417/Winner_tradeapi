// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.BorrowAddmoneyService
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
  public class BorrowAddmoneyService
  {
    public static BorrowAddmoneyDto Find(int pk)
    {
      string sql = "SELECT * FROM `borrow_addmoney` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<BorrowAddmoneyDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddmoneyService][Find(int pk)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowAddmoneyDto Find(string sub_account)
    {
      string sql = "SELECT * FROM `borrow_addmoney` WHERE `sub_account` = @sub_account AND status = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account
          });
          return readConnection.QueryFirstOrDefault<BorrowAddmoneyDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddmoneyService][Find(string sub_account)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<BorrowAddmoneyDto> FindAll()
    {
      string sql = "SELECT * FROM `borrow_addmoney`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<BorrowAddmoneyDto>(sql).AsList<BorrowAddmoneyDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddmoneyService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(BorrowAddmoneyDto source)
    {
      string sql = "INSERT INTO `borrow_addmoney` (\n                `sub_account`, `member_fk`, `currency`, `exchange`, `money`, `freeze`, `status`, `add_time`, `verify_time`, `target_uid`, `target_name`)\n                VALUES (@sub_account, @member_fk, @currency, @exchange, @money, @freeze, @status, @add_time, @verify_time, @target_uid, @target_name);\n\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddmoneyService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(BorrowAddmoneyDto model)
    {
      string sql = "UPDATE `borrow_addmoney` SET \n                `sub_account` = @sub_account,\n                `member_fk` = @member_fk,\n                `currency` = @currency,\n                `exchange` = @exchange,\n                `money` = @money,\n                `freeze` = @freeze,\n                `status` = @status,\n                `add_time` = @add_time,\n                `verify_time` = @verify_time,\n                `target_uid` = @target_uid,\n                `target_name` = @target_name,\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddmoneyService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `borrow_addmoney` WHERE `pk` = @pk";
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
        LogLib.Error("[BorrowAddmoneyService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateStatus(int id, bool state)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE `borrow_addmoney` SET status = @status, verify_time = @verify_time\nWHERE pk=@pk;", (object) new
          {
            pk = id,
            status = (state ? 1 : 2),
            verify_time = DateTime.UtcNow
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowAddmoneyService][UpdateStatus]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
