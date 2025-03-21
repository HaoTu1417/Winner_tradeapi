// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.BorrowFeeService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class BorrowFeeService
  {
    public static BorrowFeeDto Find(int pk)
    {
      string sql = "SELECT * FROM `borrow_fee` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<BorrowFeeDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowFeeService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<BorrowFeeDto> FindAll()
    {
      string sql = "SELECT * FROM `borrow_fee`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<BorrowFeeDto>(sql).AsList<BorrowFeeDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowFeeService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(BorrowFeeDto source)
    {
      string sql = "INSERT INTO `borrow_fee` (\n\t\t\t\t`member_fk`, `sub_account`, `borrow_fk`, `type`, `borrow_fee`, `use_coupon`, `fee_received`, `borrow_duration`, `create_time`)\n\t\t\t\tVALUES (@member_fk, @sub_account, @borrow_fk, @type, @borrow_fee, @use_coupon, @fee_received, @borrow_duration, @create_time);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowFeeService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(BorrowFeeDto model)
    {
      string sql = "UPDATE `borrow_fee` SET \n\t\t\t\t`Recommend_fk` = @Recommend_fk,\n\t\t\t\t`sub_account` = @sub_account,\n\t\t\t\t`borrow_fk` = @borrow_fk,\n\t\t\t\t`type` = @type,\n\t\t\t\t`borrow_fee` = @borrow_fee,\n\t\t\t\t`use_coupon` = @use_coupon,\n\t\t\t\t`fee_received` = @fee_received,\n\t\t\t\t`borrow_duration` = @borrow_duration,\n\t\t\t\t`create_time` = @create_time\n\t\t\t\t WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowFeeService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `borrow_fee` WHERE `pk` = @pk";
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
        LogLib.Error("[BorrowFeeService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<BorrowFeeDto> FindByYearMonth(int year, int month)
    {
      string sql = "SELECT * FROM `borrow_fee`\n                           WHERE YEAR(create_time) = @year AND MONTH(create_time) = @month";
      var data = new{ year = year, month = month };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<BorrowFeeDto>(sql, (object) data).AsList<BorrowFeeDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowFeeService][FindByYearMonth]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
