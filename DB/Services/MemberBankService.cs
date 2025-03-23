// Decompiled with JetBrains decompiler
// Type: DB.Services.MemberBankService
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
using tradeapi.Models.Wallet;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class MemberBankService
  {
    public static MemberBankDto Find(string card_pk)
    {
      string sql = "SELECT * FROM `member_bank` WHERE `card_pk` = @card_pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            card_pk = card_pk
          });
          return readConnection.QueryFirstOrDefault<MemberBankDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberBankService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<GetBankCardResponse> Find(int member_fk)
    {
      string sql = "SELECT * FROM `member_bank` WHERE `member_fk` = @member_fk AND is_delete = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<GetBankCardResponse>(sql, (object) parameters).AsList<GetBankCardResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberBankService][Find List]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<GetBankCardTypeResponse> GetCardType(int member_fk)
    {
      string sql = "SELECT card_type, currency FROM `member_bank` WHERE `member_fk` = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<GetBankCardTypeResponse>(sql, (object) parameters).AsList<GetBankCardTypeResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberBankService][GetCardType]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<MemberBankDto> FindAll()
    {
      string sql = "SELECT * FROM `member_bank`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<MemberBankDto>(sql).AsList<MemberBankDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberBankService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(MemberBankDto model)
    {
      string sql = "INSERT INTO `member_bank` (\n                `member_fk`, `card_pk`, `card_type`, `currency`, `country`, `bank`, `branch`, `card`, `account`, `cms_files_fk`, `is_confirm`, `is_delete`, `create_ip`, `create_time`)\n                VALUES (@member_fk, @card_pk, @card_type, @currency, @country, @bank, @branch, @card, @account, @cms_files_fk, @is_confirm, @is_delete, @create_ip, @create_time); ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberBankService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(MemberBankDto model)
    {
      string sql = "UPDATE `member_bank` SET \n                `member_fk` = @member_fk,\n                `card_type` = @card_type,\n                `currency` = @currency,\n                `country` = @country,\n                `bank` = @bank,\n                `branch` = @branch,\n                `card` = @card,\n                `account` = @account,\n                `cms_files_fk` = @cms_files_fk,\n                `is_delete` = @is_delete,\n                `create_ip` = @create_ip,\n                `create_time` = @create_time\n                 WHERE `card_pk` = @card_pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberBankService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(string card_pk)
    {
      string sql = "UPDATE `member_bank` SET `is_delete` = 1 WHERE `card_pk` = @card_pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            card_pk = card_pk
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberBankService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
