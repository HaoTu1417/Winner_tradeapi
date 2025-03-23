// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.StockServices
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.StockInfo;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class StockServices
  {
    public static void Upsert(StockDto model)
    {
      try
      {
        string sql = "REPLACE INTO `" + StockServices.FindTable(model.market) + "` (\n                `stock_code`, `stock_name`, `market`, `exchange`, `enable`, `disable_alwayse`, `program_enable`, `program_msg`, `main_switch`, `close_reason`, `opentrade`, `update_datetime`, `yclose`, `limitbuy`, `limitsell`, `final_price`, `volume`, `full_info`)\n                VALUES (@stock_code, @stock_name, @market, @exchange, @enable, @disable_alwayse, @program_enable, @program_msg, @main_switch, @close_reason, @opentrade, @update_datetime, @yclose, @limitbuy, @limitsell, @final_price, @volume, @full_info)";
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error(ex);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public List<SimpleStockResponse> FindSimilarStock(string code)
    {
      List<SimpleStockResponse> simpleStockResponseList = new List<SimpleStockResponse>();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "select stock_code as StockCode, stock_name as StockName, Enable as enable \n                                from trade_stock where stock_code like @stockcode or stock_name  like @stockcode";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            stockcode = (code + "%")
          });
          return readConnection.Query<SimpleStockResponse>(sql, (object) parameters).ToList<SimpleStockResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockServices][FindSimilarStock]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public List<string> FindSimilarStockCode(string code)
    {
      List<string> stringList = new List<string>();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "select stock_code as StockCode\n                                from trade_stock where stock_code like @stockcode or stock_name like @stockcode";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            stockcode = (code + "%")
          });
          return readConnection.Query<string>(sql, (object) parameters).ToList<string>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockServices][FindSimilarStock]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static StockDto FindByCode(string market, string stock_code)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "\nSELECT `stock_code`, `stock_name`, `market`, `exchange`, `enable`, `disable_alwayse`, `program_enable`, `program_msg`, `main_switch`, `close_reason`, `opentrade`, `update_datetime`, `yclose`, `limitbuy`, `limitsell`, `final_price`, `volume`, `full_info`\nFROM `stock_" + market.ToLower() + "`\nWHERE `stock_code` = @stock_code\n;";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            stock_code = stock_code
          });
          return readConnection.QuerySingleOrDefault<StockDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockServices][GetStock]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<StockDto> LikeSearch(string market, string keyword)
    {
      market = market.ToLower();
      if (keyword.Length < 2)
        return new List<StockDto>();
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        string sql = "\nSELECT `stock_code`, `stock_name`, `market`, `exchange`, `enable`, `disable_alwayse`, `program_enable`, `program_msg`, `main_switch`, `close_reason`, `opentrade`, `update_datetime`, `yclose`, `limitbuy`, `limitsell`, `final_price`, `volume`, `full_info`\nFROM `stock_" + market + "`\nWHERE `main_switch` = 1 \nAND \n(\n    `stock_code` LIKE @keyword OR\n    `stock_name` LIKE @keyword\n)\nORDER BY `stock_code`\nLIMIT 200\n;";
        return readConnection.Query<StockDto>(sql, (object) new
        {
          keyword = ("%" + keyword + "%")
        }).ToList<StockDto>();
      }
    }

    public static List<StockDto> Filter(string market, string first_stock_code)
    {
      market = market.ToLower();
      if (first_stock_code.Length < 1)
        return new List<StockDto>();
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        string sql = "\nSELECT `stock_code`, `stock_name`, `market`, `exchange`, `enable`, `disable_alwayse`, `program_enable`, `program_msg`, `main_switch`, `close_reason`, `opentrade`, `update_datetime`, `yclose`, `limitbuy`, `limitsell`, `final_price`, `volume`, `full_info`\nFROM `stock_" + market + "`\nWHERE `main_switch` = 1 \nAND `stock_code` LIKE @first_stock_code\nORDER BY `stock_code`\nLIMIT 200\n;";
        return readConnection.Query<StockDto>(sql, (object) new
        {
          first_stock_code = (first_stock_code + "%")
        }).ToList<StockDto>();
      }
    }

    public static List<StockDto> FindRankIncrease(string market, int count)
    {
      market = market.ToLower();
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(393, 2);
        interpolatedStringHandler.AppendLiteral("\nSELECT `stock_code`, `stock_name`, `market`, `exchange`, `enable`, `disable_alwayse`, `program_enable`, `program_msg`, `main_switch`, `close_reason`, `opentrade`, `update_datetime`, `yclose`, `limitbuy`, `limitsell`, `final_price`, `volume`, `full_info`\nFROM `stock_");
        interpolatedStringHandler.AppendFormatted(market);
        interpolatedStringHandler.AppendLiteral("`\nWHERE `main_switch` = 1 \nAND `yclose` > 0\nAND `final_price` > 0\nORDER BY (`final_price` - `yclose`)/ `yclose` DESC \nLIMIT ");
        interpolatedStringHandler.AppendFormatted<int>(count);
        interpolatedStringHandler.AppendLiteral("\n;");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        return readConnection.Query<StockDto>(stringAndClear).ToList<StockDto>();
      }
    }

    public static List<StockDto> FindRankDecline(string market, int count)
    {
      market = market.ToLower();
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(388, 2);
        interpolatedStringHandler.AppendLiteral("\nSELECT `stock_code`, `stock_name`, `market`, `exchange`, `enable`, `disable_alwayse`, `program_enable`, `program_msg`, `main_switch`, `close_reason`, `opentrade`, `update_datetime`, `yclose`, `limitbuy`, `limitsell`, `final_price`, `volume`, `full_info`\nFROM `stock_");
        interpolatedStringHandler.AppendFormatted(market);
        interpolatedStringHandler.AppendLiteral("`\nWHERE `main_switch` = 1 \nAND `yclose` > 0\nAND `final_price` > 0\nORDER BY (`final_price` - `yclose`)/ `yclose` \nLIMIT ");
        interpolatedStringHandler.AppendFormatted<int>(count);
        interpolatedStringHandler.AppendLiteral("\n;");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        return readConnection.Query<StockDto>(stringAndClear).ToList<StockDto>();
      }
    }

    public static List<StockDto> FindRankVolume(string market, int count)
    {
      market = market.ToLower();
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(364, 2);
        interpolatedStringHandler.AppendLiteral("\nSELECT `stock_code`, `stock_name`, `market`, `exchange`, `enable`, `disable_alwayse`, `program_enable`, `program_msg`, `main_switch`, `close_reason`, `opentrade`, `update_datetime`, `yclose`, `limitbuy`, `limitsell`, `final_price`, `volume`, `full_info`\nFROM `stock_");
        interpolatedStringHandler.AppendFormatted(market);
        interpolatedStringHandler.AppendLiteral("`\nWHERE `main_switch` = 1 \nAND `yclose` > 0\nAND `final_price` > 0\nORDER BY `volume` DESC\nLIMIT ");
        interpolatedStringHandler.AppendFormatted<int>(count);
        interpolatedStringHandler.AppendLiteral("\n;");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        return readConnection.Query<StockDto>(stringAndClear).ToList<StockDto>();
      }
    }

    public static StockDto? Find(string market, string stock_code)
    {
      string str = (string) null;
      switch (market.ToLower())
      {
        case "us":
          str = "stock_us";
          break;
        case "hk":
          str = "stock_hk";
          break;
        case "cn":
          str = "stock_cn";
          break;
        case "tw":
          str = "stock_tw";
          break;
        case "vn":
          str = "stock_vn";
          break;
      }
      if (str == null)
        return (StockDto) null;
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM `" + str + "` WHERE `stock_code` = @stock_code";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            stock_code = stock_code
          });
          return readConnection.QuerySingleOrDefault<StockDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockServices][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<string> FindAllEnabledStockCode(string market, string? exchange)
    {
      string sql = "SELECT stock_code FROM `" + StockServices.FindTable(market) + "` WHERE main_switch = 1";
      var data = new{ exchange = exchange };
      if (!string.IsNullOrEmpty(exchange))
        sql += " AND exchange = @exchange";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<string>(sql, (object) data).AsList<string>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockServices][FindAllEnabledStockCode]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    private static string FindTable(string market)
    {
      switch (market.ToLower())
      {
        case "us":
          return "stock_us";
        case "hk":
          return "stock_hk";
        case "cn":
          return "stock_cn";
        case "tw":
          return "stock_tw";
        case "vn":
          return "stock_vn";
        default:
          throw new Exception("Wrong Market: " + market);
      }
    }
  }
}
