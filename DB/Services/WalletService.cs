// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WalletService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Wallet;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class WalletService
  {
    public static WalletDto Find(int pk)
    {
      string sql = string.Format("SELECT * FROM `wallet` WHERE member_fk = {0}", (object) pk);
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<WalletDto>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static Decimal GetCoupon(int pk)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
      interpolatedStringHandler.AppendLiteral("SELECT coupon FROM `wallet` WHERE member_fk = ");
      interpolatedStringHandler.AppendFormatted<int>(pk);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<Decimal>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][GetCoupon]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void BalanceMoneyChange(int member_fk, Decimal change)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE wallet \n                SET balance = balance + @change, \n                last_update_time = UTC_TIMESTAMP()\n                WHERE member_fk = @member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            change = change,
            member_fk = member_fk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][BalanceMoneyChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void FreezeMoneyChange(int member_fk, Decimal change)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE wallet \n                SET freeze = freeze + @change, last_update_time = UTC_TIMESTAMP()\n                WHERE member_fk = @member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            change = change,
            member_fk = member_fk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][FreezeMoneyChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void GmoneyMoneyChange(int member_fk, Decimal change)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE wallet \n                SET coupon = coupon + @change, last_update_time = UTC_TIMESTAMP()\n                WHERE member_fk = @member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            change = change,
            member_fk = member_fk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][GmoneyMoneyChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static WalletResponse FindWalletResponse(int pk)
    {
      string sql = string.Format("SELECT m.account as `account`, \n                m.id_auth,\n                m.level_id,\n                w.balance,\n                w.coupon,\n                w.freeze,\n                w.richbox_balance,\n                w.richbox_interest,\n                (w.balance - w.freeze) AS available,\n                w.currency\n                FROM `member` m \n                INNER JOIN `wallet` w ON w.member_fk = m.pk               \n                WHERE m.pk = {0}", (object) pk);
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<WalletResponse>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][FindWalletResponse]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void InsertWallet(WalletDto walletDto)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n            INSERT INTO wallet \n            (member_fk, currency, balance, freeze, richbox_balance, status, coupon, total_recharge, total_withdraw, last_update_time) \n            VALUES \n            (@member_fk, @currency, @balance, @freeze, @anxin_balance, @status, @coupon, @total_recharge, @total_withdraw, @last_update_time)";
          DynamicParameters parameters = DapperMysql.GetParameters((object) walletDto);
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][InsertWallet]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<WalletDto> FindWallet(int pk)
    {
      string sql = string.Format("SELECT * FROM `wallet` WHERE member_fk = {0}", (object) pk);
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletDto>(sql).AsList<WalletDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][FindWallet]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void CouponMoneyChange(int member_fk, Decimal change)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE wallet \n                SET coupon = coupon + @change\n                WHERE member_fk = @member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            change = change,
            member_fk = member_fk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][CouponMoneyChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static bool DebugBalance(int member)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(227, 1);
      interpolatedStringHandler.AppendLiteral("SELECT IFNULL((wallet.`balance` - SUM(wallet_record.affect)),0) AS b\n                FROM `wallet`\n                INNER JOIN wallet_record ON wallet_record.member_fk = wallet.member_fk\n                WHERE wallet.member_fk = ");
      interpolatedStringHandler.AppendFormatted<int>(member);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<Decimal>(stringAndClear) == 0M;
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][FindWalletList]" + ex.Message);
        return false;
      }
    }

    public static void RichBoxBalanceMoneyChange(int member_fk, Decimal change)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE wallet \n                SET richbox_balance = richbox_balance + @change, \n                last_update_time = UTC_TIMESTAMP()\n                WHERE member_fk = @member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            change = change,
            member_fk = member_fk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][RichBoxBalanceMoneyChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void RichBoxInterestChange(int member_fk, Decimal change)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE wallet \n                SET richbox_interest = richbox_interest + @change, \n                last_update_time = UTC_TIMESTAMP()\n                WHERE member_fk = @member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            change = change,
            member_fk = member_fk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][RichBoxInterestChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static Decimal GetRichBoxBalance(int pk)
    {
      string sql = string.Format("SELECT richbox_balance FROM `wallet` WHERE member_fk = {0}", (object) pk);
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<Decimal>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][GetRichBoxBalance]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<WalletDto> FindPositiveRichboxBalance()
    {
      string sql = string.Format("SELECT w.member_fk, w.richbox_balance, m.richbox_rate FROM `wallet` w\n                                         INNER JOIN member m ON w.member_fk = m.pk\n                                         WHERE w.richbox_balance > 0");
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletDto>(sql).AsList<WalletDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][FindPositiveRichboxBalance]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<WalletDto> FindAllWithRichBoxRate()
    {
      string sql = "SELECT *, m.richbox_rate  FROM `wallet` w\n                           INNER JOIN member m ON w.member_fk = m.pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletDto>(sql).AsList<WalletDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void RichBoxInterestChange(List<RichBoxRecordDto> records)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          StringBuilder stringBuilder = new StringBuilder("");
          foreach (RichBoxRecordDto record in records)
            stringBuilder.Append(string.Format("UPDATE wallet\n                                                  SET richbox_interest = richbox_interest + {0},\n                                                  richbox_balance = richbox_balance + {0},\n                                                  last_update_time = '{2}'\n                                                  WHERE member_fk = {3};", (object) record.affect, (object) record.balance, (object) record.create_time.ToString("yyyy-MM-dd HH:mm:ss"), (object) record.member_fk));
          writeConntion.Execute(stringBuilder.ToString());
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletService][RichBoxInterestChange]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
