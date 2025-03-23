// Decompiled with JetBrains decompiler
// Type: DB.Services.StockOptionRecordService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class StockOptionRecordService
  {
    public static StockOptionRecordDto Find(int pk)
    {
      string sql = "SELECT * FROM `stock_option_record` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<StockOptionRecordDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordDto][Find]" + ex.Message);
        return (StockOptionRecordDto) null;
      }
    }

    public static List<GetStockOptionDetailResponse> Find(
      int member_fk,
      string market,
      string stock_code)
    {
      string sql = "SELECT * FROM `stock_option_record`\n                           WHERE `member_fk` = @member_fk AND `market` = @market AND `stock_code` = @stock_code";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            market = market,
            stock_code = stock_code
          });
          return readConnection.Query<GetStockOptionDetailResponse>(sql, (object) parameters).AsList<GetStockOptionDetailResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordDto][Find]" + ex.Message);
        return (List<GetStockOptionDetailResponse>) null;
      }
    }

    public static List<StockOptionRecordDto> FindAllByMember(int member_fk, int stock_option_fk)
    {
      string sql = "SELECT * FROM `stock_option_record` WHERE member_fk = @member_fk AND stock_option_fk = @stock_option_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            stock_option_fk = stock_option_fk
          });
          return readConnection.Query<StockOptionRecordDto>(sql, (object) parameters).AsList<StockOptionRecordDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordDto][FindAllByMember]" + ex.Message);
        return (List<StockOptionRecordDto>) null;
      }
    }

    public static List<StockOptionRecordDto> FindAllByMember(int member_fk)
    {
      string sql = "SELECT * FROM `stock_option_record` WHERE member_fk = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<StockOptionRecordDto>(sql, (object) parameters).AsList<StockOptionRecordDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordDto][FindAllByMember]" + ex.Message);
        return (List<StockOptionRecordDto>) null;
      }
    }

    public static List<StockOptionRecordDto> FindAll()
    {
      string sql = "SELECT * FROM `stock_option_record`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<StockOptionRecordDto>(sql).AsList<StockOptionRecordDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordDto][FindAll]" + ex.Message);
        return (List<StockOptionRecordDto>) null;
      }
    }

    public static int FindPkAfterInsert(StockOptionRecordDto source)
    {
      string sql = "INSERT INTO `stock_option_record` (\n                `member_fk`, `market`, `stock_option_fk`, `stock_code`, `stock_name`, `currency`, `type`, `price`, `quantity`, `total`, `status`, `create_time`)\n                VALUES (@member_fk, @market, @stock_option_fk, @stock_code, @stock_name, @currency, @type, @price, @quantity, @total, @status, @create_time);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(StockOptionRecordDto model)
    {
      string sql = "UPDATE `stock_option_record` SET \n                `member_fk` = @member_fk,\n                `stock_optoin_fk` = @stock_option_fk,\n                `stock_code` = @stock_code,\n                `price` = @price,\n                `quantity` = @quantity,\n                `total` = @total,\n                `status` = @status,\n                `create_time` = @create_time\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateStatus(int pk, int status)
    {
      string sql = "UPDATE `stock_option_record` SET \n                `status` = @status\n                 WHERE `pk` = @pk";
      try
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          pk = pk,
          status = status
        });
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) parameters);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordService][UpdateStatus]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int NotifyPayment(int pk, int payment, string last5)
    {
      string sql = "UPDATE `stock_option_record` SET \n                `payment` = @payment\n                `last5` = @last5\n                 WHERE `pk` = @pk";
      try
      {
        DynamicParameters parameters = DapperMysql.GetParameters((object) new
        {
          pk = pk,
          payment = payment,
          last5 = last5
        });
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) parameters);
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordService][NotifyPayment]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `stock_option_record` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[StockOptionRecordService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
