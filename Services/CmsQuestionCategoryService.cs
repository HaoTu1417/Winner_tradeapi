// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.CmsQuestionCategoryService
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
    public class CmsQuestionCategoryService
    {
        public static List<QuestionCatalogResponse> GetQuestionCatalog(string lang)
        {
            string sql = "SELECT * FROM `cms_question_category`\n                           WHERE lang = @lang AND enable = 1\n                           ORDER BY sort ASC";
            var data = new{ lang = lang };
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                    return readConnection.Query<QuestionCatalogResponse>(sql, (object) data).AsList<QuestionCatalogResponse>();
            }
            catch (Exception ex)
            {
                LogLib.Error("[CmsQuestionCategoryService][GetQuestionCatalog]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}