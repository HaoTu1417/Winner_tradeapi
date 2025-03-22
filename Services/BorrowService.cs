// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.BorrowService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.SubAccount;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class BorrowService
  {
    public static int FindPkAfterInsert(BorrowDto borrowDto)
    {
      string sql = "INSERT INTO `borrow` (\n                `sub_account`, `borrow_plan_fk`, `member_fk`, `order_id`, `status`, `market`, `borrow_type`, `currency`, `deposit_money`, `init_money`, `multiple`, `auto_renewal`, `borrow_money`, `borrow_interest`, `repayment_type`, `borrow_duration`, `position`, `rate`, `total`, `trading_time`, `loss_warn_sms_send`, `stock_money`, `total_coupon`, `total_fee`, `total_interest`, `create_time`, `begin_time`, `end_time`, `verify_time`)\n                VALUES (@sub_account, @borrow_plan_fk, @member_fk, @order_id, @status, @market, @borrow_type, @currency, @deposit_money, @init_money, @multiple, @auto_renewal, @borrow_money, @borrow_interest, @repayment_type, @borrow_duration, @position, @rate, @total, @trading_time, @loss_warn_sms_send, @stock_money, @total_coupon, @total_fee, @total_interest, @create_time, @begin_time, @end_time, @verify_time);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) borrowDto);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<BorrowDto> GetBorrows(int member_id)
    {
      string sql = "SELECT * FROM `borrow` WHERE member_fk = @member_id";
      var data = new{ member_id = member_id };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<BorrowDto>(sql, (object) data).AsList<BorrowDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][GetBorrows(int member_id)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<BorrowDto> GetBorrows(int member_id, int status)
    {
      string sql = "SELECT * FROM `borrow` WHERE member_fk = @member_id AND status = @status";
      var data = new
      {
        member_id = member_id,
        status = status
      };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<BorrowDto>(sql, (object) data).AsList<BorrowDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][GetBorrows(int member_id, int status)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowDto GetBorrow(string sub_account)
    {
      string sql = "SELECT * FROM `borrow` WHERE sub_account = @sub_account";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingle<BorrowDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][GetBorrow]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void SetAutoRenewal(string sub_account, bool enable)
    {
      string sql = "UPDATE `borrow` SET `auto_renewal` = @enable WHERE `sub_account` = @sub_account";
      var data = new
      {
        sub_account = sub_account,
        enable = enable
      };
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][SetAutoRenewal]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static bool IsDuplicateOrderId(string order_id)
    {
      string sql = "SELECT COUNT(*) FROM `borrow` WHERE `order_id` = @order_id";
      var data = new{ order_id = order_id };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>(sql, (object) data) > 0;
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][IsDuplicateOrderId]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowApply GetBorrowApply(int id)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(891, 1);
      interpolatedStringHandler.AppendLiteral("SELECT m.time_zone AS time_zone, p.warning_line AS warning_line, ");
      interpolatedStringHandler.AppendLiteral("p.break_line AS break_line, b.pk AS pk, b.member_fk AS member_fk, b.borrow_plan_fk AS borrow_plan_fk, ");
      interpolatedStringHandler.AppendLiteral("m.account AS member_username, m.real_name AS member_real_name, ");
      interpolatedStringHandler.AppendLiteral("b.order_id AS order_id, b.`status` AS `status`, b.borrow_type AS borrow_type, b.currency AS currency, ");
      interpolatedStringHandler.AppendLiteral("b.market AS market, b.borrow_duration AS borrow_duration, b.auto_renewal AS auto_renewal, ");
      interpolatedStringHandler.AppendLiteral("b.begin_time AS begin_time, b.end_time AS end_time, b.deposit_money AS deposit_money, ");
      interpolatedStringHandler.AppendLiteral("b.borrow_money AS borrow_money, b.multiple AS multiple, b.rate AS rate, b.borrow_interest AS borrow_interest, ");
      interpolatedStringHandler.AppendLiteral("b.init_money AS init_money, b.total_coupon AS total_coupon, b.total_fee AS total_fee, b.create_time AS create_time, b.verify_time AS verify_time ");
      interpolatedStringHandler.AppendLiteral("FROM borrow AS b ");
      interpolatedStringHandler.AppendLiteral("LEFT JOIN member AS m ON m.pk = b.member_fk ");
      interpolatedStringHandler.AppendLiteral("LEFT JOIN borrow_plan AS p ON p.pk = b.borrow_plan_fk ");
      interpolatedStringHandler.AppendLiteral("WHERE b.pk = ");
      interpolatedStringHandler.AppendFormatted<int>(id);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = id
          });
          return readConnection.QuerySingle<BorrowApply>(stringAndClear, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][GetBorrowApply]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateStatus(int id, BorrowStatus state)
    {
      string sql = "UPDATE `borrow` SET status = @status, verify_time = @verify_time WHERE pk=@pk;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            pk = id,
            status = (int) state,
            verify_time = DateTime.UtcNow
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][UpdateStatus]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int AcceptOrder(int id, string subAccount, Decimal use_coupon)
    {
      string sql = "UPDATE `borrow` SET\n                sub_account = @subAccount,\n                total_coupon = @use_coupon,\n                total_fee = total_fee - @use_coupon,\n                status = 1,\n                verify_time = @verify_time\n                WHERE pk=@pk AND status = -1";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            pk = id,
            subAccount = subAccount,
            use_coupon = use_coupon,
            verify_time = DateTime.UtcNow
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][UpdateStatus]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int InAdvanceReset(int id, string subAccount)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE `borrow` SET stock_money = IFNULL((select sum(total_amount) from trade_deal WHERE sub_account = @subAccount), 0),\ntotal_fee=(select sum(total_cost) from trade_deal WHERE sub_account = @subAccount),\ntotal_coupon=(select sum(use_coupon) from  borrow_fee WHERE sub_account = @subAccount),\ntotal_interest=(select sum(borrow_fee) from borrow_fee WHERE sub_account = @subAccount)\nWHERE pk=@pk;", (object) new
          {
            pk = id,
            subAccount = subAccount
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][InAdvanceReset]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `borrow` WHERE `pk` = @pk";
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
        LogLib.Error("[BorrowService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void DisableAutoRenewal(int pk)
    {
      string sql = "UPDATE `borrow` SET auto_renewal = 0 WHERE pk = @pk";
      var data = new{ pk = pk };
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowService][DisableAutoRenewal]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
