// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.StockHolidayService
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
    public class StockHolidayService
    {
        public static List<StockHolidayDto> Find(string market)
        {
            string sql = "SELECT * FROM `stock_holiday` WHERE market = @market";
            var data = new{ market = market };
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                    return readConnection.Query<StockHolidayDto>(sql, (object) data).AsList<StockHolidayDto>();
            }
            catch (Exception ex)
            {
                LogLib.Error("[StockHolidayService][Find]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}