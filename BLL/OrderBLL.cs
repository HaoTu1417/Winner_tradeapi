// Decompiled with JetBrains decompiler
// Type: BLLs.OrderBLL
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace BLLs
{
  public class OrderBLL
  {
    private string _market;
    private int _price_type;
    private string _buyOrSell;
    private int _shares;
    private Decimal _price;
    private StockQuote _quote;
    private List<TradePositionDto> _trade_position_list;
    private TradeAccountDto _trade_account;
    private static TimeZoneInfo vn_tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
    private static TimeZoneInfo us_tz = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
    private static Dictionary<string, OrderBLL.Period> _open_periods = new Dictionary<string, OrderBLL.Period>()
    {
      {
        "HOSE",
        new OrderBLL.Period()
        {
          start = new TimeSpan(9, 0, 0),
          end = new TimeSpan(9, 15, 0)
        }
      }
    };
    private static Dictionary<string, OrderBLL.Period> _close_periods = new Dictionary<string, OrderBLL.Period>()
    {
      {
        "HOSE",
        new OrderBLL.Period()
        {
          start = new TimeSpan(14, 30, 0),
          end = new TimeSpan(14, 45, 0)
        }
      },
      {
        "HNX",
        new OrderBLL.Period()
        {
          start = new TimeSpan(14, 30, 0),
          end = new TimeSpan(14, 45, 0)
        }
      }
    };
    private static readonly OrderBLL.Period _hose_allowed_order_time = new OrderBLL.Period()
    {
      start = new TimeSpan(6, 0, 0),
      end = new TimeSpan(14, 45, 0)
    };
    private static readonly OrderBLL.Period _hnx_allowed_order_time = new OrderBLL.Period()
    {
      start = new TimeSpan(6, 0, 0),
      end = new TimeSpan(14, 45, 0)
    };
    private static readonly OrderBLL.Period _upcom_allowed_order_time = new OrderBLL.Period()
    {
      start = new TimeSpan(6, 0, 0),
      end = new TimeSpan(15, 0, 0)
    };

    public OrderBLL(
      string market,
      int price_type,
      string buyOrSell,
      int shares,
      Decimal price,
      StockQuote quote,
      List<TradePositionDto> trade_position_list,
      TradeAccountDto trade_account)
    {
      this._market = market;
      this._price_type = price_type;
      this._price = price;
      this._buyOrSell = buyOrSell;
      this._shares = shares;
      this._quote = quote;
      this._trade_position_list = trade_position_list;
      this._trade_account = trade_account;
    }

    private void CheckUS(DateTime localNow)
    {
      if (this._price_type > 2)
        return;
      Decimal amount = (this._price_type == 1 ? this._price : (this._buyOrSell == "B" ? this._price * 1.2M : this._price * 0.8M)) * (Decimal) this._shares;
      if (this._trade_account.mem_money - this._trade_account.frozen_money < amount + OrderHelper.GetHandlingFee(this._market, this._buyOrSell == "B" ? 1 : 2, amount))
        throw new AppException(1460, "insufficient_funds");
    }

    private void CheckVN(DateTime localNow)
    {
      if (!OrderBLL.IsAllowedOrder(this._quote.exchange, localNow))
        throw new AppException(1410, "this_time_cannot_order");
      if (this._quote.exchange == "HOSE")
      {
        if (this._shares % 10 != 0)
          throw new AppException(1511, "illegal_commission_purchase_multiple_10");
        if ((OrderBLL.IsDeniedToCancelDuringOpenPeriod(this._quote.exchange, localNow) || OrderBLL.IsDeniedToCancelDuringClosePeriod(this._quote.exchange, localNow)) && this._price_type != 1)
          throw new AppException(1760, "hose_limit_order_match_session");
      }
      else if (this._quote.exchange == "HNX")
      {
        if (this._shares % 100 != 0)
          throw new AppException(1510, "illegal_commission_purchase_multiple_100");
        if (OrderBLL.IsDeniedToCancelDuringClosePeriod(this._quote.exchange, localNow) && this._price_type != 1)
          throw new AppException(1770, "hnx_limit_order_match_session");
      }
      else if (this._quote.exchange == "UPCOM")
      {
        if (this._shares % 100 != 0)
          throw new AppException(1510, "illegal_commission_purchase_multiple_100");
        if (this._trade_account.loan_type == "trial")
          throw new AppException(1421, "trial_cannot_order_upcom_stock");
      }
      int availableOffsetVolume = OrderBLL.CalculateAvailableOffsetVolume(this._trade_position_list, 1);
      if (this._buyOrSell == "S" && availableOffsetVolume - this._shares < 0)
        throw new AppException(1470, "insufficient_position");
      if (!(this._buyOrSell == "B"))
        return;
      Decimal amount = (this._price_type == 1 ? this._price : this._quote.ceiling) * (Decimal) this._shares;
      if (this._trade_account.mem_money - this._trade_account.frozen_money < amount + OrderHelper.GetHandlingFee(this._market, 1, amount))
        throw new AppException(1460, "insufficient_funds");
    }

    public void CheckOrder()
    {
      DateTime localTime = OrderBLL.GetLocalTime(this._market);
      int availableOffsetVolume1 = OrderBLL.CalculateAvailableOffsetVolume(this._trade_position_list, 1);
      int availableOffsetVolume2 = OrderBLL.CalculateAvailableOffsetVolume(this._trade_position_list, -1);
      if (this._price_type < 1 || this._price_type > 3)
        throw new AppException(1402, "price_type_error");
      if (this._price_type == 1 && this._price <= 0M)
        throw new AppException(1403, "wrong_price");
      if (this._price_type == 3 && this._price <= 0M)
        throw new AppException(1403, "wrong_price");
      if (this._shares <= 0)
        throw new AppException(1710, "wrong_quantity");
      if (this._price_type == 2)
      {
        if (this._buyOrSell == "B" && (this._quote.asks == null || this._quote.asks.Length == 0))
          throw new AppException(1418, "not_allowed_market_order_to_buy");
        if (this._buyOrSell == "S" && (this._quote.bids == null || this._quote.bids.Length == 0))
          throw new AppException(1419, "not_allowed_market_order_to_sell");
      }
      if (localTime.DayOfWeek == DayOfWeek.Saturday || localTime.DayOfWeek == DayOfWeek.Sunday)
        throw new AppException(1410, "this_time_cannot_order");
      if (OrderBLL.IsHoliday(this._market, localTime))
        throw new AppException(1410, "this_time_cannot_order");
      if (this._trade_account.status > 1)
        throw new AppException(1430, "cannot_trade_account_termination");
      if (this._price_type == 3)
      {
        int stockType = this._buyOrSell == "B" ? -1 : 1;
        if (OrderBLL.CalculateAvailableOffsetVolume(this._trade_position_list, stockType) - OrderBLL.CalculateStopLosePos(this._trade_position_list, stockType) < this._shares)
          throw new AppException(1470, "insufficient_position");
      }
      if (this._buyOrSell == "S" && availableOffsetVolume1 < this._shares || this._buyOrSell == "B" && availableOffsetVolume2 < this._shares)
      {
        DateTime dateTime = this._trade_account.end_time;
        DateTime date1 = dateTime.Date;
        dateTime = DateTime.UtcNow;
        DateTime date2 = dateTime.Date;
        if (date1 <= date2 && (!BorrowService.GetBorrow(this._trade_account.sub_account).auto_renewal || this._trade_account.close_type > 0))
          throw new AppException(1411, "account_expires_cannot_open_position");
        if (this._trade_account.status == 1)
          throw new AppException(1412, "account_frozen_cannot_open_position");
        StockDto stockDto = StockServices.Find(this._market, this._quote.stock_code);
        if (stockDto == null)
          throw new AppException(1400, "stock_no_exist");
        if (!stockDto.main_switch)
          throw new AppException(1404, "the_stock_cannot_open_position");
      }
      if (this._market == "VN")
        this.CheckVN(localTime);
      else
        this.CheckUS(localTime);
    }

    public static int CalculateAvailableOffsetVolume(
      List<TradePositionDto> trade_position_list,
      int stockType)
    {
      TradePositionDto tradePositionDto = trade_position_list.FirstOrDefault<TradePositionDto>((Func<TradePositionDto, bool>) (x => x.stock_type == stockType)) ?? new TradePositionDto();
      return tradePositionDto.holding_volume - tradePositionDto.new_pos - tradePositionDto.close_pos;
    }

    public static int CalculateStopLosePos(
      List<TradePositionDto> trade_position_list,
      int stockType)
    {
      return (trade_position_list.FirstOrDefault<TradePositionDto>((Func<TradePositionDto, bool>) (x => x.stock_type == stockType)) ?? new TradePositionDto()).stop_lose_pos;
    }

    private static bool IsHoliday(string market, DateTime dt)
    {
      DateTime date = OrderBLL.ConvertToUtcTime(dt, market).Date;
      foreach (StockHolidayDto stockHolidayDto in StockHolidayService.Find(market))
      {
        if (date == stockHolidayDto.date.Date && stockHolidayDto.is_allday)
          return true;
      }
      return false;
    }

    public static DateTime GetLocalTime(string market)
    {
      TimeZoneInfo destinationTimeZone;
      switch (market)
      {
        case "US":
          destinationTimeZone = OrderBLL.us_tz;
          break;
        case "VN":
          destinationTimeZone = OrderBLL.vn_tz;
          break;
        default:
          throw new AppException(1417, "unkown_market");
      }
      return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, destinationTimeZone);
    }

    public static DateTime ConvertToUtcTime(DateTime localTime, string market)
    {
      TimeZoneInfo timeZoneInfo;
      switch (market)
      {
        case "US":
          timeZoneInfo = OrderBLL.us_tz;
          break;
        case "VN":
          timeZoneInfo = OrderBLL.vn_tz;
          break;
        default:
          throw new AppException(1417, "unkown_market");
      }
      TimeZoneInfo sourceTimeZone = timeZoneInfo;
      return TimeZoneInfo.ConvertTimeToUtc(localTime, sourceTimeZone);
    }

    public static bool IsDeniedToCancelDuringOpenPeriod(string exchange, DateTime time)
    {
      OrderBLL.Period period;
      return OrderBLL._open_periods.TryGetValue(exchange, out period) && time.TimeOfDay >= period.start && time.TimeOfDay <= period.end;
    }

    public static bool IsDeniedToCancelDuringClosePeriod(string exchange, DateTime time)
    {
      OrderBLL.Period period;
      return OrderBLL._close_periods.TryGetValue(exchange, out period) && time.TimeOfDay >= period.start && time.TimeOfDay <= period.end;
    }

    private static bool IsAllowedOrder(string exchange, DateTime time)
    {
      OrderBLL.Period allowedOrderTime;
      switch (exchange)
      {
        case "HOSE":
          allowedOrderTime = OrderBLL._hose_allowed_order_time;
          break;
        case "HNX":
          allowedOrderTime = OrderBLL._hnx_allowed_order_time;
          break;
        case "UPCOM":
          allowedOrderTime = OrderBLL._upcom_allowed_order_time;
          break;
        default:
          throw new AppException(1416, "unkown_exchange");
      }
      OrderBLL.Period period = allowedOrderTime;
      return time.TimeOfDay >= period.start && time.TimeOfDay <= period.end;
    }

    private class Period
    {
      public TimeSpan start { get; set; }

      public TimeSpan end { get; set; }
    }
  }
}
