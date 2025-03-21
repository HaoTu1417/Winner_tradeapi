// Decompiled with JetBrains decompiler
// Type: DB.Services.MutilangSubjectService
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
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class MutilangSubjectService
  {
    public static MutilangSubjectDto Find(string lang)
    {
      string sql = "SELECT * FROM `mutilang_subject` WHERE `lang` = @lang";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            lang = lang
          });
          return readConnection.QueryFirstOrDefault<MutilangSubjectDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MutilangSubjectService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<MutilangSubjectDto> FindAll()
    {
      string sql = "SELECT * FROM `mutilang_subject` WHERE mutilang_subject.enable = 1 ORDER BY CASE WHEN mutilang_subject.`lang` = 'VN' THEN 0 ELSE 1 END";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<MutilangSubjectDto>(sql).AsList<MutilangSubjectDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[MutilangSubjectService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static MutilangSubjectDto FindAdminDefault()
    {
      string sql = "SELECT * FROM `mutilang_subject` WHERE `admin_default` = 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<MutilangSubjectDto>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MutilangSubjectService][FindAdminDefault]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(MutilangSubjectDto model)
    {
      string sql = "INSERT INTO `mutilang_subject` (\n                `lang`, `title`, `enable`, `icon`, `admin_default`, `app_default`)\n                VALUES (@lang, @title, @enable, @icon, @admin_default, @app_default); ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MutilangSubjectService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(MutilangSubjectDto model)
    {
      string sql = "UPDATE `mutilang_subject` SET \n                `title` = @title,\n                `enable` = @enable,\n                `icon` = @icon,\n                `admin_default` = @admin_default,\n                `app_default` = @app_default\n                 WHERE `lang` = @lang";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MutilangSubjectService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(string lang)
    {
      string sql = "DELETE FROM `mutilang_subject` WHERE `lang` = @lang";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            lang = lang
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MutilangSubjectService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
