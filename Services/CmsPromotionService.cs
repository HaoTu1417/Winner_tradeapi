// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.CmsPromotionService
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
  public class CmsPromotionService
  {
    public static List<PromotionListResponse> GetPromotionList(string lang)
    {
      string sql = "SELECT pk, lang, title, title as activity_name, view, sort, on_active, trash, show_activity_time, starttime, endtime, url, img_url \n                           FROM `cms_promotion`\n                           WHERE on_active = 1 AND `lang` = @lang\n                           ORDER BY sort ASC";
      var data = new{ lang = lang };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<PromotionListResponse>(sql, (object) data).AsList<PromotionListResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsPromotionService][GetPromotionList]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static PromotionContentResponse GetPromotionContent(int pk)
    {
      string sql = "SELECT pk, title, title as activity_name, topic_content, show_activity_time, starttime, endtime \n                           FROM `cms_promotion`\n                           WHERE `pk` = @pk";
      var data = new{ pk = pk };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<PromotionContentResponse>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsPromotionService][GetPromotionContent]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
