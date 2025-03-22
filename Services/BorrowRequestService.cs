// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.BorrowRequestService
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
namespace tradeapi.Services
{
  public class BorrowRequestService
  {
    public static BorrowRequestDto FindTerminateRequest(string sub_account)
    {
      string sql = "SELECT * FROM `borrow_request` WHERE sub_account = @sub_account AND type = 2 AND status = 0";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<BorrowRequestDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowRequestService][FindTerminateRequest]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(BorrowRequestDto dto)
    {
      string sql = "INSERT INTO `borrow_request` \n                (`borrow_plan_fk`, `sub_account`, `borrow_fk`, `member_fk`, `type`, `borrow_fee`, `use_coupon`, `fee_received`, `borrow_duration`, `new_end_time`, `status`, `add_time`, `verify_time`) VALUES \n                (@borrow_plan_fk, @sub_account, @borrow_fk, @member_fk, @type, @borrow_fee, @use_coupon, @fee_received, @borrow_duration, @new_end_time, @status, @add_time, @verify_time)";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          writeConntion.Execute(sql, (object) dto);
          return writeConntion.QuerySingle<int>("SELECT LAST_INSERT_ID()");
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowRequestService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static bool IsExistRecord(string sub_account)
    {
      string sql = "SELECT count(*) FROM borrow_request WHERE sub_account = @sub_account AND status = 0";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingle<int>(sql, (object) data) > 0;
      }
      catch (Exception ex)
      {
        LogLib.Fatal(ex.Message, ex);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowRequestDto Find(int pk)
    {
      string sql = "SELECT * FROM `borrow_request` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QuerySingle<BorrowRequestDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Fatal(ex.Message, ex);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateState(int id, bool state)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE `borrow_request` SET status = @status,verify_time=@verify_time\n                                WHERE pk=@pk;", (object) new
          {
            pk = id,
            status = (state ? 1 : 2),
            verify_time = DateTime.UtcNow
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowRequestService][UpdateState]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `borrow_request` WHERE `pk` = @pk";
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
        LogLib.Error("[BorrowRequestService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
