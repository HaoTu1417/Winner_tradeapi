// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.SmartBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Collections.Generic;
using tradeapi.Models.Smart;

#nullable enable
namespace tradeapi.Business
{
    public class SmartBiz
    {
        public static List<GetRankingResponse> GetRanking()
        {
            return new List<GetRankingResponse>()
            {
                new GetRankingResponse()
                {
                    sn = 1,
                    stock_code = "AAPL",
                    stock_name = "Apple Inc.",
                    final_price = 150.25M,
                    recommend = 4
                },
                new GetRankingResponse()
                {
                    sn = 2,
                    stock_code = "MSFT",
                    stock_name = "Microsoft Corporation",
                    final_price = 305.45M,
                    recommend = 5
                }
            };
        }
    }
}