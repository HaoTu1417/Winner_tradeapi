// Decompiled with JetBrains decompiler
// Type: DB.Services.CmsAdvertiseService
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
  public class CmsAdvertiseService
  {
    public static CmsAdvertiseDto Find(int cms_files_fk)
    {
      string sql = "SELECT * FROM `cms_advertise` WHERE `cms_files_fk` = @cms_files_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            cms_files_fk = cms_files_fk
          });
          return readConnection.QueryFirstOrDefault<CmsAdvertiseDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsAdvertiseService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<CmsAdvertiseDto> FindAll()
    {
      string sql = "SELECT * FROM `cms_advertise`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<CmsAdvertiseDto>(sql).AsList<CmsAdvertiseDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsAdvertiseService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(CmsAdvertiseDto model)
    {
      string sql = "INSERT INTO `cms_advertise` (\r\n\t\t\t\t`cms_files_fk`, `enable`, `sort`, `url`, `size`, `lang`)\r\n\t\t\t\tVALUES (@cms_files_fk, @enable, @sort, @url, @size, @lang); ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsAdvertiseService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(CmsAdvertiseDto model)
    {
      string sql = "UPDATE `cms_advertise` SET \r\n\t\t\t\t`enable` = @enable,\r\n\t\t\t\t`sort` = @sort,\r\n\t\t\t\t`url` = @url,\r\n\t\t\t\t`size` = @size\r\n\t\t\t\t`lang` = @lang\r\n\t\t\t\t WHERE `cms_files_fk` = @cms_files_fk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsAdvertiseService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int cms_files_fk)
    {
      string sql = "DELETE FROM `cms_advertise` WHERE `cms_files_fk` = @cms_files_fk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            cms_files_fk = cms_files_fk
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsAdvertiseService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static CmsAdvertiseDto FindByLang(string lang)
    {
      string sql = "SELECT *, cms_files.url FROM `cms_advertise`\r\n                           INNER JOIN cms_files on cms_files.pk = cms_advertise.cms_files_fk\r\n                           WHERE lang = @lang\r\n                           ORDER BY pk DESC LIMIT 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            lang = lang
          });
          return readConnection.QuerySingleOrDefault<CmsAdvertiseDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsAdvertiseService][FindByLang]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
