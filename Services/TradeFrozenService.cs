// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.TradeFrozenService
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
  public class TradeFrozenService
  {
    public static TradeFrozenDto Find(int pk)
    {
      string sql = "SELECT * FROM `trade_frozen` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<TradeFrozenDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static TradeFrozenDto? FindByOrderId(int trade_order_fk)
    {
      string sql = "SELECT * FROM trade_frozen WHERE trade_order_fk = @trade_order_fk";
      var data = new{ trade_order_fk = trade_order_fk };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<TradeFrozenDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][FindByOrderId]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static TradeFrozenDto FindByTradeOrderFk(int trade_order_fk)
    {
      string sql = "SELECT * FROM `trade_frozen` WHERE `trade_order_fk` = @trade_order_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            trade_order_fk = trade_order_fk
          });
          return readConnection.QuerySingleOrDefault<TradeFrozenDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][FindByTradeOrderFk]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeFrozenDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_frozen`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeFrozenDto>(sql).AsList<TradeFrozenDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(TradeFrozenDto source)
    {
      string sql = "\nINSERT INTO `trade_frozen`\n(`sub_account`, `trade_order_fk`, `pk`, `info`, `type`, `frozen_volume`, `frozen_money`, `frozen_datetime`)\nVALUES\n(@sub_account, @trade_order_fk, @pk, '', @type, @frozen_volume, @frozen_money, @frozen_datetime);\n\nSELECT LAST_INSERT_ID();\n";
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        return writeConntion.ExecuteScalar<int>(sql, (object) source);
    }

    public static int FindPkAfterInsert(TradeFrozenDto source)
    {
      string sql = "INSERT INTO `trade_frozen` (\n                `sub_account`, `trade_order_fk`, `info`, `type`, `frozen_volume`, `frozen_money`, `frozen_datetime`)\n                VALUES (@sub_account, @trade_order_fk, @info, @type, @frozen_volume, @frozen_money, @frozen_datetime);\n\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(TradeFrozenDto model)
    {
      string sql = "UPDATE `trade_frozen` SET \n                `sub_account` = @sub_account,\n                `trade_order_fk` = @trade_order_fk,\n                `info` = @info,\n                `type` = @type,\n                `frozen_volume` = @frozen_volume,\n                `frozen_money` = @frozen_money,\n                `frozen_datetime` = @frozen_datetime\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static Decimal GetFrozenMoney(string sub_account)
    {
      string sql = "SELECT SUM(frozen_money) FROM trade_frozen WHERE sub_account = @sub_account";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingle<Decimal>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][GetFrozenMoney]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int DelTradeFrozen(int tradeOrderFk)
    {
      string sql = "DELETE FROM trade_frozen WHERE type = 1 AND trade_order_fk = @tradeOrderFk;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) new
          {
            tradeOrderFk = tradeOrderFk
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][DelTradeFrozen]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void Remove(int trade_order_fk)
    {
      try
      {
        string sql = "DELETE FROM trade_frozen WHERE trade_order_fk = @trade_order_fk";
        var data = new{ trade_order_fk = trade_order_fk };
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeFrozenService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
