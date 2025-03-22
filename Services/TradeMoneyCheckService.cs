// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.TradeMoneyCheckService
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
using tradeapi.Models.Dto;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class TradeMoneyCheckService
  {
    public static TradeMoneyCheckDto Find(int pk)
    {
      string sql = "SELECT * FROM `trade_money_check` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<TradeMoneyCheckDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyCheckService][Find(int pk)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static TradeMoneyCheckDto Find(string sub_account)
    {
      string sql = "SELECT * FROM `trade_money_check` WHERE `sub_account` = @sub_account AND state = 0 AND type = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account
          });
          return readConnection.QueryFirstOrDefault<TradeMoneyCheckDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyCheckService][Find(string sub_account)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeMoneyCheckDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_money_check`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeMoneyCheckDto>(sql).AsList<TradeMoneyCheckDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyCheckService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(TradeMoneyCheckDto source)
    {
      string sql = "INSERT INTO `trade_money_check` (\n                `sub_account`, `sn`, `type`, `state`, `frozen`, `exchange`, `currency`, `amount`, `request_time`, `acccept_by`, `accept_time`)\n                VALUES (@sub_account, @sn, @type, @state, @frozen, @exchange, @currency, @amount, @request_time, @acccept_by, @accept_time);\n\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyCheckService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(TradeMoneyCheckDto model)
    {
      string sql = "UPDATE `trade_money_check` SET \n                `sub_account` = @sub_account,\n                `sn` = @sn,\n                `type` = @type,\n                `state` = @state,\n                `frozen` = @frozen,\n                `exchange` = @exchange,\n                `currency` = @currency,\n                `amount` = @amount,\n                `request_time` = @request_time,\n                `acccept_by` = @acccept_by,\n                `accept_time` = @accept_time\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyCheckService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `trade_money_check` WHERE `pk` = @pk";
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
        LogLib.Error("[TradeMoneyCheckService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static bool IsDuplicateSN(string sn)
    {
      string sql = "SELECT COUNT(*) FROM `trade_money_check` WHERE `sn` = @sn";
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
        LogLib.Error("[TradeMoneyCheckService][IsDuplicateSN]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static VwTradeAccountDto GetVwTradeAccountBySubAccount(string sub_account)
    {
      string sql = "SELECT * FROM `vw_trade_account` WHERE sub_account = @sub_account";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account
          });
          return readConnection.QueryFirstOrDefault<VwTradeAccountDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyCheckService][GetVwTradeAccountBySubAccount]" + ex.Message);
        return (VwTradeAccountDto) null;
      }
    }

    public static int UpdateWithdraw(int pk, int status)
    {
      string sql = "UPDATE trade_money_check SET state = @state, accept_time = @accept_time WHERE pk=@pk;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            pk = pk,
            state = status,
            accept_time = DateTime.UtcNow
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyCheckService][UpdateWithdraw]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
