// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.IndChartBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Models.IndChart;

#nullable enable
namespace tradeapi.Business
{
  public class IndChartBiz
  {
    private static string GetHistoryTableName(string period, string market)
    {
      market = market.ToLower();
      if (market != "vn" && market != "us")
        throw new AppException(1570, "market_not_exist");
      string historyTableName;
      switch (period)
      {
        case "daily":
          historyTableName = "history_daily_" + market + "_pt";
          break;
        case "weekly":
          historyTableName = "history_weekly_" + market;
          break;
        case "monthly":
          historyTableName = "history_monthly_" + market;
          break;
        default:
          historyTableName = "history_daily_" + market + "_pt";
          break;
      }
      return historyTableName;
    }

    public static List<StockHistoryList> GetHistory(
      string period,
      string market,
      string stock_code)
    {
      return HistoryDailyService.FindHistory(IndChartBiz.GetHistoryTableName(period, market), stock_code).OrderBy<StockHistoryList, string>((Func<StockHistoryList, string>) (x => x.date)).ToList<StockHistoryList>();
    }

    public static List<TodayResponse> GetToday(string market, string stock_code)
    {
      CacheQueryAsync.SelectDB(market);
      return (CacheQueryAsync.StringGet<TodayTick>(stock_code) ?? new TodayTick()).tick.Select<string[], TodayResponse>((Func<string[], TodayResponse>) (x => new TodayResponse()
      {
        tick = x
      })).ToList<TodayResponse>();
    }

    public static void SetIndicator(
      int member_fk,
      string name,
      double param1,
      double param2,
      double param3,
      double param4,
      double param5)
    {
      IndicatorParamDto model = IndicatorParamService.Find(member_fk, name);
      if (model == null)
      {
        int num = (int) IndicatorParamService.Insert(new IndicatorParamDto()
        {
          member_fk = member_fk,
          name = name.ToUpper(),
          param1 = param1,
          param2 = param2,
          param3 = param3,
          param4 = param4,
          param5 = param5
        });
      }
      else
      {
        model.param1 = param1;
        model.param2 = param2;
        model.param3 = param3;
        model.param4 = param4;
        model.param5 = param5;
        IndicatorParamService.Update(model);
      }
    }

    public static GetIndicatorResponse GetIndicator(int member_fk, string name)
    {
      IndicatorParamDto indicatorParamDto = IndicatorParamService.Find(member_fk, name);
      if (indicatorParamDto == null)
      {
        if (name.ToUpper() == "SMA1")
          return new GetIndicatorResponse()
          {
            name = "SMA1",
            param1 = 20.0
          };
        if (name.ToUpper() == "SMA2")
          return new GetIndicatorResponse()
          {
            name = "SMA2",
            param1 = 50.0
          };
        if (name.ToUpper() == "SMA3")
          return new GetIndicatorResponse()
          {
            name = "SMA3",
            param1 = 200.0
          };
        if (name.ToUpper() == "KD")
          return new GetIndicatorResponse()
          {
            name = "KD",
            param1 = 5.0,
            param2 = 3.0
          };
        if (name.ToUpper() == "MACD")
          return new GetIndicatorResponse()
          {
            name = "MACD",
            param1 = 12.0,
            param2 = 26.0,
            param3 = 9.0
          };
        if (!(name.ToUpper() == "RSI"))
          return new GetIndicatorResponse();
        return new GetIndicatorResponse()
        {
          name = "RSI",
          param1 = 5.0
        };
      }
      return new GetIndicatorResponse()
      {
        name = indicatorParamDto.name,
        param1 = indicatorParamDto.param1,
        param2 = indicatorParamDto.param2,
        param3 = indicatorParamDto.param3,
        param4 = indicatorParamDto.param4,
        param5 = indicatorParamDto.param5
      };
    }
  }
}
