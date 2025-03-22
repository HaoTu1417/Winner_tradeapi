// Decompiled with JetBrains decompiler
// Type: DB.Services.TradePositionService
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
  public class TradePositionService
  {
    public static TradePositionDto? Find(string account, string code, string market)
    {
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        string sql = "SELECT * FROM trade_position WHERE sub_account = @account AND stock_code = @stock_code AND market = @market";
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          account = account,
          stock_code = code,
          market = market
        });
        return readConnection.QuerySingleOrDefault<TradePositionDto>(sql, (object) parameters);
      }
    }

    public static List<TradePositionDto> FindBySubAccount(string sub_account)
    {
      string sql = "\nSELECT * FROM `trade_position` \nWHERE `sub_account` = @sub_account \n;";
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          sub_account = sub_account
        });
        return readConnection.Query<TradePositionDto>(sql, (object) parameters).ToList<TradePositionDto>();
      }
    }

    public static List<TradePositionDto> FindAll(
      string market,
      string sub_account,
      string stock_code)
    {
      string sql = "\nSELECT * FROM `trade_position` \nWHERE `market` = @market\nAND `sub_account` = @sub_account \nAND `stock_code` = @stock_code\n;";
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          market = market,
          sub_account = sub_account,
          stock_code = stock_code
        });
        return readConnection.Query<TradePositionDto>(sql, (object) parameters).AsList<TradePositionDto>();
      }
    }

    public static TradePositionDto Builder(
      string market,
      string sub_account,
      string stock_code,
      string stock_name,
      int stock_type)
    {
      TradePositionDto model = TradePositionService.FindAll(market, sub_account, stock_code).Find((Predicate<TradePositionDto>) (x => x.stock_type == stock_type));
      if (model == null)
      {
        model = new TradePositionDto()
        {
          sub_account = sub_account,
          stock_code = stock_code,
          stock_name = stock_name,
          stock_type = stock_type,
          market = market,
          holding_volume = 0,
          stop_lose_pos = 0,
          new_pos = 0,
          close_pos = 0,
          lastprice = 0M,
          total = 0M,
          cost_purchase = 0M,
          cost_volume = 0,
          cost_price = 0M,
          live_volume = 0,
          live_cost = 0M
        };
        TradePositionService.Insert(model);
      }
      return model;
    }

    public static void Insert(TradePositionDto model)
    {
      string sql = "\nINSERT INTO `trade_position`\n(`sub_account`, `stock_code`, `stock_name`, `stock_type`, `market`, `holding_volume`, `stop_lose_pos`, `new_pos`, `close_pos`, `lastprice`, `total`, `cost_purchase`, `cost_volume`, `cost_price`, `live_volume`, `live_cost`)\nVALUES\n(@sub_account, @stock_code, @stock_name, @stock_type, @market, @holding_volume, @stop_lose_pos, @new_pos, @close_pos, @lastprice, @total, @cost_purchase, @cost_volume, @cost_price, @live_volume, @live_cost);\n";
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        writeConntion.Execute(sql, (object) model);
    }

    public static int Update(TradePositionDto model)
    {
      string sql = "\nUPDATE `trade_position`\nSET `stock_name` = @stock_name\n,`stock_type` = @stock_type\n,`market` = @market\n,`holding_volume` = @holding_volume\n,`stop_lose_pos` = @stop_lose_pos\n,`new_pos` = @new_pos\n,`close_pos` = @close_pos\n,`lastprice` = @lastprice\n,`total` = @total\n,`cost_purchase` = @cost_purchase\n,`cost_volume` = @cost_volume\n,`cost_price` = @cost_price\n,`live_volume` = @live_volume\n,`live_cost` = @live_cost\nWHERE `sub_account` = @sub_account\n;";
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        return writeConntion.Execute(sql, (object) model);
    }

    public static Decimal GetPositionValue(string sub_account)
    {
      string sql = "SELECT SUM(total) FROM `trade_position` WHERE `sub_account` = @sub_account";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account
          });
          return readConnection.ExecuteScalar<Decimal>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradePositionService][GetPositionValue]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateClosePosition(string sub_account, Decimal deductionAmount)
    {
      string sql = "\n        UPDATE `trade_position`\n        SET `close_pos` = `close_pos` - @deductionAmount\n        WHERE `sub_account` = @sub_account;\n    ";
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        return writeConntion.Execute(sql, (object) new
        {
          sub_account = sub_account,
          deductionAmount = deductionAmount
        });
    }

    public static void AddStopClosePos(string sub_account, string stock_code, int volume)
    {
      string sql = "UPDATE `trade_position` SET `stop_lose_pos` = `stop_lose_pos` + @volume WHERE `sub_account` = @sub_account AND stock_code = @stock_code";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          var data = new
          {
            sub_account = sub_account,
            stock_code = stock_code,
            volume = volume
          };
          writeConntion.Execute(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradePositionService][AddStopClosePos]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void ReduceStopLosePos(string sub_account, string stock_code, int volume)
    {
      string sql = "UPDATE `trade_position` SET `stop_lose_pos` = `stop_lose_pos` - @volume WHERE `sub_account` = @sub_account AND stock_code = @stock_code";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          var data = new
          {
            sub_account = sub_account,
            stock_code = stock_code,
            volume = volume
          };
          writeConntion.Execute(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradePositionService][ReduceStopLosePos]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void ReduceClosePos(string sub_account, string stock_code, int volume)
    {
      string sql = "UPDATE `trade_position` SET `close_pos` = `close_pos` - @volume WHERE `sub_account` = @sub_account AND stock_code = @stock_code";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          var data = new
          {
            sub_account = sub_account,
            stock_code = stock_code,
            volume = volume
          };
          writeConntion.Execute(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradePositionService][ReduceClosePos]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void AddClosePos(string sub_account, string stock_code, int volume)
    {
      string sql = "UPDATE `trade_position` SET `close_pos` = `close_pos` + @volume WHERE `sub_account` = @sub_account AND stock_code = @stock_code";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          var data = new
          {
            sub_account = sub_account,
            stock_code = stock_code,
            volume = volume
          };
          writeConntion.Execute(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradePositionService][AddClosePos]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetPositionBySubAccount(string sub_account)
    {
      string sql = "SELECT COUNT(*) FROM trade_position WHERE sub_account = @sub_account;";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>(sql, (object) new
          {
            sub_account = sub_account
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradePositionService][GetPositionBySubAccount]" + ex.Message);
        return 0;
      }
    }
  }
}
