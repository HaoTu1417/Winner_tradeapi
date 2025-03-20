// Decompiled with JetBrains decompiler
// Type: DB.Services.CmsMarqService
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
  public class CmsMarqService
  {
    public static CmsMarqDto Find(int pk)
    {
      string sql = "SELECT * FROM `cms_marq` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<CmsMarqDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsMarqService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<string> FindAll(string lang)
    {
      string sql = "SELECT msg FROM `cms_marq` WHERE lang = @lang";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            lang = lang
          });
          return readConnection.Query<string>(sql, (object) parameters).AsList<string>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsMarqService][FindAll(string lang)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<CmsMarqDto> FindAll()
    {
      string sql = "SELECT * FROM `cms_marq`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<CmsMarqDto>(sql).AsList<CmsMarqDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsMarqService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(CmsMarqDto source)
    {
      string sql = "INSERT INTO `cms_marq` (\n\t\t\t\t`lang`, `enable`, `msg`)\n\t\t\t\tVALUES (@lang, @enable, @msg);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsMarqService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(CmsMarqDto model)
    {
      string sql = "UPDATE `cms_marq` SET \n\t\t\t\t`lang` = @lang,\n\t\t\t\t`enable` = @enable,\n\t\t\t\t`msg` = @msg\n\t\t\t\t WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsMarqService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `cms_marq` WHERE `pk` = @pk";
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
        LogLib.Error("[CmsMarqService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
