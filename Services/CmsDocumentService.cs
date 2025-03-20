// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.CmsDocumentService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Info;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class CmsDocumentService
  {
    public static DocResponse GetDoc(int id, string lang)
    {
      string sql1 = "SELECT * FROM `cms_document` WHERE pk = @id AND lang = @lang";
      var data = new{ id = id, lang = lang };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DocResponse doc = readConnection.QuerySingleOrDefault<DocResponse>(sql1, (object) data);
          if (doc == null || doc.trash == 1 || !doc.status.HasValue || doc.status.GetValueOrDefault() != 1)
            throw new AppException(2170, "invalid_data");
          string sql2 = "UPDATE `cms_document` SET view = view + 1\n                            WHERE pk = @id AND lang = @lang";
          readConnection.Execute(sql2, (object) data);
          return doc;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsDocumentService][GetDoc]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static CmsDocumentDto? GetDocByCid(string cid, string lang)
    {
      string sql = "SELECT * FROM `cms_document` WHERE cid = @cid AND lang = @lang AND trash = 0 AND status = 1";
      var data = new{ cid = cid, lang = lang };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<CmsDocumentDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error(ex);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void UpdateDocView(int pk)
    {
      string sql = "UPDATE `cms_document` SET view = view + 1 WHERE pk = @pk";
      var data = new{ pk = pk };
      try
      {
        using (IDbConnection connection = DapperMysql.GetConnection())
          connection.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsDocumentService][UpdateDocView]" + ex.Message);
        throw new AppException(1030, "read_db_exception");
      }
    }
  }
}
