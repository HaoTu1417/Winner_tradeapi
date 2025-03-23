// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.CmsFilesServices
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class CmsFilesServices
  {
    public static List<CmsFilesDto> GetAllCmsFilesByTableKey(string table, int key)
    {
      string sql = "\n                   select * from cms_files\n                   where `table` = @table and `key` = @key\n                  \n                    ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            table = table,
            key = key
          });
          return readConnection.Query<CmsFilesDto>(sql, (object) parameters).ToList<CmsFilesDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsFilesServices][GetAllCmsFilesByTableKey]");
        LogLib.Error(ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int InsertCmsFiles(CmsFilesDto item)
    {
      string sql = "\n                INSERT INTO cms_files ( url,file_type,`table`,`key`)\n                VALUES ( @url,@file_type,@table,@key);\n                SELECT LAST_INSERT_ID();";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            url = item.url,
            file_type = item.file_type,
            table = item.table,
            key = item.key
          });
          return writeConntion.ExecuteScalar<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsFilesServices][InsertCmsFiles]");
        LogLib.Error(ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void DeleteCmsFiles(int pk)
    {
      string sql = "\n                DELETE FROM cms_files\n                where pk = @pk;\n                ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsFilesServices][DeleteCmsFiles]");
        LogLib.Error(ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateKey(int pk, int key)
    {
      string sql = "UPDATE `cms_files` SET \n\t\t\t\t`key` = @key\n\t\t\t\t WHERE `pk` = @pk";
      DynamicParameters parameters = DapperMysql.GetParameters((object) new
      {
        pk = pk,
        key = key
      });
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) parameters);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsFilesServices][UpdateKey]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
