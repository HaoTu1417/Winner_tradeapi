// Decompiled with JetBrains decompiler
// Type: DB.Services.TradeOrderService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class TradeOrderService
  {
    public static int FindPkAfterInsert(TradeOrderDto source)
    {
      string sql = "INSERT INTO `trade_order` (\n                `sub_account`, `sn`, `stock_code`, `stock_name`, `market`, `dir`, `order_type`, `price_type`, `price`, `status`, `volume`, `free_volume`, `succeed_volume`, `cancel_volume`, `order_time`, `order_ip`, `order_client`, `order_source`, `cancel_datetime`, `cancel_type`, `cancel_by`, `live_mode`, `live_ordersn`, `live_request`, `live_succeed`, `live_price`)\n                VALUES (@sub_account, @sn, @stock_code, @stock_name, @market, @dir, @order_type, @price_type, @price, @status, @volume, @free_volume, @succeed_volume, @cancel_volume, @order_time, @order_ip, @order_client, @order_source, @cancel_datetime, @cancel_type, @cancel_by, @live_mode, @live_ordersn, @live_request, @live_succeed, @live_price);\n\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Insert(
      string sub_account,
      string stock_code,
      string stock_name,
      string market,
      int dir,
      int price_type,
      Decimal price,
      int volume,
      string order_ip)
    {
      string sql = "\nSET @length = 6;\nSET @date = CONCAT('" + (dir == 1 ? "B" : "S") + "',DATE_FORMAT(NOW(), '%y%m%d'));\nSET @sn = (SELECT IFNULL(MAX(RIGHT(`sn`, @length)),0) FROM `trade_order` WHERE LEFT(`sn`,7) = BINARY @date);\nSET @trade_order_sn = CONCAT(@date, RIGHT(CONCAT('000000', @sn + 1), @length));\n\nINSERT INTO `trade_order`\n(`sub_account`, `sn`, `stock_code`, `stock_name`, `market`, `dir`, `order_type`, `price_type`, `price`, `status`, `volume`, `free_volume`, `succeed_volume`, `cancel_volume`, `order_time`, `order_ip`, `order_client`, `order_source`, `cancel_datetime`, `cancel_type`, `cancel_by`, `live_mode`, `live_ordersn`, `live_request`, `live_succeed`, `live_price`)\nVALUES\n(@sub_account, @trade_order_sn, @stock_code, @stock_name, @market, @dir, 0, @price_type, @price, 0, @volume, @free_volume, 0, 0, UTC_TIMESTAMP(), @order_ip, '', 1, null, 0, '', 0, '', 0, 0, 0);\n\nSELECT LAST_INSERT_ID();\n";
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          sub_account = sub_account,
          stock_code = stock_code,
          stock_name = stock_name,
          market = market,
          dir = dir,
          price_type = price_type,
          price = price,
          volume = volume,
          free_volume = volume,
          order_ip = order_ip
        });
        return writeConntion.ExecuteScalar<int>(sql, (object) parameters);
      }
    }

    public static TradeOrderDto Find(int pk)
    {
      try
      {
        string sql = "SELECT * FROM `trade_order` WHERE `pk` = @pk";
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.Query<TradeOrderDto>(sql, (object) parameters).Single<TradeOrderDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeOrderDto> GetOrders(string sub_account, DateTime from, DateTime to)
    {
      string sql = "SELECT * FROM `trade_order` \nWHERE `sub_account` = @sub_account AND `order_time` >= @from AND `order_time` < @to ORDER BY status ASC, order_time DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account,
            from = from,
            to = to
          });
          return readConnection.Query<TradeOrderDto>(sql, (object) parameters).ToList<TradeOrderDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][FindTodayBySubAccount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeOrderDto> FindOpenOrders(string sub_account)
    {
      string sql = "SELECT * FROM `trade_order` WHERE sub_account = @sub_account AND (status = 0 OR status = 2)";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          var data = new{ sub_account = sub_account };
          return readConnection.Query<TradeOrderDto>(sql, (object) data).AsList<TradeOrderDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][FindOpenOrders]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static TradeOrderDto FindBySN(string sn)
    {
      string sql = "SELECT * FROM `trade_order` WHERE `sn` = @sn";
      var data = new{ sn = sn };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<TradeOrderDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][FindBySN]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static TradeOrderDto FindBySN(string sub_account, string sn)
    {
      string sql = "SELECT * FROM `trade_order` \nWHERE `sub_account` = @sub_account AND `sn` = @sn";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account,
            sn = sn
          });
          return readConnection.QueryFirstOrDefault<TradeOrderDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][FindBySN]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void OrderCancel(string sn)
    {
      string sql = "UPDATE `trade_order` \nSET \n    `status` = 2, \n    `cancel_type` = 1, \n    `cancel_datetime` = @cancel_datetime,\n    `cancel_by` = `sub_account`,\n    `cancel_volume` = `cancel_volume` + `free_volume`\nWHERE `sn` = @sn";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          var data = new
          {
            sn = sn,
            cancel_datetime = DateTime.UtcNow
          };
          writeConntion.Execute(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][OrderCancel]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<TradeOrderDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_order`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeOrderDto>(sql).AsList<TradeOrderDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateFull(TradeOrderDto model)
    {
      string sql = "UPDATE `trade_order` SET \n                `sub_account` = @sub_account,\n                `sn` = @sn,\n                `stock_code` = @stock_code,\n                `stock_name` = @stock_name,\n                `market` = @market,\n                `dir` = @dir,\n                `order_type` = @order_type,\n                `price_type` = @price_type,\n                `price` = @price,\n                `status` = @status,\n                `volume` = @volume,\n                `free_volume` = @free_volume,\n                `succeed_volume` = @succeed_volume,\n                `cancel_volume` = @cancel_volume,\n                `order_time` = @order_time,\n                `order_ip` = @order_ip,\n                `order_client` = @order_client,\n                `order_source` = @order_source,\n                `cancel_datetime` = @cancel_datetime,\n                `cancel_type` = @cancel_type,\n                `cancel_by` = @cancel_by,\n                `live_mode` = @live_mode,\n                `live_ordersn` = @live_ordersn,\n                `live_request` = @live_request,\n                `live_succeed` = @live_succeed,\n                `live_price` = @live_price\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][UpdateFull]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `trade_order` WHERE `pk` = @pk";
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
        LogLib.Error("[TradeOrderService][Remove]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static bool IsDuplicateSN(string sn)
    {
      string sql = "SELECT COUNT(*) FROM `trade_order` WHERE `sn` = @sn";
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
        LogLib.Error("[TradeOrderService][IsDuplicateSN]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void UpdateStatus(int pk, int status)
    {
      try
      {
        string sql = "UPDATE `trade_order` SET `status` = @status WHERE `pk` = @pk";
        var data = new{ pk = pk, status = status };
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][UpdateStatus]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void CancelOrder(int pk, int cancel_type, string cancel_by)
    {
      try
      {
        string sql = "UPDATE `trade_order` SET `free_volume` = 0, `cancel_volume` = `volume` - `succeed_volume`, `cancel_datetime` = UTC_TIMESTAMP, `cancel_type` = @cancel_type, `cancel_by` = @cancel_by, `status` = 3 WHERE `pk` = @pk";
        var data = new
        {
          pk = pk,
          cancel_type = cancel_type,
          cancel_by = cancel_by
        };
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeOrderService][CancelOrder]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
