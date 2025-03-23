// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.AccountServices
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Account;
using tradeapi.Models.Dto;
using tradeapi.Models.Peizi;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class AccountServices
  {
    public AccountDto Get(string id)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "select * from trade_account where sub_account = @account";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            account = id
          });
          return readConnection.QuerySingleOrDefault<AccountDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][Get]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public void Insert(CreateAccountRequest param)
    {
      string sql = "INSERT INTO `trade_account` \n\t            (`sub_account`, `sub_pwd`, `member_fk`, `mem_name`, `type`, `flag_first`, `loan_type`, \n\t            `mem_money`, `frozen_money`, `margin`, `loan_money`, `begin_time`, `end_time`,\n\t            `status`, `warningline`, `breakline`, `agent_id`, `agent_name`, `live_state` ) VALUES\n\t            ( @sub_account, @sub_pwd, @member_fk, @mem_name, @type, 0, @loan_type, \n\t            @mem_money, 0, @margin, @loan_money, @begin_time, @end_time, \n\t             0, @warningline, @breakline, @agent_id, @agent_name, 2 );";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) param);
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public int ExtendTime(string sub_account, DateTime new_end_time)
    {
      string sql = "UPDATE trade_account SET end_time = @new_end_time, status = 0, close_time = null WHERE sub_account = @sub_account";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = sub_account,
            new_end_time = new_end_time
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][ExtendTime]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public int MarginChange(string account, Decimal expand)
    {
      string sql = "UPDATE `trade_account` SET\t\n\t            `mem_money` = mem_money + (@append_margin)\n\t            WHERE `sub_account` = @sub_account ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = account,
            append_margin = expand
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][MarginChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public void ExpandFunding(
      string account,
      double expand,
      double loan,
      double warning,
      double beakout)
    {
      string sql = "UPDATE `trade_account` SET\t\n\t            `mem_money` = mem_money + @append_margin + @append_loan,\n                `margin` = `margin` + @append_margin,\n\t            `loan_money` = loan_money + @append_loan,\n\t            `status` = 0,\n\t            `warningline` = @warningline,\n\t            `breakline` = @breakline\n\t            WHERE `sub_account` = @sub_account";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = account,
            append_margin = expand,
            append_loan = loan,
            warningline = warning,
            breakline = beakout
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][ExpandFunding]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public SimpleAccountResponse FindDefaultAccount(int member_fk)
    {
      string sql = "SELECT trade_account.sub_account, trade_account.status, trade_account.market \nFROM trade_account \nINNER JOIN `member` ON  trade_account.member_fk = member.pk\nWHERE\n   member.pk = @member_fk\n   AND trade_account.sub_account = member.sub_account";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QueryFirstOrDefault<SimpleAccountResponse>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][FindDefaultAccount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public void SetDefaultAccount(int memberid, string account)
    {
      try
      {
        string sql1 = "update trade_account set flag_first = 0 where member_fk = @memid";
        string sql2 = "update trade_account set flag_first = 1 where sub_account = @id";
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            memid = memberid,
            id = account
          });
          readConnection.Execute(sql1, (object) parameters);
          readConnection.Execute(sql2, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][SetDefaultAccount]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public List<SimpleAccountResponse> GetAccounts(int memberid)
    {
      string sql = "SELECT sub_account, status\n                FROM trade_account               \n                WHERE member_fk = @memid AND (close_time IS NULL OR date_add(end_time, interval 7 day) >= now() )\n                ORDER BY status";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            memid = memberid
          });
          return readConnection.Query<SimpleAccountResponse>(sql, (object) parameters).ToList<SimpleAccountResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][GetAccounts]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public List<AccountResponse> FindAccountByMemberId(int memberid)
    {
      string sql = "SELECT trade_account.sub_account, loan_type, status, loan_money, mem_money, margin,\n                mem_money+IFNULL(total, 0) AS balance, warningline, breakline, frozen_money, \n                IFNULL(total, 0) AS total, IFNULL(income, 0) AS income, begin_time, end_time, close_time\n                FROM trade_account\n                LEFT JOIN vm_trade_fullhold ON vm_trade_fullhold.sub_account = trade_account.sub_account\n                WHERE member_fk = @memid AND (close_time IS NULL OR close_time >= @maxdate )\n                ORDER BY close_time";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            memid = memberid,
            maxdate = DateTime.Today.AddDays(-31.0)
          });
          return readConnection.Query<AccountResponse>(sql, (object) parameters).ToList<AccountResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][FindAccountByMemberId]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public AccountResponse FindAccount(string subaccount)
    {
      string sql = "SELECT trade_account.sub_account, loan_type, status, mem_money, margin, loan_money,\n                mem_money+IFNULL(total, 0) AS balance, warningline, breakline, frozen_money, \n                IFNULL(total, 0) AS total, IFNULL(income, 0) AS income, begin_time, end_time, close_time\n                FROM trade_account\n                LEFT JOIN vm_trade_fullhold ON vm_trade_fullhold.sub_account = trade_account.sub_account\n                WHERE trade_account.sub_account = @account";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            account = subaccount
          });
          return readConnection.QuerySingleOrDefault<AccountResponse>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][FindAccount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public int FindCountsByMemberId(int memberid)
    {
      string sql = "SELECT count(*) as number FROM trade_account \n                WHERE member_fk = @memid AND (close_time IS NULL OR close_time >= @maxdate )";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            memid = memberid,
            maxdate = DateTime.Today.AddDays(-31.0)
          });
          return readConnection.QuerySingle<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][FindCountsByMemberId]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public void SetToken(string account, string token)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "update trade_account set token = @token where sub_account = @account";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            token = token,
            account = account
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][SetToken]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public string GetToken(string account)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "select token from trade_account where sub_account = @account";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            account = account
          });
          return readConnection.QuerySingleOrDefault<string>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][GetToken]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public void ClearToken(string token)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "update trade_account set token = null where token = @token";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            token = token
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][ClearToken]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public void LockAccount(string account)
    {
      string sql = "UPDATE trade_account SET status = status + 8 WHERE sub_account = @sub_account";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = account
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][LockAccount]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public void LockMoney(string account, Decimal money)
    {
      string sql = "UPDATE trade_account SET frozen_money = frozen_money + @frozen WHERE sub_account = @sub_account";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            frozen = money,
            sub_account = account
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][LockMoney]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public bool UnLockMoney(string account, Decimal money)
    {
      string sql = "UPDATE trade_account SET frozen_money = frozen_money - @frozen WHERE sub_account = @sub_account and frozen_money >= @frozen";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            frozen = money,
            sub_account = account
          });
          return writeConntion.Execute(sql, (object) parameters) == 1;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][UnLockMoney]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public bool Withdaw(string account, Decimal money)
    {
      string sql = "UPDATE trade_account SET frozen_money = frozen_money - @money, \n            mem_money = mem_money - @money \n            WHERE sub_account = @sub_account and frozen_money >= @frozen";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            money = money,
            sub_account = account
          });
          return writeConntion.Execute(sql, (object) parameters) == 1;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][Withdaw]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public Decimal FindFreeMoney(string account)
    {
      string sql = "SELECT (mem_money - frozen_money) as number FROM trade_account \n                WHERE sub_account = @memid and status = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            memid = account
          });
          return readConnection.QuerySingleOrDefault<Decimal>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][FindFreeMoney]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public void EarlyTerminate(string account)
    {
      string sql = "UPDATE trade_account SET end_time = Now(), status = 4 WHERE sub_account = @sub_account";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_account = account
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AccountServices][EarlyTerminate]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
