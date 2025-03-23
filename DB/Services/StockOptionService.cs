// Decompiled with JetBrains decompiler
// Type: DB.Services.StockOptionService
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
namespace DB.Services
{
  public class StockOptionService
  {
    public static StockOptionDto Find(int pk)
    {
      string sql = "SELECT * FROM `stock_option` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<StockOptionDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionService][Find]" + ex.Message);
        return (StockOptionDto) null;
      }
    }

    public static List<StockOptionDto> FindAll(string market)
    {
      string sql = "SELECT *, s.stock_code, s.stock_name FROM `stock_option`\n                           LEFT JOIN `stock_" + market.ToLower() + "` s ON s.stock_code = stock_option.stock_code\n                           WHERE stock_option.market = @market AND s.enable = 1 AND stock_option.enable = 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            market = market.ToUpper()
          });
          return readConnection.Query<StockOptionDto>(sql, (object) parameters).AsList<StockOptionDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionService][FindAll]" + ex.Message);
        return (List<StockOptionDto>) null;
      }
    }

    public static int FindPkAfterInsert(StockOptionDto source)
    {
      string sql = "INSERT INTO `stock_option` (\n                `stock_code`, `spot`, `remain_spot`, `price`, `quantity`, `enable`, `create_time`)\n                VALUES (@stock_code, @spot, @spot, @price, @quantity, @enable, UTC_TIMESTAMP);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(StockOptionDto model)
    {
      string sql = "UPDATE `stock_option` SET \n                `stock_code` = @stock_code,\n                `spot` = @spot,\n                `price` = @price,\n                `quantity` = @quantity,\n                `enable` = @enable\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        LogLib.Error("[StockOptionService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateRemainSpot(int pk, int diff)
    {
      string sql = "UPDATE `stock_option` SET \n                `remain_spot` = remain_spot + @diff\n                 WHERE `pk` = @pk";
      try
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          pk = pk,
          diff = diff
        });
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) parameters);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionService][UpdateStatus]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `stock_option` WHERE `pk` = @pk";
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
        LogLib.Error("[StockOptionService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
