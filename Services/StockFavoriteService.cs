// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.StockFavoriteService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class StockFavoriteService
  {
    public static void Insert(StockFavoriteDto stock_favorite)
    {
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
      {
        string sql = "INSERT INTO `stock_favorite` (`member_fk`, `market`, `stock_code`) VALUES (@member_fk, @market, @stock_code);";
        writeConntion.Execute(sql, (object) stock_favorite);
      }
    }

    public static void Delete(int member_fk, string market, string stock_code)
    {
      using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
      {
        string sql = "\nDELETE FROM `stock_favorite`\nWHERE `member_fk` = @member_fk\nAND `market` = @market\nAND `stock_code` = @stock_code\n;";
        writeConntion.Execute(sql, (object) new
        {
          member_fk = member_fk,
          market = market,
          stock_code = stock_code
        });
      }
    }

    public static List<string> FindFavourite(int member_fk, string market)
    {
      using (IDbConnection readConnection = DapperMysql.GetReadConnection())
      {
        string sql = "SELECT `stock_code` FROM `stock_favorite` WHERE `member_fk` = @member_fk AND `market` = @market";
        return readConnection.Query<string>(sql, (object) new
        {
          member_fk = member_fk,
          market = market.ToUpper()
        }).AsList<string>();
      }
    }

    public static bool IsExists(int member_fk, string market, string stock_code)
    {
      string sql = "SELECT EXISTS(SELECT `stock_code` FROM `stock_favorite` WHERE `member_fk` = @member_fk AND `market` = @market AND `stock_code` = @stock_code)";
      var data = new
      {
        member_fk = member_fk,
        market = market,
        stock_code = stock_code
      };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingle<bool>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockFavoriteService][IsExists]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
