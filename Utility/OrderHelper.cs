// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.OrderHelper
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using tradeapi.Cache;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Utility
{
  public class OrderHelper
  {
    private static Dictionary<string, FeeRate> feeRateDict = new Dictionary<string, FeeRate>();

    static OrderHelper()
    {
      foreach (SysMarketDto sysMarketDto in SysMarketService.FindAll())
        OrderHelper.feeRateDict[sysMarketDto.code] = new FeeRate()
        {
          buy_fee_rate = sysMarketDto.buy_fee,
          min_buy_fee = sysMarketDto.min_buy_fee,
          sell_fee_rate = sysMarketDto.sell_fee,
          min_sell_fee = sysMarketDto.min_sell_fee
        };
    }

    public static TradeOrderDto PlaceNewOrder(
      string sub_account,
      string market,
      string stock_code,
      string stock_name,
      int price_type,
      int dir,
      Decimal price,
      int volume,
      string order_ip,
      int order_source,
      StockQuote quote)
    {
      TradeOrderDto newOrder = OrderHelper.CreateNewOrder(sub_account, market, stock_code, stock_name, 1, price_type, dir, price, volume, order_ip, order_source);
      Decimal amount = (price_type == 1 ? price : quote.ceiling) * (Decimal) volume;
      Decimal frozen_money = amount + OrderHelper.GetHandlingFee(market, newOrder.dir, amount);
      TradeFrozenService.FindPkAfterInsert(new TradeFrozenDto()
      {
        sub_account = sub_account,
        trade_order_fk = newOrder.pk,
        info = "",
        type = 0,
        frozen_volume = volume,
        frozen_money = frozen_money,
        frozen_datetime = DateTime.UtcNow
      });
      TradeAccountService.AddFrozenMoney(sub_account, frozen_money);
      return newOrder;
    }

    public static TradeOrderDto PlaceOffsetOrder(
      string sub_account,
      string market,
      string stock_code,
      string stock_name,
      int price_type,
      int dir,
      Decimal price,
      int volume,
      string order_ip,
      int order_source)
    {
      TradeOrderDto newOrder = OrderHelper.CreateNewOrder(sub_account, market, stock_code, stock_name, 2, price_type, dir, price, volume, order_ip, order_source);
      TradePositionService.AddClosePos(sub_account, stock_code, volume);
      return newOrder;
    }

    public static TradeOrderDto PlaceStopOrder(
      string sub_account,
      string market,
      string stock_code,
      string stock_name,
      int price_type,
      int dir,
      Decimal price,
      int volume,
      string order_ip,
      int order_source)
    {
      TradeOrderDto newOrder = OrderHelper.CreateNewOrder(sub_account, market, stock_code, stock_name, 2, price_type, dir, price, volume, order_ip, order_source);
      TradePositionService.AddStopClosePos(sub_account, stock_code, volume);
      return newOrder;
    }

    private static TradeOrderDto CreateNewOrder(
      string sub_account,
      string market,
      string stock_code,
      string stock_name,
      int order_type,
      int price_type,
      int dir,
      Decimal price,
      int volume,
      string order_ip,
      int order_source)
    {
      DateTime utcNow = DateTime.UtcNow;
      TradeOrderDto tradeOrderDto = new TradeOrderDto();
      tradeOrderDto.sub_account = sub_account;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
      interpolatedStringHandler.AppendFormatted(dir == 1 ? "B" : "S");
      interpolatedStringHandler.AppendFormatted<DateTime>(DateTime.UtcNow, "yyMMdd");
      tradeOrderDto.sn = OrderHelper.MakeTradeOrderSn(interpolatedStringHandler.ToStringAndClear());
      tradeOrderDto.stock_code = stock_code;
      tradeOrderDto.stock_name = stock_name;
      tradeOrderDto.market = market;
      tradeOrderDto.dir = dir;
      tradeOrderDto.order_type = order_type;
      tradeOrderDto.price_type = price_type;
      tradeOrderDto.price = price;
      tradeOrderDto.status = 0;
      tradeOrderDto.volume = volume;
      tradeOrderDto.free_volume = volume;
      tradeOrderDto.order_time = utcNow;
      tradeOrderDto.order_ip = order_ip;
      tradeOrderDto.order_source = order_source;
      TradeOrderDto source = tradeOrderDto;
      source.pk = TradeOrderService.FindPkAfterInsert(source);
      return source;
    }

    public static void CancelOrder(int order_pk, int cancel_type, string cancel_by)
    {
      using (TransactionScope transactionScope = new TransactionScope())
      {
        TradeOrderDto order = TradeOrderService.Find(order_pk);
        DateTime utcNow = DateTime.UtcNow;
        TradeOrderService.CancelOrder(order_pk, cancel_type, cancel_by);
        order.free_volume = 0;
        order.cancel_volume = order.volume - order.succeed_volume;
        OrderHelper.EvaluateOrderStatus(order);
        TradeOrderService.UpdateStatus(order.pk, order.status);
        TradeCancelService.FindPkAfterInsert(new TradeCancelDto()
        {
          trade_order_sn = order.sn,
          sub_account = order.sub_account,
          sn = OrderHelper.MakeTradeCancelSn(),
          market = order.market,
          stock_code = order.stock_code,
          stock_name = order.stock_name,
          request_volume = order.volume,
          cancel_volume = order.volume,
          order_client = order.order_client,
          cancel_type = cancel_type,
          cancel_datetime = utcNow,
          cancel_by = cancel_by
        });
        OrderHelper.ReleaseFrozen(order);
        transactionScope.Complete();
      }
    }

    public static void ReleaseFrozen(TradeOrderDto order)
    {
      switch (order.order_type)
      {
        case 1:
          OrderHelper.ReleaseTradeFrozenMoney(order);
          break;
        case 2:
          if (TradePositionService.Find(order.sub_account, order.stock_code, order.market) == null)
            break;
          if (order.price_type == 3)
          {
            TradePositionService.ReduceStopLosePos(order.sub_account, order.stock_code, order.volume);
            break;
          }
          TradePositionService.ReduceClosePos(order.sub_account, order.stock_code, order.volume);
          break;
      }
    }

    public static void ReleaseTradeFrozenMoney(TradeOrderDto order)
    {
      TradeFrozenDto byOrderId = TradeFrozenService.FindByOrderId(order.pk);
      if (byOrderId == null)
        return;
      TradeFrozenService.Remove(order.pk);
      TradeAccountService.ReleaseForzen(order.sub_account, byOrderId.frozen_money);
    }

    public static void EvaluateOrderStatus(TradeOrderDto order)
    {
      if (order.succeed_volume + order.cancel_volume == order.volume)
      {
        if (order.succeed_volume == order.volume)
          order.status = 1;
        else if (order.cancel_volume == order.volume)
        {
          order.status = 3;
        }
        else
        {
          if (order.succeed_volume <= 0 || order.cancel_volume <= 0)
            return;
          order.status = 4;
        }
      }
      else
      {
        if (order.succeed_volume <= 0 || order.free_volume <= 0)
          return;
        order.status = 2;
      }
    }

    private static string MakeTradeOrderSn(string prefix)
    {
      string sn = "";
      for (bool flag = true; flag; flag = TradeOrderService.IsDuplicateSN(sn))
        sn = prefix + Tool.MakeRandomNo(6);
      return sn;
    }

    public static string MakeTradeMoneyRecordSn()
    {
      string sn = "";
      for (bool flag = true; flag; flag = TradeMoneyRecordService.IsDuplicateSN(sn))
        sn = Tool.MakeRandomNo(20);
      return sn;
    }

    private static string MakeTradeCancelSn()
    {
      string sn = "";
      for (bool flag = true; flag; flag = TradeCancelService.IsDuplicateSN(sn))
        sn = Tool.MakeRandomNo(11);
      return sn;
    }

    public static Decimal GetHandlingFee(string market, int dir, Decimal amount)
    {
      Decimal handlingFee;
      switch (dir)
      {
        case 1:
          handlingFee = Math.Max(Math.Ceiling(amount * OrderHelper.feeRateDict[market].buy_fee_rate), OrderHelper.feeRateDict[market].min_buy_fee);
          break;
        case 2:
          handlingFee = Math.Max(Math.Ceiling(amount * OrderHelper.feeRateDict[market].sell_fee_rate), OrderHelper.feeRateDict[market].min_sell_fee);
          break;
        default:
          handlingFee = 0M;
          break;
      }
      return handlingFee;
    }

    public static Decimal GetStampFee(int dir, Decimal amount)
    {
      int stampFee;
      switch (dir)
      {
        case 1:
          stampFee = 0;
          break;
        case 2:
          stampFee = 0;
          break;
        default:
          stampFee = 0;
          break;
      }
      return (Decimal) stampFee;
    }

    public static Decimal GetTransferFee(string market, int volume) => 0M;

    public static int GetFullPosition(
      string market,
      string stock_code,
      Decimal balance,
      int base_value)
    {
      StockQuote stockQuote = StockQuoteService.GetStockQuote(market, stock_code);
      Decimal price = !(market == "VN") ? stockQuote.price * 1.2M : stockQuote.ceiling;
      return OrderHelper.CalculateAvailableNewPos(market, balance, price, base_value);
    }

    private static int CalculateAvailableNewPos(
      string market,
      Decimal balance,
      Decimal price,
      int base_value)
    {
      if (price <= 0M)
        return 0;
      int availableNewPos = (int) (balance / price) / base_value * base_value;
      while (true)
      {
        Decimal amount = (Decimal) availableNewPos * price;
        Decimal handlingFee = OrderHelper.GetHandlingFee(market, 1, amount);
        if (!(balance >= amount + handlingFee) && availableNewPos != 0)
          availableNewPos = Math.Max(availableNewPos - base_value, 0);
        else
          break;
      }
      return availableNewPos;
    }
  }
}
