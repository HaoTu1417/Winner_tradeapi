// Decompiled with JetBrains decompiler
// Type: DB.Services.CmsPopinfoService
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
  public class CmsPopinfoService
  {
    public static CmsPopinfoDto Find(int pk)
    {
      string sql = "SELECT * FROM `cms_popinfo` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<CmsPopinfoDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsPopinfoService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static CmsPopinfoDto Find(string lang)
    {
      string sql = "SELECT * FROM `cms_popinfo` WHERE `lang` = @lang";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            lang = lang
          });
          return readConnection.QueryFirstOrDefault<CmsPopinfoDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsPopinfoService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<CmsPopinfoDto> FindAll()
    {
      string sql = "SELECT * FROM `cms_popinfo`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<CmsPopinfoDto>(sql).AsList<CmsPopinfoDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsPopinfoService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(CmsPopinfoDto source)
    {
      string sql = "INSERT INTO `cms_popinfo` (\n                `lang`, `info`, `size`, `enable`)\n                VALUES (@lang, @info, @size, @enable);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsPopinfoService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(CmsPopinfoDto model)
    {
      string sql = "UPDATE `cms_popinfo` SET \n                `lang` = @lang,\n                `info` = @info,\n                `size` = @size,\n                `enable` = @enable\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsPopinfoService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `cms_popinfo` WHERE `pk` = @pk";
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
        LogLib.Error("[CmsPopinfoService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
