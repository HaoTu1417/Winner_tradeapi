// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.MoneyDailyExchangeService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class MoneyDailyExchangeService
  {
    public static List<MoneyDailyExchangeDto> Find(string currency, DateTime date)
    {
      try
      {
        string sql = "SELECT * FROM `vw_exchange_rate` WHERE `base_symbol` = @currency AND currency_symbol <> @currency AND `date` = @date";
        var data = new{ currency = currency, date = date };
        using (IDbConnection readConnection = StockDb.GetReadConnection())
          return readConnection.Query<MoneyDailyExchangeDto>(sql, (object) data).AsList<MoneyDailyExchangeDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[MoneyDailyExchangeService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static DateTime GetLastDate()
    {
      try
      {
        string sql = "SELECT MAX(`date`) FROM `vw_exchange_rate`";
        using (IDbConnection readConnection = StockDb.GetReadConnection())
          return readConnection.QuerySingle<DateTime>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MoneyDailyExchangeService][GetLastDate]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<MoneyDailyExchangeDto> GetExchange(string currency)
    {
      try
      {
        using (IDbConnection readConnection = StockDb.GetReadConnection())
        {
          string sql1 = "SELECT `create_time` FROM `money_daily_exchange`\n                                   ORDER BY `create_time` DESC LIMIT 1";
          DateTime dateTime = readConnection.QuerySingleOrDefault<DateTime>(sql1);
          string sql2 = "SELECT * FROM `money_daily_exchange`\n                            WHERE YEAR(@create_time) AND MONTH(@create_time) AND HOUR(@create_time) AND MINUTE(@create_time) AND base_symbol = @currency";
          var data = new
          {
            create_time = dateTime,
            currency = currency
          };
          return readConnection.Query<MoneyDailyExchangeDto>(sql2, (object) data).AsList<MoneyDailyExchangeDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MoneyDailyExchangeService][GetExchange]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
