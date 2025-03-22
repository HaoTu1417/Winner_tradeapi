// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.FinanceBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Models.Finance;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
    public class FinanceBiz
    {
        public static List<GetItemsResponse> GetItems(string market, string lang)
        {
            return BorrowPlanService.GetBorrowPlans(market, lang);
        }

        public static BorrowPlanResponse GetBorrowPlan(
            BorrowPlanRequest req,
            string borrow_type,
            int member_fk)
        {
            BorrowPlanResponse borrowPlan = BorrowPlanService.GetBorrowPlan(req.market, req.lang, borrow_type, member_fk);
            borrowPlan.rates = ((IEnumerable<string>) borrowPlan.rate.Split("\r\n")).AsList<string>();
            borrowPlan.rates = borrowPlan.rates.FindAll((Predicate<string>) (e => e != ""));
            borrowPlan.fastbtns = borrowPlan.fastbtn != null ? ((IEnumerable<string>) borrowPlan.fastbtn.Split(",")).Select<string, string>((Func<string, string>) (x => x = x.Trim())).AsList<string>() : new List<string>();
            borrowPlan.currency = Tool.GetCurrencyByMarket(borrowPlan.market);
            return borrowPlan;
        }
    }
}