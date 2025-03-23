// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.TradeDealService
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
using tradeapi.Models.SubAccount;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class TradeDealService
  {
    public static TradeDealDto Find(int pk)
    {
      string sql = "SELECT * FROM `trade_deal` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<TradeDealDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeDealDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_deal`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeDealDto>(sql).AsList<TradeDealDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeDealDto> FindByTradeOrderSN(string trade_order_sn)
    {
      string sql = "SELECT * FROM `trade_deal` WHERE `trade_order_sn` = @trade_order_sn";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeDealDto>(sql, (object) new
          {
            trade_order_sn = trade_order_sn
          }).AsList<TradeDealDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][FindByTradeOrderSN]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeDealDto> FindTodayBySubAccount(string sub_account)
    {
      string sql = "SELECT * FROM `trade_deal` WHERE `sub_account` = @sub_account \n                  AND DATE(`create_datetime`) = UTC_DATE ORDER BY `create_datetime` DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeDealDto>(sql, (object) new
          {
            sub_account = sub_account
          }).AsList<TradeDealDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][FindTodayBySubAccount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(TradeDealDto source)
    {
      string sql = "INSERT INTO `trade_deal` (\n                `deal_id`, `sub_account`, `trade_order_sn`, `stock_code`, `stock_name`, `market`, `dir`, `final_price`, `final_volume`, `create_datetime`, `currency`, `total_pay`, `total_amount`, `total_cost`, `handling_fee`, `transfer_fee`, `stamp_fee`, `other_fee`)\n                VALUES (@deal_id, @sub_account, @trade_order_sn, @stock_code, @stock_name, @market, @dir, @final_price, @final_volume, @create_datetime, @currency, @total_pay, @total_amount, @total_cost, @handling_fee, @transfer_fee, @stamp_fee, @other_fee);\n\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(TradeDealDto model)
    {
      string sql = "UPDATE `trade_deal` SET \n                `deal_id` = @deal_id,\n                `sub_account` = @sub_account,\n                `trade_order_sn` = @trade_order_sn,\n                `stock_code` = @stock_code,\n                `stock_name` = @stock_name,\n                `market` = @market,\n                `dir` = @dir,\n                `final_price` = @final_price,\n                `final_volume` = @final_volume,\n                `create_datetime` = @create_datetime,\n                `currency` = @currency,\n                `total_pay` = @total_pay,\n                `total_amount` = @total_amount,\n                `total_cost` = @total_cost,\n                `handling_fee` = @handling_fee,\n                `transfer_fee` = @transfer_fee,\n                `stamp_fee` = @stamp_fee,\n                `other_fee` = @other_fee\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `trade_deal` WHERE `pk` = @pk";
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
        LogLib.Error("[TradeDealService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<HisdealResponse> GetDeals(
      string sub_account,
      DateTime? start_time,
      DateTime? end_time,
      string? keyword)
    {
      string str = "SELECT * FROM `trade_deal` WHERE `sub_account` = @sub_account";
      if (start_time.HasValue)
        str += " AND `create_datetime` >= @start_time";
      if (end_time.HasValue)
      {
        end_time = new DateTime?(end_time.Value.Add(new TimeSpan(23, 59, 59)));
        str += " AND `create_datetime` <= @end_time";
      }
      if (keyword != null)
      {
        keyword = "%" + keyword + "%";
        str += " AND `stock_code` LIKE @keyword";
      }
      string sql = str + " ORDER BY create_datetime DESC";
      var data = new
      {
        sub_account = sub_account,
        start_time = start_time,
        end_time = end_time,
        keyword = keyword
      };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<HisdealResponse>(sql, (object) data).AsList<HisdealResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][GetDeals]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static DealResponse GetDeal(string deal_id)
    {
      string sql = "SELECT *, trade_order.order_time AS order_datetime FROM `trade_deal`                       \n                       INNER JOIN `trade_order` ON trade_deal.trade_order_sn = trade_order.sn\n                       WHERE trade_deal.`deal_id` = @deal_id";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          var data = new{ deal_id = deal_id };
          return readConnection.QueryFirstOrDefault<DealResponse>(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeDealService][GetDeal]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
