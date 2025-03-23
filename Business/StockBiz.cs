// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.StockBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.IndChart;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Business
{
  public class StockBiz
  {
    private static object _lock = new object();

    public static List<StockOptionDto> GetAllStockOption(string market)
    {
      return StockOptionService.FindAll(market);
    }

    public static List<GetStockOptionPotitionResponse> GetAllStockOptionPosition(int member_fk)
    {
      try
      {
        string table1 = HistoryDailyService.FindTable("VN");
        string table2 = HistoryDailyService.FindTable("US");
        DateTime maxDate1 = HistoryDailyService.FindMaxDate(table1);
        DateTime maxDate2 = HistoryDailyService.FindMaxDate(table2);
        List<StockHistoryList> historyByDate1 = HistoryDailyService.FindHistoryByDate(table1, maxDate1.ToString("yyyy-MM-dd"));
        List<StockHistoryList> historyByDate2 = HistoryDailyService.FindHistoryByDate(table2, maxDate2.ToString("yyyy-MM-dd"));
        List<GetStockOptionPotitionResponse> byMember = StockOptionPositionService.FindByMember(member_fk);
        foreach (GetStockOptionPotitionResponse potitionResponse in byMember)
        {
          StockHistoryList stockHistoryList = new StockHistoryList()
          {
            stock_code = potitionResponse.stock_code
          };
          if (potitionResponse.market.ToLower() == "vn")
          {
            int index = historyByDate1.BinarySearch(stockHistoryList, (IComparer<StockHistoryList>) new StockBiz.Comp());
            potitionResponse.sell_price = index >= 0 ? historyByDate1[index].close : 0M;
          }
          else
          {
            int index = historyByDate2.BinarySearch(stockHistoryList, (IComparer<StockHistoryList>) new StockBiz.Comp());
            potitionResponse.sell_price = index >= 0 ? historyByDate2[index].close : 0M;
          }
        }
        return byMember;
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockBiz][GetAllStockOptionPosition]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<GetStockOptionDetailResponse> GetStockOptionRecordDetail(
      int member_fk,
      string market,
      string stock_code)
    {
      return StockOptionRecordService.Find(member_fk, market, stock_code);
    }

    public static int GetAvailableQuantity(int member_fk, int stock_option_fk)
    {
      StockOptionDto stockOptionDto = StockOptionService.Find(stock_option_fk);
      List<StockOptionRecordDto> allByMember = StockOptionRecordService.FindAllByMember(member_fk, stock_option_fk);
      return allByMember != null ? stockOptionDto.quantity - allByMember.Sum<StockOptionRecordDto>((Func<StockOptionRecordDto, int>) (x => x.quantity)) : stockOptionDto.quantity;
    }

    public static void BuyStockOption(
      int member_fk,
      int stock_option_fk,
      int quantity,
      string market = "vn")
    {
      int availableQuantity = StockBiz.GetAvailableQuantity(member_fk, stock_option_fk);
      if (quantity <= 0 || availableQuantity <= 0 || availableQuantity < quantity)
        throw new AppException(1621, "stock_option_quantity_not_enough");
      StockOptionDto stockOptionDto = StockOptionService.Find(stock_option_fk);
      StockDto byCode = StockServices.FindByCode(stockOptionDto.market, stockOptionDto.stock_code);
      if (byCode == null)
        throw new AppException(1575, "stock_name_error");
      Decimal num = stockOptionDto.price * (Decimal) quantity;
      if (!WalletLib.IsRequestrMoneyOk(member_fk, num))
        throw new AppException(1803, "insufficient_balance");
      using (TransactionScope transactionScope = new TransactionScope())
      {
        lock (StockBiz._lock)
        {
          stockOptionDto = StockOptionService.Find(stock_option_fk);
          if (stockOptionDto.remain_spot <= 0)
            throw new AppException(1622, "stock_option_spot_not_enough");
          if (availableQuantity == stockOptionDto.quantity)
            StockOptionService.UpdateRemainSpot(stock_option_fk, -1);
        }
        Decimal change = ExchangeLib.Convert(num, stockOptionDto.currency, ConfigLib.Get("wallet_currency"));
        if (WalletLib.BuyStockOption(member_fk, change, stockOptionDto.stock_code))
        {
          MemberDto memberDto = MemberServices.Find(member_fk);
          StockOptionPositionDto model = StockOptionPositionService.Find(memberDto.pk, byCode.stock_code, stockOptionDto.market.ToUpper());
          if (model == null)
          {
            StockOptionPositionService.Insert(new StockOptionPositionDto()
            {
              member_fk = memberDto.pk,
              market = stockOptionDto.market.ToUpper(),
              stock_code = byCode.stock_code,
              stock_name = byCode.stock_name,
              quantity = quantity,
              freeze = 0,
              last_price = stockOptionDto.price,
              total_cost = change,
              currency = stockOptionDto.currency
            });
          }
          else
          {
            model.quantity += quantity;
            model.total_cost += change;
            model.last_price = stockOptionDto.price;
            StockOptionPositionService.BuyUpdate(model);
          }
          StockOptionRecordService.FindPkAfterInsert(new StockOptionRecordDto()
          {
            member_fk = member_fk,
            market = stockOptionDto.market,
            stock_option_fk = stock_option_fk,
            stock_code = stockOptionDto.stock_code,
            stock_name = stockOptionDto.stock_name,
            currency = stockOptionDto.currency,
            type = 1,
            price = stockOptionDto.price,
            quantity = quantity,
            total = change,
            status = 2,
            create_time = DateTime.UtcNow
          });
        }
        transactionScope.Complete();
      }
    }

    public static void SellStockOption(
      int member_fk,
      string stock_code,
      int quantity,
      string market = "vn")
    {
      if (quantity <= 0)
        throw new AppException(1621, "stock_option_quantity_not_enough");
      StockDto byCode = StockServices.FindByCode(market, stock_code);
      if (byCode == null)
        throw new AppException(1575, "stock_name_error");
      StockOptionPositionDto model = StockOptionPositionService.Find(member_fk, stock_code, market.ToUpper());
      if (model == null)
        throw new AppException(1621, "no_stock_option_position");
      if (model.quantity - model.freeze <= 0)
        throw new AppException(1624, "sell_stock_option_quantity_not_enough");
      List<StockHistoryList> history = IndChartBiz.GetHistory("daily", market, stock_code);
      if (history == null || history.Count == 0)
        throw new AppException(1625, "sell_stock_option_history_not_found");
      StockHistoryList stockHistoryList = history.OrderByDescending<StockHistoryList, string>((Func<StockHistoryList, string>) (x => x.date)).ToList<StockHistoryList>()[0];
      using (TransactionScope transactionScope = new TransactionScope())
      {
        StockOptionRecordService.FindPkAfterInsert(new StockOptionRecordDto()
        {
          member_fk = member_fk,
          market = market,
          stock_code = stock_code,
          stock_name = byCode.stock_name,
          currency = model.currency,
          type = 2,
          price = stockHistoryList.close,
          quantity = quantity,
          total = stockHistoryList.close * (Decimal) quantity,
          status = 1,
          create_time = DateTime.UtcNow
        });
        model.freeze += quantity;
        StockOptionPositionService.SellUpdate(model);
        transactionScope.Complete();
      }
    }

    public class Comp : IComparer<StockHistoryList>
    {
      public int Compare(StockHistoryList x, StockHistoryList y)
      {
        return string.Compare(x.stock_code, y.stock_code);
      }
    }
  }
}
