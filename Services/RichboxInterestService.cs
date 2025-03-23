// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.RichboxInterestService
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
  public class RichboxInterestService
  {
    public static Decimal TotalRecordedAmount(int member_fk)
    {
      string sql = "\nSELECT IFNULL(SUM(`amount`),0)\nFROM `richbox_interest`\nWHERE `member_fk` = @member_fk\n";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return (readConnection != null ? new Decimal?(readConnection.ExecuteScalar<Decimal>(sql, (object) parameters)) : new Decimal?()).Decimal();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxInterestService][TotalRecordedAmount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static Decimal GrandTotalRecordedAmount(int member_fk)
    {
      string sql = "\nSELECT IFNULL(SUM(`amount`),0)\nFROM `richbox_interest`\nWHERE `member_fk` = @member_fk\nAND `amount` > 0\n";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return (readConnection != null ? new Decimal?(readConnection.ExecuteScalar<Decimal>(sql, (object) parameters)) : new Decimal?()).Decimal();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxInterestService][GrandTotalRecordedAmount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static DateTime? LastDate(int member_fk)
    {
      string sql = "\nSELECT MAX(`date`)\nFROM `richbox_interest`\nWHERE `member_fk` = @member_fk\n";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection != null ? readConnection.ExecuteScalar<DateTime?>(sql, (object) parameters) : new DateTime?();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxInterestService][LastDate]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static uint Insert(RichboxInterestDto model)
    {
      string sql = "\nINSERT INTO `richbox_interest`\n(`member_fk`, `pk`, `amount`, `date`, `remarks`)\nVALUES\n(@member_fk, @pk, @amount, @date, @remarks);\n\nSELECT LAST_INSERT_ID();\n";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return (writeConntion != null ? writeConntion.ExecuteScalar(sql, (object) model) : (object) null).UInt();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxInterestService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<RichboxInterestDto> FindAllByMember(int member_fk)
    {
      string sql = "\nSELECT `member_fk`, `pk`, `amount`, `date`, `remarks`\nFROM `richbox_interest`\nWHERE `member_fk` = @member_fk\nORDER BY `date`,`pk`\n";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return (readConnection != null ? readConnection.Query<RichboxInterestDto>(sql, (object) parameters) : (IEnumerable<RichboxInterestDto>) null).AsList<RichboxInterestDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxInterestService][FindAllByMember]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
