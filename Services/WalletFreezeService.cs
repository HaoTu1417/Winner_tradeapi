// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WalletFreezeService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Wallet;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class WalletFreezeService
  {
    public static void Insert(WalletFreezeDto walletfreezeDto)
    {
      string sql = "INSERT INTO `wallet_freeze`  (`member_fk`, `sn`, `freeze`, `subtype`, `create_time`) \n                           VALUES (@member_fk, @sn, @freeze, @subtype, @create_time)";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) walletfreezeDto);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletFreezeService][Insert]" + ex.Message);
        throw new AppException(1040, "write_db_exception");
      }
    }

    public static List<FreezehistoryResponse> FindFreezeHistory(int member_fk, string lang)
    {
      string sql = "SELECT *, message_template.title as type FROM `wallet_freeze`\n                           INNER JOIN message_template ON wallet_freeze.subtype = message_template.temp_id AND message_template.lang = @lang\n                           WHERE wallet_freeze.member_fk = @member_fk  ORDER by wallet_freeze.create_time DESC";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            lang = lang
          });
          return writeConntion.Query<FreezehistoryResponse>(sql, (object) parameters).AsList<FreezehistoryResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletFreezeService][FindFreezeHistory]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public void Delete(int id)
    {
      string sql = "DELETE FROM wallet_freeze WHERE pk = @id";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            id = id
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletFreezeService][Delete]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public WalletFreezeDto FindWalletFreezeBySn(string sn)
    {
      string sql = "SELECT * FROM wallet_freeze WHERE sn = @sn ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sn = sn
          });
          return readConnection.QuerySingleOrDefault<WalletFreezeDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletFreezeService][FindWalletFreezeBySn]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void DeleteBySN(int member_fk, string sn)
    {
      string sql = "DELETE FROM wallet_freeze WHERE member_fk = @member_fk and sn= @sn";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            sn = sn
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletFreezeService][Delete]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
