// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.RecommendRewardService
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
  public class RecommendRewardService
  {
    public static RecommendRewardDto Find(int pk)
    {
      string sql = "SELECT * FROM `recommend_reward` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<RecommendRewardDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static RecommendRewardDto FindByMemberAndYYMM(int member, string yymm)
    {
      string sql = "SELECT * FROM `recommend_reward` WHERE `member_fk` = @mem AND yymm = @yymm";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            mem = member,
            yymm = yymm
          });
          return readConnection.QueryFirstOrDefault<RecommendRewardDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][FindByMemberAndYYMM]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<RecommendRewardDto> FindAll()
    {
      string sql = "SELECT * FROM `recommend_reward`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RecommendRewardDto>(sql).AsList<RecommendRewardDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(RecommendRewardDto source)
    {
      string sql = "INSERT INTO `recommend_reward` (`member_fk`, `yymm`, `year`, `month`, `currency`, `total_reward`, `state`, \n            `create_time`) VALUES (@member_fk, @yymm, @year, @month, @currency, @total_reward, @state, @create_time);\n\n            select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(RecommendRewardDto model)
    {
      string sql = "UPDATE `recommend_reward` SET \n                `Recommend_fk` = @Recommend_fk,\n                `month` = @month,\n                `currency` = @currency,\n                `total_reward` = @total_reward,\n                `state` = @state,\n                `withdraw` = @withdraw,\n                `paydate` = @paydate,\n                `note` = @note\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `recommend_reward` WHERE `pk` = @pk";
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
        LogLib.Error("[RecommendRewardService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<RecommendRewardInfo> FindByMemberFk(int member_fk)
    {
      string sql = "SELECT T1.year, T1.month, T1.total_reward, GROUP_CONCAT(T2.monthly_members ORDER BY T2.layer ASC) AS members, GROUP_CONCAT(T2.monthly_borrow_fee ORDER BY T2.layer ASC) AS borrow_fees, GROUP_CONCAT(T2.monthly_reward ORDER BY T2.layer ASC) AS rewards, T1.state, T1.withdraw, T1.paydate\n                           FROM `recommend_reward` T1 LEFT JOIN `recommend_reward_summary` T2 ON (T1.pk = T2.recommend_reward_fk)\n                           WHERE T1.member_fk = @member_fk\n                           GROUP BY T2.recommend_reward_fk\n                           ORDER BY T1.create_time DESC";
      var data = new{ member_fk = member_fk };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RecommendRewardInfo>(sql, (object) data).AsList<RecommendRewardInfo>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][FindByMemberFk]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static Decimal GetTotalRewardByMemberFk(int member_fk)
    {
      string sql = "\nSELECT IFNULL(SUM(total_reward), 0) \nFROM `recommend_reward` \nWHERE `member_fk` = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QuerySingle<Decimal>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][GetTotalRewardByMemberFk]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static RecommendRewardDto FindByYearMonth(int member_fk, string yymm)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 2);
      interpolatedStringHandler.AppendLiteral("SELECT * FROM `recommend_reward` WHERE `member_fk` = ");
      interpolatedStringHandler.AppendFormatted<int>(member_fk);
      interpolatedStringHandler.AppendLiteral(" AND `yymm` = '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("'");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<RecommendRewardDto>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][FindByYearMonth]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateStatus(int member_fk, string yymm)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(197, 2);
      interpolatedStringHandler.AppendLiteral("UPDATE `recommend_reward` SET \n                `state` = 2,\n                `paydate` = UTC_TIMESTAMP,\n                `withdraw` = UTC_TIMESTAMP\n                 WHERE `member_fk` = ");
      interpolatedStringHandler.AppendFormatted<int>(member_fk);
      interpolatedStringHandler.AppendLiteral(" AND yymm = '");
      interpolatedStringHandler.AppendFormatted(yymm);
      interpolatedStringHandler.AppendLiteral("'");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Execute(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][UpdateStatus]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<AllRecommendRewradInfoResponse> FindByMember(int member)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 1);
      interpolatedStringHandler.AppendLiteral("SELECT * FROM `recommend_reward` where member_fk = ");
      interpolatedStringHandler.AppendFormatted<int>(member);
      interpolatedStringHandler.AppendLiteral(" order by yymm DESC");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<AllRecommendRewradInfoResponse>(stringAndClear).AsList<AllRecommendRewradInfoResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][FindByMember]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    internal static void UpdateReward(int member, string yymm)
    {
      string sql = "UPDATE `recommend_reward` SET total_reward = \n            (SELECT SUM(recommend_reward_detail.reward) FROM recommend_reward_detail\n              WHERE recommend_reward_detail.parent = @member\n              AND recommend_reward_detail.yymm = @yymm)\n            WHERE yymm = @yymm AND member_fk = @member AND state <= 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            yymm = yymm,
            member = member
          });
          readConnection.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRewardService][UpdateReward]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
