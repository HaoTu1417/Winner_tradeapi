// Decompiled with JetBrains decompiler
// Type: Services.TradeAccountService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using Models.TradeAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace Services
{
  public class TradeAccountService
  {
    public static VM_TradeBalance FindBalance(string sub_account)
    {
      string sql = "\nSELECT * FROM `vw_trade_balance` \nWHERE `sub_account` = @sub_account";
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          sub_account = sub_account
        });
        return readConnection.QueryFirstOrDefault<VM_TradeBalance>(sql, (object) parameters);
      }
    }

    public static TradeAccountDto Find(string sub_account)
    {
      string sql = "SELECT * FROM `trade_account` WHERE `sub_account` = @sub_account";
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          sub_account = sub_account
        });
        return readConnection.QueryFirstOrDefault<TradeAccountDto>(sql, (object) parameters);
      }
    }

    public static List<TradeAccountDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_account`";
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        return readConnection.Query<TradeAccountDto>(sql).AsList<TradeAccountDto>();
    }

    public static int Insert(TradeAccountDto model)
    {
      try
      {
        string sql = "INSERT INTO `trade_account` (\n                `sub_account`, `member_fk`, `borrow_plan_fk`, `type`, `market`, `loan_type`, `currency`, `mem_money`, `frozen_money`, `margin`, `margin_float`, `multiple`, `loan_money`, `time_zone`, `begin_time`, `end_time`, `close_time`, `status`, `warningline`, `breakline`, `notice_warning`, `notice_close`, `live_state`, `live_balance`, `live_breakline`, `live_Broker`, `live_account_fk`, `borrow_duration`)\n                VALUES (@sub_account, @member_fk, @borrow_plan_fk, @type, @market, @loan_type, @currency, @mem_money, @frozen_money, @margin, @margin_float, @multiple, @loan_money, @time_zone, @begin_time, @end_time, @close_time, @status, @warningline, @breakline, @notice_warning, @notice_close, @live_state, @live_balance, @live_breakline, @live_Broker, @live_account_fk, @borrow_duration); ";
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Update(TradeAccountDto model)
    {
      string sql = "\n                UPDATE `trade_account`\n                SET `member_fk` = @member_fk\n                ,`borrow_plan_fk` = @borrow_plan_fk\n                ,`type` = @type\n                ,`market` = @market\n                ,`loan_type` = @loan_type\n                ,`currency` = @currency\n                ,`mem_money` = @mem_money\n                ,`frozen_money` = @frozen_money\n                ,`margin` = @margin\n                ,`margin_float` = @margin_float\n                ,`loan_money` = @loan_money\n                ,`time_zone` = @time_zone\n                ,`begin_time` = @begin_time\n                ,`end_time` = @end_time\n                ,`close_time` = @close_time\n                ,`status` = @status\n                ,`warningline` = @warningline\n                ,`breakline` = @breakline\n                ,`notice_warning` = @notice_warning\n                ,`notice_close` = @notice_close\n                ,`live_state` = @live_state\n                ,`live_balance` = @live_balance\n                ,`live_breakline` = @live_breakline\n                ,`live_Broker` = @live_Broker\n                ,`live_account_fk` = @live_account_fk\n                WHERE `sub_account` = @sub_account\n                ;";
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        return writeConntion.Execute(sql, (object) model);
    }

    public static int Remove(string sub_account)
    {
      string sql = "DELETE FROM `trade_account` WHERE `sub_account` = @sub_account";
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          sub_account = sub_account
        });
        return writeConntion.Execute(sql, (object) parameters);
      }
    }

    public static int FindActiveCount(DateTime dt, int member)
    {
      try
      {
        string sql = "SELECT Count(*) FROM `trade_account` WHERE member_fk = @id AND loan_type <> 'trial' \n                AND ( close_time IS NULL OR close_time >= @tradeDate )";
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            id = member,
            tradeDate = dt
          });
          return readConnection.QueryFirstOrDefault<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][FindActiveCount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int GetAccountCountWithoudTrial(int member)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(80, 1);
      interpolatedStringHandler.AppendLiteral("SELECT count(*) FROM `trade_account` WHERE member_fk = ");
      interpolatedStringHandler.AppendFormatted<int>(member);
      interpolatedStringHandler.AppendLiteral(" AND loan_type <> 'trial'");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<int>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][GetAccountCountWithoudTrial]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void CloseTradeAccount(string sub_account)
    {
      string sql = "UPDATE `trade_account` SET `status` = 3, `margin_float` = 0,`mem_money` = 0, `close_time` = UTC_TIMESTAMP WHERE `sub_account` = @sub_account";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][CloseTradeAccount]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void UpdateFrozenMoney(string sub_account, Decimal frozen_money)
    {
      string sql = "UPDATE `trade_account` SET `frozen_money` = @frozen_money WHERE `sub_account` = @sub_account";
      var data = new
      {
        sub_account = sub_account,
        frozen_money = frozen_money
      };
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][UpdateFrozenMoney]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void AddFrozenMoney(string sub_account, Decimal frozen_money)
    {
      string sql = "UPDATE `trade_account` SET `frozen_money` = `frozen_money` + @frozen_money WHERE `sub_account` = @sub_account";
      var data = new
      {
        sub_account = sub_account,
        frozen_money = frozen_money
      };
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][AddFrozenMoney]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void ReduceFrozenMoney(string sub_account, Decimal frozen_money)
    {
      string sql = "UPDATE `trade_account` SET `frozen_money` = `frozen_money` - @frozen_money WHERE `sub_account` = @sub_account";
      var data = new
      {
        sub_account = sub_account,
        frozen_money = frozen_money
      };
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][ReduceFrozenMoney]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateTradeAccountVolume(
      bool verifyStatus,
      string subAccount,
      Decimal frozen)
    {
      string sql = !verifyStatus ? "UPDATE trade_account AS t SET t.mem_money = frozen_money = t.frozen_money - @frozen WHERE t.sub_account = @sub_account " : "UPDATE trade_account AS t SET t.mem_money = t.mem_money - @frozen, frozen_money = t.frozen_money - @frozen WHERE t.sub_account = @sub_account ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            sub_account = subAccount,
            frozen = frozen
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][UpdateTradeAccountVolume]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateMoney(string subAccount, Decimal money)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE trade_account SET mem_money = mem_money + @money\nWHERE sub_account = @sub_account;", (object) new
          {
            sub_account = subAccount,
            money = Convert.ToInt32(money)
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][UpdateMoney]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int InAdvanceClose(string subAccount, int close_type)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE trade_account SET status = @status, close_type=@close_type WHERE sub_account = @subAccount;", (object) new
          {
            subAccount = subAccount,
            status = 2,
            close_type = close_type
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][InAdvanceClose]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateExpandBorrow(
      string subAccount,
      Decimal effectMoney,
      Decimal multiple,
      Decimal warninglineMultiple,
      Decimal breaklineMultiple)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE trade_account SET warningline = (loan_money + @finance) + ((margin + @effectMoney) * @warninglineMultiple),breakline = (loan_money + @finance) + ((margin + @effectMoney) * @breaklineMultiple), mem_money = mem_money + @effectMoney + @finance, margin = margin + @effectMoney, margin_float = margin_float + @effectMoney, loan_money = loan_money + @finance WHERE sub_account = @subAccount;", (object) new
          {
            subAccount = subAccount,
            effectMoney = effectMoney,
            finance = (effectMoney * multiple),
            warninglineMultiple = warninglineMultiple,
            breaklineMultiple = breaklineMultiple
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][UpdateExpandBorrow]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static Decimal GetMarginFloat(int member_fk)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<Decimal>("SELECT IFNULL(SUM(margin_float), 0) FROM trade_account WHERE member_fk = @member_fk AND status = 0", (object) new
          {
            member_fk = member_fk
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeAccountService][GetMarginFloat]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void ReleaseForzen(string sub_account, Decimal frozen)
    {
      string sql = "UPDATE trade_account SET frozen_money = frozen_money - @frozen WHERE sub_account = @sub_account";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            frozen = frozen,
            sub_account = sub_account
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Fatal(ex.Message, ex);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
