// Decompiled with JetBrains decompiler
// Type: DB.Services.AppLogoService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
    public class AppLogoService
    {
        public static List<AppLogoDto> FindAppLogoList(string whereSql = "")
        {
            string sql = "SELECT *\n                FROM (\n                    SELECT app_logo.cms_files_fk, app_logo.type, IF(app_logo.enable = 1, 'o', 'x') AS enable, app_logo.lang, cms_files.url AS url,\n                    ROW_NUMBER() OVER (PARTITION BY app_logo.type ORDER BY app_logo.cms_files_fk DESC) AS n\n                    FROM `app_logo`\n                    LEFT JOIN cms_files on cms_files.pk = app_logo.cms_files_fk " + whereSql + "\n                ) AS x\n                WHERE n <= 1";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                    return readConnection.Query<AppLogoDto>(sql).AsList<AppLogoDto>();
            }
            catch (Exception ex)
            {
                LogLib.Error("[AppLogoService][FindAppLogoList]" + ex.Message);
                return (List<AppLogoDto>) null;
            }
        }
    }
}