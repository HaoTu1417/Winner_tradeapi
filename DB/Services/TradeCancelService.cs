// Decompiled with JetBrains decompiler
// Type: DB.Services.TradeCancelService
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
  public class TradeCancelService
  {
    public static TradeCancelDto Find(int pk)
    {
      string sql = "SELECT * FROM `trade_cancel` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<TradeCancelDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeCancelService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeCancelDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_cancel`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeCancelDto>(sql).AsList<TradeCancelDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeCancelService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateFull(TradeCancelDto model)
    {
      string sql = "UPDATE `trade_cancel` SET \n                `trade_order_sn` = @trade_order_sn,\n                `sub_account` = @sub_account,\n                `sn` = @sn,\n                `market` = @market,\n                `stock_code` = @stock_code,\n                `stock_name` = @stock_name,\n                `request_volume` = @request_volume,\n                `cancel_volume` = @cancel_volume,\n                `order_ip` = @order_ip,\n                `order_client` = @order_client,\n                `cancel_type` = @cancel_type,\n                `cancel_datetime` = @cancel_datetime,\n                `cancel_by` = @cancel_by\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeCancelService][UpdateFull]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `trade_cancel` WHERE `pk` = @pk";
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
        LogLib.Error("[TradeCancelService][Remove]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static bool IsDuplicateSN(string sn)
    {
      string sql = "SELECT COUNT(*) FROM `trade_cancel` WHERE `sn` = @sn";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          var data = new{ sn = sn };
          return readConnection.ExecuteScalar<int>(sql, (object) data) > 0;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeCancelService][IsDuplicateSN]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(TradeCancelDto source)
    {
      try
      {
        string sql = "INSERT INTO `trade_cancel` (\n                `trade_order_sn`, `sub_account`, `sn`, `market`, `stock_code`, `stock_name`, `request_volume`, `cancel_volume`, `order_ip`, `order_client`, `cancel_type`, `cancel_datetime`, `cancel_by`)\n                VALUES (@trade_order_sn, @sub_account, @sn, @market, @stock_code, @stock_name, @request_volume, @cancel_volume, @order_ip, @order_client, @cancel_type, @cancel_datetime, @cancel_by);\n                SELECT LAST_INSERT_ID();";
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeCancelService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
