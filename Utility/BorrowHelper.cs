// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.BorrowHelper
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Common;
using tradeapi.Models.Dto;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Utility
{
  public class BorrowHelper
  {
    public static Decimal GetRate(string rate_string, int multiple, string borrow_type)
    {
      Decimal rate = 0M;
      switch (borrow_type)
      {
        case "day":
        case "week":
        case "month":
        case "vip":
          BorrowHelper.ConvertToDict(rate_string).TryGetValue(multiple, out rate);
          break;
      }
      return rate;
    }

    private static Dictionary<int, Decimal> ConvertToDict(string rate_string)
    {
      Dictionary<int, Decimal> dict = new Dictionary<int, Decimal>();
      foreach (string str in rate_string.Split('\n'))
      {
        string[] strArray = str.Trim().Split(':');
        try
        {
          dict.Add(int.Parse(strArray[0]), Decimal.Parse(strArray[1]));
        }
        catch (Exception ex)
        {
        }
      }
      return dict;
    }

    public static (DateTime begin_time, DateTime end_time) GetBeginAndEndTime(
      DateTime create_time,
      string market,
      int trading_time,
      int borrow_duration,
      string borrow_type)
    {
      List<StockHolidayDto> stockHolidayDtoList = StockHolidayService.Find(market);
      DateTime begin_time = create_time.Date.AddDays((double) trading_time);
      DateTime dateTime;
      if (trading_time == 0 && BorrowHelper.IsClosedHour(market, create_time))
      {
        dateTime = create_time.Date;
        begin_time = dateTime.AddDays(1.0);
      }
      if (BorrowHelper.IsClosedDay(stockHolidayDtoList, begin_time))
      {
        while (BorrowHelper.IsClosedDay(stockHolidayDtoList, begin_time))
          begin_time = begin_time.AddDays(1.0);
      }
      DateTime end_time = begin_time;
      switch (borrow_type)
      {
        case "trial":
        case "free":
        case "day":
          for (int index = 1; index < borrow_duration; ++index)
          {
            end_time = end_time.AddDays(1.0);
            if (BorrowHelper.IsClosedDay(stockHolidayDtoList, end_time))
            {
              end_time = end_time.AddDays(1.0);
              --index;
            }
          }
          break;
        case "week":
          end_time = end_time.AddDays((double) (borrow_duration * 7 - 1));
          break;
        case "month":
        case "vip":
          dateTime = end_time.AddMonths(borrow_duration);
          end_time = dateTime.AddDays(-1.0);
          break;
      }
      while (BorrowHelper.IsClosedDay(stockHolidayDtoList, end_time))
        end_time = end_time.AddDays(1.0);
      switch (market)
      {
        case "US":
          StockHolidayDto stockHolidayDto1 = stockHolidayDtoList.SingleOrDefault<StockHolidayDto>((Func<StockHolidayDto, bool>) (x => !x.is_allday && x.date == begin_time));
          if (stockHolidayDto1 == null)
          {
            begin_time = Tool.ConvertTimeToUtc("US", begin_time.Add(new TimeSpan(9, 30, 0)));
          }
          else
          {
            ref DateTime local = ref begin_time;
            dateTime = stockHolidayDto1.open;
            TimeSpan timeOfDay = dateTime.TimeOfDay;
            begin_time = local.Add(timeOfDay);
          }
          StockHolidayDto stockHolidayDto2 = stockHolidayDtoList.SingleOrDefault<StockHolidayDto>((Func<StockHolidayDto, bool>) (x => !x.is_allday && x.date == end_time));
          if (stockHolidayDto2 == null)
          {
            end_time = Tool.ConvertTimeToUtc("US", end_time.Add(new TimeSpan(16, 0, 0)));
            break;
          }
          ref DateTime local1 = ref end_time;
          dateTime = stockHolidayDto2.close;
          TimeSpan timeOfDay1 = dateTime.TimeOfDay;
          end_time = local1.Add(timeOfDay1);
          break;
        case "VN":
          begin_time = Tool.ConvertTimeToUtc("VN", begin_time.Add(new TimeSpan(9, 0, 0)));
          end_time = Tool.ConvertTimeToUtc("VN", end_time.Add(new TimeSpan(15, 0, 0)));
          break;
      }
      return (begin_time, end_time);
    }

    public static bool IsClosedDay(List<StockHolidayDto> holidays, DateTime date)
    {
      return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidays.Any<StockHolidayDto>((Func<StockHolidayDto, bool>) (x => x.is_allday && x.date == date));
    }

    public static bool IsClosedHour(string market, DateTime datetime)
    {
      DateTime local_time = Tool.ConvertTimeFromUtc(market, datetime);
      switch (market)
      {
        case "VN":
          return local_time.Date.Add(new TimeSpan(14, 30, 0)) < local_time;
        case "US":
          StockHolidayDto stockHolidayDto = StockHolidayService.Find(market).SingleOrDefault<StockHolidayDto>((Func<StockHolidayDto, bool>) (x => !x.is_allday && x.date == local_time.Date));
          return stockHolidayDto == null ? local_time.Date.Add(new TimeSpan(16, 0, 0)) < local_time : local_time.Date.Add(stockHolidayDto.close.TimeOfDay) < local_time;
        default:
          return false;
      }
    }

    public static bool IsClosedDay(string market, DateTime date)
    {
      return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || StockHolidayService.Find(market).Any<StockHolidayDto>((Func<StockHolidayDto, bool>) (x => x.is_allday && x.date == date));
    }

    public static (int multiple, int borrow_duration, Decimal deposit_money, Decimal borrow_money, Decimal rate, Decimal borrow_fee) BorrowFee(
      BorrowPlanDto borrow_plan,
      Decimal deposit_money,
      int multiple,
      int borrow_duration,
      string currency)
    {
      string borrowType = borrow_plan.borrow_type;
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      switch (borrowType)
      {
        case "trial":
          string[] strArray1 = borrow_plan.unique_set.Split('|');
          num1 = 0M;
          deposit_money = BorrowHelper.ConvertToDecimal(strArray1[0], borrow_plan.market);
          num2 = BorrowHelper.ConvertToDecimal(strArray1[1], borrow_plan.market);
          borrow_duration = Convert.ToInt32(strArray1[2]);
          multiple = (int) Math.Round(num2 / deposit_money);
          break;
        case "free":
          string[] strArray2 = borrow_plan.unique_set.Split('|');
          multiple = Convert.ToInt32(strArray2[0]);
          num1 = 0M;
          num2 = deposit_money * (Decimal) multiple;
          borrow_duration = Convert.ToInt32(strArray2[1]);
          break;
        case "day":
        case "week":
        case "month":
        case "vip":
          num1 = BorrowHelper.GetRate(borrow_plan.rate, multiple, borrowType);
          num2 = deposit_money * (Decimal) multiple;
          break;
      }
      Decimal num3 = BorrowHelper.Round(num2 * (Decimal) borrow_duration * num1 / 100M, currency);
      return (multiple, borrow_duration, deposit_money, num2, num1, num3);
    }

    public static Decimal ExpandFee(
      Decimal new_deposit_money,
      Decimal org_deposit_money,
      string borrow_type,
      string market,
      Decimal org_borrow_fee,
      DateTime begin_time,
      DateTime end_time,
      string currency)
    {
      DateTime date = DateTime.UtcNow.Date;
      if (date < begin_time.Date)
        date = begin_time.Date;
      Decimal num1;
      switch (borrow_type)
      {
        case "day":
          int tradingDays1 = BorrowHelper.GetTradingDays(date, end_time, market);
          int tradingDays2 = BorrowHelper.GetTradingDays(begin_time, end_time, market);
          num1 = org_borrow_fee * new_deposit_money / org_deposit_money * (Decimal) tradingDays1 / (Decimal) tradingDays2;
          break;
        case "week":
        case "month":
        case "vip":
          Decimal num2 = org_borrow_fee * new_deposit_money / org_deposit_money;
          TimeSpan timeSpan = end_time - date;
          Decimal num3 = (Decimal) (timeSpan.Days + 1);
          Decimal num4 = num2 * num3;
          timeSpan = end_time - begin_time;
          Decimal num5 = (Decimal) (timeSpan.Days + 1);
          num1 = num4 / num5;
          break;
        default:
          throw new AppException(1809, "expandfund_is_not_allowed");
      }
      return BorrowHelper.Round(num1, currency);
    }

    public static int GetTradingDays(DateTime begin_time, DateTime end_time, string market)
    {
      int tradingDays = 0;
      for (DateTime date = begin_time; date <= end_time; date = date.AddDays(1.0))
      {
        if (!BorrowHelper.IsClosedDay(market, date))
          ++tradingDays;
      }
      return tradingDays;
    }

    private static Decimal ConvertToDecimal(string s, string market)
    {
      s = !(market == "VN") ? s : s.Replace(".", "");
      return Convert.ToDecimal(s);
    }

    public static Decimal Round(Decimal value, string currency)
    {
      int num;
      switch (currency)
      {
        case "USD":
          num = 2;
          break;
        case "VND":
          num = 0;
          break;
        case "TWD":
          num = 0;
          break;
        default:
          num = 2;
          break;
      }
      int decimals = num;
      return Math.Round(value, decimals);
    }
  }
}
