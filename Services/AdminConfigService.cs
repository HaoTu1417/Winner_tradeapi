// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.AdminConfigService
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
namespace tradeapi.Services
{
  public class AdminConfigService
  {
    public static AdminConfigDto Find(string name)
    {
      string sql = "SELECT * FROM `admin_config` WHERE `name` = @name";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            name = name
          });
          return readConnection.QueryFirstOrDefault<AdminConfigDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminConfigService][Find]" + ex.Message);
        return (AdminConfigDto) null;
      }
    }

    public static List<AdminConfigDto> FindAll()
    {
      string sql = "SELECT * FROM `admin_config`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<AdminConfigDto>(sql).AsList<AdminConfigDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminConfigService][FindAll]" + ex.Message);
        return (List<AdminConfigDto>) null;
      }
    }

    public static int Insert(AdminConfigDto model)
    {
      string sql = "INSERT INTO `admin_config` (\n                `name`, `title`, `group`, `type`, `value`, `options`, `tips`, `ajax_url`, `next_items`, `param`, `format`, `table`, `level`, `key`, `option`, `pid`, `ak`, `sort`, `status`)\n                VALUES (@name, @title, @group, @type, @value, @options, @tips, @ajax_url, @next_items, @param, @format, @table, @level, @key, @option, @pid, @ak, @sort, @status); ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminConfigService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(AdminConfigDto model)
    {
      string sql = "UPDATE `admin_config` SET \n                `title` = @title,\n                `group` = @group,\n                `type` = @type,\n                `value` = @value,\n                `options` = @options,\n                `tips` = @tips,\n                `ajax_url` = @ajax_url,\n                `next_items` = @next_items,\n                `param` = @param,\n                `format` = @format,\n                `table` = @table,\n                `level` = @level,\n                `key` = @key,\n                `option` = @option,\n                `pid` = @pid,\n                `ak` = @ak,\n                `sort` = @sort,\n                `status` = @status\n                 WHERE `name` = @name";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminConfigService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateConfig(AdminConfigDto model)
    {
      string sql = "UPDATE `admin_config` SET \n                `title` = @title,\n                `group` = @group,\n                `value` = @value,\n                `tips` = @tips,\n                `format` = @format,\n                `sort` = @sort,\n                `status` = @status\n                 WHERE `name` = @name";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminConfigService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(string name)
    {
      string sql = "DELETE FROM `admin_config` WHERE `name` = @name";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            name = name
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[AdminConfigService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
