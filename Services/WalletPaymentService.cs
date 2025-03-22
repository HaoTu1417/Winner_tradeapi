// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WalletPaymentService
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
  public class WalletPaymentService
  {
    public static WalletPaymentDto Find(int pk)
    {
      string sql = "SELECT * FROM `wallet_payment` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<WalletPaymentDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletPaymentService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static WalletPaymentDto FindByPayCode(string payCode)
    {
      string sql = "SELECT * FROM `wallet_payment` WHERE `pay_code` = @payCode";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            payCode = payCode
          });
          return readConnection.QueryFirstOrDefault<WalletPaymentDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletPaymentService][FindByPayCode]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static WalletPaymentDto FindByPayType(string payCode, string payType)
    {
      string sql = "SELECT * FROM `wallet_payment` WHERE `pay_code` = @payCode AND `pay_type` = @payType";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            payCode = payCode,
            payType = payType
          });
          return readConnection.QueryFirstOrDefault<WalletPaymentDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletPaymentService][FindByPayType]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<WalletPaymentDto> FindAll()
    {
      string sql = "SELECT * FROM `wallet_payment`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletPaymentDto>(sql).AsList<WalletPaymentDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletPaymentService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<CollectInfoResponse> GetCollectInfo()
    {
      string sql = "SELECT pay_code as card, pay_name as payee, currency, min_recharge, fastbtn FROM `wallet_payment` WHERE status = 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<CollectInfoResponse>(sql).AsList<CollectInfoResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletPaymentService][GetCollectInfo]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<GetBankCardTypeResponse> GetCardType()
    {
      string sql = "SELECT DISTINCT pay_name as name, currency FROM `wallet_payment` WHERE status = 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<GetBankCardTypeResponse>(sql).AsList<GetBankCardTypeResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletPaymentService][GetCardType]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static string FindRedirectUrl(string pay_type)
    {
      string sql = "SELECT pay_url FROM `wallet_payment` WHERE status = 1 and pay_type = @pay_type";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pay_type = pay_type
          });
          return readConnection.QueryFirstOrDefault<string>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletPaymentService][FindRedirectUrl]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
