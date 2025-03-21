// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.CmsBulletinService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Info;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class CmsBulletinService
  {
    public static List<BulletinResponse> GetBulletin(string lang)
    {
      DateTime utcNow = DateTime.UtcNow;
      string sql = "SELECT * FROM `cms_bulletin` \n                   WHERE `lang` = @lang \n                   AND `on_active` = 1 \n                   AND `trash` = 0\n                   AND `starttime` <= @currentUtcTime\n                   AND (`endtime` IS NULL OR `endtime` >= @currentUtcTime)\n                   ORDER BY starttime DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<BulletinResponse>(sql, (object) new
          {
            lang = lang,
            currentUtcTime = utcNow
          }).AsList<BulletinResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsBulletinService][GetBulletin]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BulletinContentResponse GetBulletinContent(int pk)
    {
      string sql1 = "SELECT * FROM `cms_bulletin` WHERE `pk` = @pk";
      string sql2 = "UPDATE `cms_bulletin` SET view = view + 1 WHERE `pk` = @pk";
      var data = new{ pk = pk };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          BulletinContentResponse bulletinContent = readConnection.QueryFirstOrDefault<BulletinContentResponse>(sql1, (object) data);
          if (bulletinContent != null)
            readConnection.Execute(sql2, (object) data);
          return bulletinContent;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsBulletinService][GetBulletinContent]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
