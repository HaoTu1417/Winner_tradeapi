// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.CmsQuestionService
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
  public class CmsQuestionService
  {
    public static List<QuestionListResponse> GetQuestionList(string lang, int catalog_pk)
    {
      string sql = "SELECT T1.`pk`, T1.`question`\n                           FROM `cms_question` T1\n                           INNER JOIN `cms_question_category` T2 ON (T1.question_category_fk = T2.pk)\n                           WHERE T1.enable = 1 AND T2.`lang` = @lang AND T2.pk = @catalog_pk\n                           ORDER BY T1.sort ASC";
      var data = new{ lang = lang, catalog_pk = catalog_pk };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<QuestionListResponse>(sql, (object) data).AsList<QuestionListResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsQuestionService][GetQuestionList]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static AnswerResponse GetQuestionAnswer(string lang, int question_pk)
    {
      string sql = "SELECT T1.`question`, T1.`answer`\n                           FROM `cms_question` T1\n                           INNER JOIN `cms_question_category` T2 ON (T1.question_category_fk = T2.pk)\n                           WHERE T1.`pk` = @question_pk AND T1.enable = 1 AND T2.`lang` = @lang";
      var data = new
      {
        question_pk = question_pk,
        lang = lang
      };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<AnswerResponse>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsQuestionService][GetQuestionAnswer]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<ServiceQuestioResponse> GetServiceQuestionList(string lang, int catalog_pk)
    {
      string sql = "SELECT T1.`pk`, T1.`question`, T1.`answer`\n                           FROM `cms_question` T1\n                           INNER JOIN `cms_question_category` T2 ON (T1.question_category_fk = T2.pk)\n                           WHERE T1.enable = 1 AND T1.commonly_used = 1 AND T2.`lang` = @lang AND T2.pk = @catalog_pk\n                           ORDER BY T1.sort ASC";
      var data = new{ lang = lang, catalog_pk = catalog_pk };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<ServiceQuestioResponse>(sql, (object) data).AsList<ServiceQuestioResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsQuestionService][GetServiceQuestionList]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static ServiceAnswerResponse GetServiceAnswer(string lang, int question_pk)
    {
      string sql = "SELECT T1.`answer`\n                           FROM `cms_question` T1\n                           INNER JOIN `cms_question_category` T2 ON (T1.question_category_fk = T2.pk)\n                           WHERE T1.`pk` = @question_pk AND T1.enable = 1 AND T1.commonly_used = 1 AND T2.`lang` = @lang";
      var data = new
      {
        question_pk = question_pk,
        lang = lang
      };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<ServiceAnswerResponse>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[CmsQuestionService][GetServiceAnswer]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
