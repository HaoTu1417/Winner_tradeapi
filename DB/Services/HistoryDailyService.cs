// Decompiled with JetBrains decompiler
// Type: DB.Services.HistoryDailyService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.IndChart;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class HistoryDailyService
  {
    public static List<StockHistoryList> FindHistory(string table, string stock_code, int count = 90)
    {
      string sql = "SELECT stock_code, DATE_FORMAT(date, '%Y-%m-%d') as date, open, high, low, close, volume\nFROM `" + table + "` WHERE `stock_code` = @stock_code ORDER BY date DESC LIMIT @count";
      try
      {
        using (IDbConnection readConnection = StockDb.GetReadConnection())
        {
          var data = new
          {
            stock_code = stock_code,
            count = count
          };
          return readConnection.Query<StockHistoryList>(sql, (object) data).AsList<StockHistoryList>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[HistoryDailyService][FindHistory]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static DateTime FindMaxDate(string table)
    {
      string sql = "SELECT MAX(date) FROM " + table;
      try
      {
        using (IDbConnection readConnection = StockDb.GetReadConnection())
          return readConnection.ExecuteScalar<DateTime>(sql);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        LogLib.Error("[StockOptionPositionService][FindMaxDate]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<StockHistoryList> FindHistoryByDate(string table, string date)
    {
      string sql = "SELECT stock_code, DATE_FORMAT(date, '%Y-%m-%d') as date, open, high, low, close, volume\nFROM " + table + " WHERE `date` = @date ORDER BY stock_code ASC";
      try
      {
        using (IDbConnection readConnection = StockDb.GetReadConnection())
        {
          var data = new{ date = date, table = table };
          return readConnection.Query<StockHistoryList>(sql, (object) data).AsList<StockHistoryList>();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        LogLib.Error("[HistoryDailyService][FindHistoryByDate]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static string FindTable(string market)
    {
      switch (market.ToLower())
      {
        case "us":
          return "history_daily_us_pt";
        case "vn":
          return "history_daily_vn_pt";
        case "hk":
          return "history_daily_hk_pt";
        case "cn":
          return "history_daily_cn_pt";
        case "tw":
          return "history_daily_tw_pt";
        default:
          throw new Exception("Wrong Market: " + market);
      }
    }
  }
}
