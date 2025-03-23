// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.TradeMoneyRecordService
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
// using tradeApi2.Models.Dto;

#nullable enable
namespace tradeapi.Services
{
  public class TradeMoneyRecordService
  {
    public static TradeMoneyRecordDto Find(int pk)
    {
      string sql = "SELECT * FROM `trade_money_record` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<TradeMoneyRecordDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyRecordService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<HisAccountRecordResponse> FindByTradeAccount(string sub_account)
    {
      string sql = "SELECT t1.*, t2.deal_id FROM `trade_money_record` t1 INNER JOIN trade_deal t2 ON t1.trade_deal_fk = t2.pk WHERE t1.`sub_account` = @sub_account ORDER BY t1.create_datetime DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account
          });
          return readConnection.Query<HisAccountRecordResponse>(sql, (object) parameters).AsList<HisAccountRecordResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyRecordService][FindByTradeAccount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<TradeMoneyRecordDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_money_record`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeMoneyRecordDto>(sql).AsList<TradeMoneyRecordDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyRecordService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(TradeMoneyRecordDto source)
    {
      string sql = "INSERT INTO `trade_money_record` (\n                `member_fk`, `sub_account`, `trade_deal_fk`, `sn`, `temp_id`, `op`, `currency`, `balance`, `affect`, `exchange`, `wallet_amount`, `info`, `reviewer`, `create_datetime`, `market`, `stock_code`, `stock_name`, `param`)\n                VALUES (@member_fk, @sub_account, @trade_deal_fk, @sn, @temp_id, @op, @currency, @balance, @affect, @exchange, @wallet_amount, @info, @reviewer, @create_datetime, @market, @stock_code, @stock_name, @param);\n\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyRecordService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(TradeMoneyRecordDto model)
    {
      string sql = "UPDATE `trade_money_record` SET \n                `member_fk` = @member_fk,\n                `sub_account` = @sub_account,\n                `trade_deal_fk` = @trade_deal_fk,\n                `sn` = @sn,\n                `temp_id` = @temp_id,\n                `op` = @op,\n                `currency` = @currency,\n                `balance` = @balance,\n                `affect` = @affect,\n                `exchange` = @exchange,\n                `wallet_amount` = @wallet_amount,\n                `info` = @info,\n                `reviewer` = @reviewer,\n                `create_datetime` = @create_datetime,\n                `market` = @market,\n                `stock_code` = @stock_code,\n                `stock_name` = @stock_name,\n                `param` = @param,\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyRecordService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `trade_money_record` WHERE `pk` = @pk";
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
        LogLib.Error("[TradeMoneyRecordService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<MoneyRecordResponse> GetMoneyRecords(string sub_account)
    {
      string sql = "SELECT * FROM trade_money_record WHERE sub_account = @sub_account AND stock_code IS NULL";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<MoneyRecordResponse>(sql, (object) data).AsList<MoneyRecordResponse>();
      }
      catch (Exception ex)
      {
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static bool IsDuplicateSN(string sn)
    {
      string sql = "SELECT COUNT(*) FROM `trade_money_record` WHERE `sn` = @sn";
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
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static TradeMoneyRecordDto GetByAccount(string sub_account)
    {
      string sql = "SELECT * FROM `trade_money_record` WHERE sub_account = @sub_account ORDER BY create_datetime DESC ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account
          });
          return readConnection.QueryFirstOrDefault<TradeMoneyRecordDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyRecordService][GetByAccount]" + ex.Message);
        return (TradeMoneyRecordDto) null;
      }
    }

    public static int Insert(TradeMoneyRecordDto model)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("INSERT INTO trade_money_record (sub_account, member_fk, trade_deal_fk, sn, temp_id, op,currency, balance, affect, exchange,wallet_amount,info,reviewer,create_datetime,market,stock_code,stock_name, param) VALUES(@sub_account, @member_fk, @trade_deal_fk, @sn, @temp_id, @op, @currency, @balance, @affect, @exchange, @wallet_amount, @info, @reviewer, @create_datetime, @market, @stock_code, @stock_name, @param);", (object) new
          {
            sub_account = model.sub_account,
            member_fk = model.member_fk,
            trade_deal_fk = model.trade_deal_fk,
            sn = model.sn,
            temp_id = model.temp_id,
            op = model.op,
            currency = model.currency,
            balance = model.balance,
            affect = model.affect,
            exchange = model.exchange,
            wallet_amount = model.wallet_amount,
            info = model.info,
            reviewer = model.reviewer,
            create_datetime = model.create_datetime,
            market = model.market,
            stock_code = model.stock_code,
            stock_name = model.stock_name,
            param = model.param
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeMoneyRecordService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
