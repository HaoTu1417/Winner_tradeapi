// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.RecommendRewardDetailService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Recommend;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class RecommendRewardDetailService
  {
    public static RecommendRewardDetailDto Find(int pk)
    {
      string sql = "SELECT * FROM `recommend_reward_detail` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<RecommendRewardDetailDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardDetailService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<RecommendRewardDetailDto> FindAll()
    {
      string sql = "SELECT * FROM `recommend_reward_detail`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RecommendRewardDetailDto>(sql).AsList<RecommendRewardDetailDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardDetailService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(RecommendRewardDetailDto source)
    {
      string sql = "INSERT INTO `recommend_reward_detail` (\n                `recommend_reward_fk`, `member_fk`, `parent`, `borrow_fee_fk`, `yymm`, `borrow_date`, `currency`, `management_fee`, `generation`, `rate`, `reward`)\n                VALUES (@recommend_reward_fk, @member_fk, @parent, @borrow_fee_fk, @yymm, @borrow_date, @currency, @management_fee, @generation, @rate, @reward); \n\n            select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardDetailService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(RecommendRewardDetailDto model)
    {
      string sql = "UPDATE `recommend_reward_detail` SET \n                `recommend_reward_fk` = @recommend_reward_fk,\n                `member_fk` = @member_fk,\n                `parent` = @parent,\n                `borrow_fee_fk` = @borrow_fee_fk,\n                `yymm` = @yymm,\n                `borrow_date` = @borrow_date,\n                `currency` = @currency,\n                `management_fee` = @management_fee,\n                `generation` = @generation,\n                `rate` = @rate,\n                `reward` = @reward\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardDetailService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `recommend_reward_detail` WHERE `pk` = @pk";
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
        LogLib.Error("[RecommendRewardDetailService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<RecommendRewardDetailResponse> FindByYearMonth(int member_fk, string yymm)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(426, 2);
      interpolatedStringHandler.AppendLiteral("SELECT member.real_name,\n                d.`generation`, \n                d.`borrow_date`,\n                d.`management_fee`,\n                d.`rate`,\n                d.`reward`,\n                d.`currency`\n                FROM `recommend_reward_detail` d\n                INNER JOIN `member` ON member.pk = d.member_fk\n                WHERE d.parent = ");
      interpolatedStringHandler.AppendFormatted<int>(member_fk);
      interpolatedStringHandler.AppendLiteral(" AND d.yymm = '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("'\n                ORDER BY d.generation, d.`borrow_date`");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RecommendRewardDetailResponse>(stringAndClear).ToList<RecommendRewardDetailResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardDetailService][FindByYearMonth]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<int> GetUniqueMemberFks()
    {
      string sql = "SELECT DISTINCT(parent) FROM recommend_reward_detail";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<int>(sql).AsList<int>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardDetailService][GetUniqueMemberFks]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static MonthProfitModel FindProfit(string yymm, int member, int deep)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(531, 3);
      interpolatedStringHandler.AppendLiteral("SELECT generation, COUNT(DISTINCT(recommend_reward_detail.member_fk)) AS monthly_members, SUM(management_fee) AS `monthly_borrow_fee`, SUM(reward) AS monthly_reward \n                FROM recommend_reward_detail \n                INNER JOIN recommend_reward ON recommend_reward_detail.parent = recommend_reward.member_fk AND recommend_reward_detail.yymm = recommend_reward.yymm\n                WHERE recommend_reward_detail.yymm = '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("' AND recommend_reward_detail.parent = ");
      interpolatedStringHandler.AppendFormatted<int>(member);
      interpolatedStringHandler.AppendLiteral(" AND generation = ");
      interpolatedStringHandler.AppendFormatted<int>(deep);
      interpolatedStringHandler.AppendLiteral("\n                GROUP BY parent, generation");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<MonthProfitModel>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardDetailService][FindProfit]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
