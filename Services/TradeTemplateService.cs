// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.TradeTemplateService
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
namespace tradeapi.Services
{
  public class TradeTemplateService
  {
    public static TradeTemplateDto GetByTempId(int tempId, string lang)
    {
      string sql = "SELECT * FROM `trade_template` WHERE `temp_id` = @tempId AND `lang` = @lang;";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            tempId = tempId,
            lang = lang
          });
          return readConnection.QueryFirstOrDefault<TradeTemplateDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeTemplateService][GetByTempId]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static TradeTemplateDto Find(int pk)
    {
      string sql = "SELECT * FROM `trade_template` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<TradeTemplateDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeTemplateService][Find]" + ex.Message);
        return (TradeTemplateDto) null;
      }
    }

    public static List<TradeTemplateDto> FindAll()
    {
      string sql = "SELECT * FROM `trade_template`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeTemplateDto>(sql).AsList<TradeTemplateDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeTemplateService][FindAll]" + ex.Message);
        return (List<TradeTemplateDto>) null;
      }
    }

    public static List<TradeTemplateDto> FindDropDown(string lang)
    {
      string sql = "SELECT `temp_id`, `name` FROM trade_template\n                WHERE `lang` = @lang ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeTemplateDto>(sql, (object) new
          {
            lang = lang.ToUpper()
          }).AsList<TradeTemplateDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeTemplateService][FindDropDown]" + ex.Message);
        return (List<TradeTemplateDto>) null;
      }
    }

    public static int FindPkAfterInsert(TradeTemplateDto source)
    {
      string sql = "INSERT INTO `trade_template` (\n                `temp_id`, `lang`, `name`, `template`, `param`, `demo`)\n                VALUES (@temp_id, @lang, @name, @template, @param, @demo);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeTemplateService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(TradeTemplateDto model)
    {
      string sql = "UPDATE `trade_template` SET \n                `temp_id` = @temp_id,\n                `lang` = @lang,\n                `name` = @name,\n                `template` = @template,\n                `param` = @param,\n                `demo` = @demo\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[TradeTemplateService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `trade_template` WHERE `pk` = @pk";
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
        LogLib.Error("[TradeTemplateService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
