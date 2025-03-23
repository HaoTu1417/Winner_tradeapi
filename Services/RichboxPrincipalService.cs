// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.RichboxPrincipalService
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
  public class RichboxPrincipalService
  {
    public static Decimal TotalAmount(int member_fk)
    {
      string sql = "\nSELECT IFNULL(SUM(`amount`),0)\nFROM `richbox_principal`\nWHERE `member_fk` = @member_fk\n";
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
        LogLib.Error("[RichboxPrincipalService][TotalAmount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static Decimal DepositTotalAmount(int member_fk, DateTime begin)
    {
      string sql = "\nSELECT IFNULL(SUM(`amount`),0)\nFROM `richbox_principal`\nWHERE `member_fk` = @member_fk\nAND `amount` > 0\nAND `date` >= @begin\n";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            begin = begin
          });
          return (readConnection != null ? new Decimal?(readConnection.ExecuteScalar<Decimal>(sql, (object) parameters)) : new Decimal?()).Decimal();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxPrincipalService][DepositTotalAmount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static DateTime? FirstDate(int member_fk)
    {
      string sql = "\nSELECT MIN(`date`)\nFROM `richbox_principal`\nWHERE `member_fk` = @member_fk\n";
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
        LogLib.Error("[RichboxPrincipalService][FirstDate]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static DateTime? LastDate(int member_fk)
    {
      string sql = "\nSELECT MAX(`date`)\nFROM `richbox_principal`\nWHERE `member_fk` = @member_fk\n";
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
        LogLib.Error("[RichboxPrincipalService][LastDate]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static uint Insert(RichboxPrincipalDto model)
    {
      string sql = "\nINSERT INTO `richbox_principal`\n(`member_fk`, `pk`, `amount`, `date`, `remarks`)\nVALUES\n(@member_fk, @pk, @amount, @date, @remarks);\n\nSELECT LAST_INSERT_ID();\n";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return (writeConntion != null ? writeConntion.ExecuteScalar(sql, (object) model) : (object) null).UInt();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxPrincipalService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<RichboxPrincipalDto> FindAllByMember(int member_fk)
    {
      string sql = "\nSELECT `member_fk`, `pk`, `amount`, `date`, `remarks`\nFROM `richbox_principal`\nWHERE `member_fk` = @member_fk\nORDER BY `date`,`pk`\n";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return (readConnection != null ? readConnection.Query<RichboxPrincipalDto>(sql, (object) parameters) : (IEnumerable<RichboxPrincipalDto>) null).AsList<RichboxPrincipalDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichboxPrincipalService][FindAllByMember]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
