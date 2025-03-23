// Decompiled with JetBrains decompiler
// Type: DB.Services.StockOptionPositionService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class StockOptionPositionService
  {
    public static StockOptionPositionDto? Find(int member_fk, string code, string market)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM stock_option_position WHERE member_fk = @member_fk AND stock_code = @stock_code AND market = @market";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            stock_code = code,
            market = market
          });
          return readConnection.QuerySingleOrDefault<StockOptionPositionDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionPositionService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<GetStockOptionPotitionResponse> FindByMember(int member_fk)
    {
      string sql = "\nSELECT * FROM `stock_option_position`\nWHERE `member_fk` = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<GetStockOptionPotitionResponse>(sql, (object) parameters).ToList<GetStockOptionPotitionResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionPositionService][FindByMember]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void Insert(StockOptionPositionDto model)
    {
      string sql = "\nINSERT INTO `stock_option_position`\n(`member_fk`, `market`, `stock_code`, `stock_name`, `quantity`, `freeze`, `last_price`, `total_cost`)\nVALUES\n(@member_fk, @market, @stock_code, @stock_name, @quantity, @freeze, @last_price, @total_cost)\n";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionPositionService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int BuyUpdate(StockOptionPositionDto model)
    {
      string sql = "\nUPDATE `stock_option_position`\nSET `quantity` = @quantity ,`last_price` = @last_price ,`total_cost` = @total_cost\nWHERE `member_fk` = @member_fk AND `stock_code` = @stock_code AND `market` = @market\n;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionPositionService][BuyUpdate]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int SellUpdate(StockOptionPositionDto model)
    {
      string sql = "\nUPDATE `stock_option_position`\nSET `freeze` = @freeze \nWHERE `member_fk` = @member_fk AND `stock_code` = @stock_code AND `market` = @market\n;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionPositionService][SellUpdate]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
