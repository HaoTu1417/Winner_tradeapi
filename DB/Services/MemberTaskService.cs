// Decompiled with JetBrains decompiler
// Type: DB.Services.MemberTaskService
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
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class MemberTaskService
  {
    public static MemberTaskDto Find(int pk)
    {
      string sql = "SELECT * FROM `member_task` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<MemberTaskDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberTaskService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static MemberTaskDto FindBySubType_Currency_Lang(
      int sub_type,
      string lang,
      string currency)
    {
      string sql = "SELECT * FROM `member_task` \nWHERE `sub_type` = @sub_type AND `lang` = @lang AND `currency` = @currency";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_type = sub_type,
            lang = lang,
            currency = currency
          });
          return readConnection.QueryFirstOrDefault<MemberTaskDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberTaskService][FindBySubType_Currency_Lang]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static MemberTaskDto FindBySubType_Currency(int sub_type, string currency)
    {
      string sql = "SELECT * FROM `member_task` \nWHERE `sub_type` = @sub_type AND `currency` = @currency";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            sub_type = sub_type,
            currency = currency
          });
          return readConnection.QueryFirstOrDefault<MemberTaskDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberTaskService][FindBySubType_Currency]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<MemberTaskDto> FindAll()
    {
      string sql = "SELECT * FROM `member_task` ORDER BY `sub_type`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<MemberTaskDto>(sql).AsList<MemberTaskDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberTaskService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(MemberTaskDto model)
    {
      string sql = "INSERT INTO `member_task` (\n                `pk`, `sub_type`, `currency`, `lang`, `coin`, `title`, `content`)\n                VALUES (@pk, @sub_type, @currency, @lang, @coin, @title, @content); ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberTaskService][Insert]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateFull(MemberTaskDto model)
    {
      string sql = "UPDATE `member_task` SET \n                `sub_type` = @sub_type,\n                `currency` = @currency,\n                `lang` = @lang,\n                `coin` = @coin,\n                `title` = @title,\n                `content` = @content\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberTaskService][UpdateFull]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `member_task` WHERE `pk` = @pk";
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
        LogLib.Error("[MemberTaskService][Remove]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<MemberTaskDto> FindAllByLang(string lang, string currency)
    {
      string sql = "SELECT * FROM `member_task` WHERE `lang` = @lang AND `currency` = @currency ORDER BY `sub_type`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<MemberTaskDto>(sql, (object) new
          {
            lang = lang,
            currency = currency
          }).AsList<MemberTaskDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error(ex);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
