// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.OrderBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using BLLs;
using DB.Services;
using Models.Dto;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi.Models.Enums;
using tradeapi.Models.Order;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class OrderBiz
  {
    private const bool CHECK_ORDER = true;

    public static PreSellResponse GetPreSell(
      string market,
      string sub_account,
      int price_type,
      string stock_code,
      Decimal price,
      int volume)
    {
      List<TradePositionDto> all = TradePositionService.FindAll(market, sub_account, stock_code);
      TradeAccountDto trade_account = TradeAccountService.Find(sub_account);
      StockQuote quote = StockQuoteService.GetStockQuote(market, stock_code) ?? throw new AppException(1400, "stock_no_exist");
      new OrderBLL(market, price_type, "S", volume, price, quote, all, trade_account).CheckOrder();
      return new PreSellResponse()
      {
        sub_account = sub_account,
        market = market,
        price_type = price_type,
        stock_code = stock_code,
        stock_name = quote.stock_name,
        price = price,
        volume = volume
      };
    }

    public static PreBuyResponse GetPreBuy(
      string market,
      string sub_account,
      int price_type,
      string stock_code,
      Decimal price,
      int volume)
    {
      List<TradePositionDto> all = TradePositionService.FindAll(market, sub_account, stock_code);
      TradeAccountDto trade_account = TradeAccountService.Find(sub_account);
      StockQuote quote = StockQuoteService.GetStockQuote(market, stock_code) ?? throw new AppException(1400, "stock_no_exist");
      new OrderBLL(market, price_type, "B", volume, price, quote, all, trade_account).CheckOrder();
      return new PreBuyResponse()
      {
        sub_account = sub_account,
        market = market,
        price_type = price_type,
        stock_code = stock_code,
        stock_name = quote.stock_name,
        price = price,
        volume = volume
      };
    }

    public static void Cancel(int member_id, string ip, string trade_order_sn)
    {
      TradeOrderDto tradeOrderDto = TradeOrderService.FindBySN(trade_order_sn) ?? throw new AppException(1700, "order_does_not_exist");
      if (tradeOrderDto.status == 1 || tradeOrderDto.status == 3 || tradeOrderDto.status == 4)
        throw new AppException(1740, "order_ended");
      StockDto stockDto = StockServices.Find(tradeOrderDto.market, tradeOrderDto.stock_code) ?? throw new AppException(1400, "stock_no_exist");
      DateTime localTime = OrderBLL.GetLocalTime(tradeOrderDto.market);
      if (OrderBLL.IsDeniedToCancelDuringOpenPeriod(stockDto.exchange, localTime))
        throw new AppException(1720, "cannot_order_opening_auction");
      if (OrderBLL.IsDeniedToCancelDuringClosePeriod(stockDto.exchange, localTime))
        throw new AppException(1730, "cannot_order_closing_auction");
      MemberDto memberDto = MemberServices.Find(member_id);
      OrderHelper.CancelOrder(tradeOrderDto.pk, 1, memberDto.account);
    }

    public static OrderResponse Order(
      int member_fk,
      string market,
      string sub_account,
      int price_type,
      string stock_code,
      int dir,
      Decimal price,
      int volume,
      string order_ip)
    {
      List<TradePositionDto> all = TradePositionService.FindAll(market, sub_account, stock_code);
      TradeAccountDto trade_account = TradeAccountService.Find(sub_account);
      StockQuote quote = StockQuoteService.GetStockQuote(market, stock_code) ?? throw new AppException(1400, "stock_no_exist");
      string str;
      if (dir != 1)
      {
        if (dir != 2)
          throw new AppException(1414, "dir_error");
        str = "S";
      }
      else
        str = "B";
      string buyOrSell = str;
      new OrderBLL(market, price_type, buyOrSell, volume, price, quote, all, trade_account).CheckOrder();
      string stockName = quote.stock_name;
      TradeOrderDto tradeOrderDto = new TradeOrderDto();
      switch (price_type)
      {
        case 1:
        case 2:
          int availableOffsetVolume = OrderBLL.CalculateAvailableOffsetVolume(all, dir == 1 ? -1 : 1);
          if (volume > availableOffsetVolume)
          {
            if (availableOffsetVolume > 0)
              OrderHelper.PlaceOffsetOrder(sub_account, market, stock_code, stockName, price_type, dir, price, availableOffsetVolume, order_ip, 1);
            tradeOrderDto = OrderHelper.PlaceNewOrder(sub_account, market, stock_code, stockName, price_type, dir, price, volume - availableOffsetVolume, order_ip, 1, quote);
            break;
          }
          tradeOrderDto = OrderHelper.PlaceOffsetOrder(sub_account, market, stock_code, stockName, price_type, dir, price, volume, order_ip, 1);
          break;
        case 3:
          tradeOrderDto = OrderHelper.PlaceStopOrder(sub_account, market, stock_code, stockName, price_type, dir, price, volume, order_ip, 1);
          break;
      }
      return new OrderResponse()
      {
        trade_order_sn = tradeOrderDto.sn,
        stock_code = stock_code,
        stock_name = stockName,
        market = market,
        dir = (OrderSide) dir,
        price_type = price_type,
        price = price,
        volume = volume
      };
    }

    public static OrderHoldingResponse GetHolding(
      string market,
      string sub_account,
      string stock_code,
      string lang)
    {
      List<TradePositionDto> all = TradePositionService.FindAll(market, sub_account, stock_code);
      List<TradePositionDto> bySubAccount = TradePositionService.FindBySubAccount(sub_account);
      StockDto stockDto = StockServices.Find(market, stock_code);
      TradePositionDto tradePositionDto1 = all.Find((Predicate<TradePositionDto>) (x => x.stock_type == 1)) ?? new TradePositionDto();
      TradePositionDto tradePositionDto2 = all.Find((Predicate<TradePositionDto>) (x => x.stock_type == -1)) ?? new TradePositionDto();
      int num1 = tradePositionDto1.holding_volume - tradePositionDto1.new_pos - tradePositionDto1.close_pos;
      int num2 = tradePositionDto2.holding_volume - tradePositionDto2.new_pos - tradePositionDto2.close_pos;
      TradeAccountDto tradeAccountDto = TradeAccountService.Find(sub_account);
      Decimal balance = tradeAccountDto.mem_money - tradeAccountDto.frozen_money;
      Decimal num3 = tradeAccountDto.margin + tradeAccountDto.loan_money;
      Decimal num4 = (tradePositionDto1.cost_purchase + tradePositionDto2.cost_purchase) / num3 * 100M;
      Decimal num5 = bySubAccount.Sum<TradePositionDto>((Func<TradePositionDto, Decimal>) (x => x.cost_purchase)) / num3 * 100M;
      string str = stockDto?.exchange.Text();
      int num6;
      switch (str)
      {
        case "HOSE":
          num6 = 10;
          break;
        case "HNX":
        case "UPCOM":
          num6 = 100;
          break;
        default:
          num6 = 1;
          break;
      }
      int base_value = num6;
      string currencyByMarket = Tool.GetCurrencyByMarket(market);
      int num7 = num1 - num2;
      int fullPosition = OrderHelper.GetFullPosition(market, stock_code, balance, base_value);
      return new OrderHoldingResponse()
      {
        stock_code = stock_code,
        exchange = str,
        holding = num7,
        currency = currencyByMarket,
        balance = balance,
        stock_cost_percent = num4,
        total_cost_percent = num5,
        base_value = base_value,
        full_position = fullPosition,
        avalible = stockDto != null && stockDto.main_switch
      };
    }
  }
}
