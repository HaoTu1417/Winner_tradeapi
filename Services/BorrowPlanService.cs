// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.BorrowPlanService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Finance;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class BorrowPlanService
  {
    public static BorrowPlanDto GetBorrowPlan(int pk)
    {
      string sql = "SELECT * FROM `borrow_plan` WHERE pk = @pk";
      var data = new{ pk = pk };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<BorrowPlanDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowPlanService][GetBorrowPlan(int pk)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<GetItemsResponse> GetBorrowPlans(string market, string lang)
    {
      string sql = "SELECT * FROM `borrow_plan` WHERE enable = 1 AND market = @market AND lang = @lang ORDER BY sort ASC";
      var data = new{ market = market, lang = lang };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<GetItemsResponse>(sql, (object) data).AsList<GetItemsResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowPlanService][GetBorrowPlans]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowPlanResponse GetBorrowPlan(
      string market,
      string lang,
      string borrow_type,
      int member_fk)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(275, 2);
      interpolatedStringHandler.AppendLiteral("SELECT *, borrow_plan.pk as pk, m.");
      interpolatedStringHandler.AppendFormatted(borrow_type);
      interpolatedStringHandler.AppendLiteral("_rate as rate FROM `borrow_plan`\n                                        INNER JOIN member m ON m.pk = ");
      interpolatedStringHandler.AppendFormatted<int>(member_fk);
      interpolatedStringHandler.AppendLiteral("\n                                        WHERE enable = 1 AND market = @market AND borrow_plan.lang = @lang AND borrow_type = @borrow_type");
      string sql = string.Format(interpolatedStringHandler.ToStringAndClear());
      var data = new
      {
        market = market,
        borrow_type = borrow_type,
        lang = lang
      };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<BorrowPlanResponse>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowPlanService][GetBorrowPlan(string market, string borrow_type)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowPlanDto GetBorrowPlan(string sub_account)
    {
      string sql = "SELECT * FROM borrow_plan WHERE pk = (SELECT borrow_plan_fk FROM trade_account WHERE sub_account = @sub_account)";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<BorrowPlanDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowPlanService][GetBorrowPlan(string sub_account)]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static BorrowPlanDto FindByBorrowType(string borrow_type, string market)
    {
      string sql = "SELECT * FROM `borrow_plan` WHERE `borrow_type` = @borrow_type and `market` = @market";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            borrow_type = borrow_type,
            market = market
          });
          return readConnection.QueryFirstOrDefault<BorrowPlanDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[BorrowPlanService][FindByBorrowType]" + ex.Message);
        return (BorrowPlanDto) null;
      }
    }
  }
}
