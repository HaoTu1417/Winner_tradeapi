// Decompiled with JetBrains decompiler
// Type: DB.Services.SysCountryService
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
  public class SysCountryService
  {
    public static SysCountryDto Find(string pk)
    {
      string sql = "SELECT * FROM `sys_country` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<SysCountryDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysCountryService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<SysCountryDto> FindAll(string lang)
    {
      string sql = "SELECT sys_country.pk pk, mutilang_table.value label, mutilang_table.lang lang, sys_country.enable, sys_country.currency, sys_country.flag, sys_country.code\n                           FROM `sys_country` INNER JOIN `mutilang_table` ON mutilang_table.dbtable = 'sys_country' AND sys_country.pk = mutilang_table.key AND mutilang_table.lang = @lang\n                           WHERE sys_country.enable = 1 ORDER BY CASE WHEN sys_country.`lang` = 'VN' THEN 0 ELSE 1 END";
      try
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          lang = lang
        });
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<SysCountryDto>(sql, (object) parameters).AsList<SysCountryDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysCountryService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<SysCountryDto> FindAll()
    {
      string sql = "SELECT * FROM sys_country WHERE enable = 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<SysCountryDto>(sql).AsList<SysCountryDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysCountryService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(SysCountryDto model)
    {
      string sql = "INSERT INTO `sys_country` (\n                `pk`, `label`, `enable`, `lang`, `currency`, `flag`, `code`)\n                VALUES (@pk, @label, @enable, @lang, @currency, @flag, @code); ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysCountryService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(SysCountryDto model)
    {
      string sql = "UPDATE `sys_country` SET \n                `label` = @label,\n                `enable` = @enable,\n                `lang` = @lang,\n                `currency` = @currency,\n                `flag` = @flag,\n                `code` = @code\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysCountryService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(string pk)
    {
      string sql = "DELETE FROM `sys_country` WHERE `pk` = @pk";
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
        LogLib.Error("[SysCountryService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
