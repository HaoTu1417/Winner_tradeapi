// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.AdminBankService
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
using tradeapi.Models.Wallet;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class AdminBankService
  {
    public static List<CollectInfoResponse> FindActiveAccount()
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM admin_bank\n                                   WHERE is_delete = 0 AND status = 1";
          return readConnection.Query<CollectInfoResponse>(sql).AsList<CollectInfoResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][FindActiveAccount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<CollectInfoResponse> Find(string country)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM admin_bank\n                                   WHERE country = @country AND status = 1";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            country = country
          });
          return readConnection.Query<CollectInfoResponse>(sql, (object) parameters).AsList<CollectInfoResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][Find(string country)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static CollectInfoResponse Find(int type, string currency)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM admin_bank\n                                   WHERE type = @type AND currency = @currency AND status = 1";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            type = type,
            currency = currency
          });
          return readConnection.QueryFirstOrDefault<CollectInfoResponse>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][Find(int type, string currency)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<string> FindAvailableCurrency(int type)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT DISTINCT currency FROM admin_bank\n                                   WHERE type = @type AND status = 1";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            type = type
          });
          return readConnection.Query<string>(sql, (object) parameters).AsList<string>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][FindAvailableCurrency]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<GetBankCardTypeResponse> GetCardType(string country)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT *, admin_bank.type card_type, admin_bank.bank_name as name FROM admin_bank\n                                    WHERE is_delete = 0 AND status = 1";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            country = country
          });
          return readConnection.Query<GetBankCardTypeResponse>(sql, (object) parameters).AsList<GetBankCardTypeResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][GetCardType]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static AdminBankDto Find(string currency, string account_number, string payee)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM admin_bank\n                                   WHERE currency = @currency AND card = @account_number AND payee = @payee AND status = 1";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            currency = currency,
            account_number = account_number,
            payee = payee
          });
          return readConnection.QuerySingleOrDefault<AdminBankDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][Find(string currency, string account_number, string payee)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkByAccountNumber(string account_number)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT pk FROM admin_bank\n                                   WHERE card = @account_number";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            account_number = account_number
          });
          return readConnection.ExecuteScalar<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][FindPkByAccountNumber]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void Insert(AdminBankDto param)
    {
      string sql = "INSERT INTO `admin_bank` \n                (type, country, currency, card, swift, bank_name, open_bank, payee, notes, status, image, viplists, bankimgid)\n                VALUES\n                (@type, @country, @currency, @card, @swift, @bank_name, @open_bank, @payee, @notes, @status, @image, @viplists, @bankimgid)";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) param);
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminBankService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
