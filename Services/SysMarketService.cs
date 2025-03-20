// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.SysMarketService
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
  public class SysMarketService
  {
    public static SysMarketDto Find(string code)
    {
      string sql = "SELECT * FROM `sys_market` WHERE `code` = @code";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            code = code
          });
          return readConnection.QueryFirstOrDefault<SysMarketDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysMarketService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<SysMarketDto> FindAll()
    {
      string sql = "SELECT * FROM `sys_market`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<SysMarketDto>(sql).AsList<SysMarketDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysMarketService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<SysMarketDto> GetOpenMarkets(string lang)
    {
      string sql = "SELECT `code` AS code, `exchange` AS exchange, g.`value` AS label\n                       FROM sys_market\n                       LEFT JOIN mutilang_table g ON g.`key` = sys_market.`code`\n                       WHERE sys_market.enable = 1 AND g.`dbtable` = 'sys_market' AND g.`field` = 'name' AND g.`lang` = @lang ORDER BY CASE WHEN sys_market.`code` = 'VN' THEN 0 ELSE 1 END, sort";
      try
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          lang = lang
        });
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<SysMarketDto>(sql, (object) parameters).AsList<SysMarketDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[SysMarketService][GetOpenMarkets]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
