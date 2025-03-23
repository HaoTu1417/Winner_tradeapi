// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.RecommendRewardSummaryService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Recommend;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class RecommendRewardSummaryService
  {
    public static RecommendRewardSummaryDto Find(int pk)
    {
      string sql = "SELECT * FROM `recommend_reward_summary` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<RecommendRewardSummaryDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<RecommendRewardSummaryDto> FindAll()
    {
      string sql = "SELECT * FROM `recommend_reward_summary`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RecommendRewardSummaryDto>(sql).AsList<RecommendRewardSummaryDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(RecommendRewardSummaryDto source)
    {
      string sql = "INSERT INTO `recommend_reward_summary` (\n                `Recommend_fk`, `recommend_reward_fk`, `layer`, `yymm`, `total_Recommends`, `total_borrow_fee`, `total_reward`)\n                VALUES (@Recommend_fk, @recommend_reward_fk, @layer, @yymm, @total_Recommends, @total_borrow_fee, @total_reward);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(RecommendRewardSummaryDto model)
    {
      string sql = "UPDATE `recommend_reward_summary` SET \n                `Recommend_fk` = @Recommend_fk,\n                `recommend_reward_fk` = @recommend_reward_fk,\n                `layer` = @layer,\n                `yymm` = @yymm,\n                `total_Recommends` = @total_Recommends,\n                `total_borrow_fee` = @total_borrow_fee,\n                `total_reward` = @total_reward\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `recommend_reward_summary` WHERE `pk` = @pk";
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
        LogLib.Error("[RecommendRewardSummaryService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<RecommendRewardSummaryVw> FindByMemberFkAndYymm(int member_fk, string yymm)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(141, 2);
      interpolatedStringHandler.AppendLiteral("SELECT t1.* \n                FROM `recommend_reward_summary` t1 \n                WHERE t1.`member_fk` = '");
      interpolatedStringHandler.AppendFormatted<int>(member_fk);
      interpolatedStringHandler.AppendLiteral("' AND t1.yymm = '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("' ORDER BY t1.layer");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RecommendRewardSummaryVw>(stringAndClear).AsList<RecommendRewardSummaryVw>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][FindByMemberFkAndYymm]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    internal static int InsertAgent(string yymm)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(650, 2);
      interpolatedStringHandler.AppendLiteral("INSERT INTO `recommend_reward_summary` (`member_fk`, `recommend_reward_fk`, `layer`, `yymm`, `monthly_members`, `monthly_borrow_fee`, `monthly_reward`)\n                (SELECT parent AS member_fk, recommend_reward.pk AS recommend_reward_fk, generation AS layer, '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("' AS yymm, \n                COUNT(*) AS monthly_members, SUM(management_fee) AS `monthly_borrow_fee`, SUM(reward) AS monthly_reward \n                FROM recommend_reward_detail \n                INNER JOIN recommend_reward ON recommend_reward.member_fk = recommend_reward_detail.parent\n                Where recommend_reward_detail.yymm = '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("'\n                GROUP BY parent, generation) ");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][InertAgent]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static RecommendRewardSummaryDto FindByYymmAndDeep(string yymm, int member, int deep)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(86, 3);
      interpolatedStringHandler.AppendLiteral("SELECT * FROM `recommend_reward_summary` WHERE member_fk = ");
      interpolatedStringHandler.AppendFormatted<int>(member);
      interpolatedStringHandler.AppendLiteral(" AND yymm = '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("' AND layer = ");
      interpolatedStringHandler.AppendFormatted<int>(deep);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<RecommendRewardSummaryDto>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][FindByYymmAndDeep]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void UpdateByYymmAndDeep(MonthProfitModel data, int id)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(194, 4);
      interpolatedStringHandler.AppendLiteral("UPDATE `recommend_reward_summary` SET \n                   `monthly_members` = ");
      interpolatedStringHandler.AppendFormatted<int>(data.monthly_members);
      interpolatedStringHandler.AppendLiteral(",\n                   `monthly_borrow_fee` = ");
      interpolatedStringHandler.AppendFormatted<Decimal>(data.monthly_borrow_fee);
      interpolatedStringHandler.AppendLiteral(",\n                   `monthly_reward` = ");
      interpolatedStringHandler.AppendFormatted<Decimal>(data.monthly_reward);
      interpolatedStringHandler.AppendLiteral("\n                 WHERE `pk` = ");
      interpolatedStringHandler.AppendFormatted<int>(id);
      interpolatedStringHandler.AppendLiteral(" ");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardSummaryService][UpdateByYymmAndDeep]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
