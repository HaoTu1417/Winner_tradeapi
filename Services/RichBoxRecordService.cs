// Decompiled with JetBrains decompiler
// Type: DB.Services.RichBoxRecordService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class RichBoxRecordService
  {
    public static IEnumerable<RichBoxRecordDto> FindAll(int member_fk)
    {
      string sql = "SELECT * FROM `richbox_record` WHERE member_fk = @member_fk ORDER BY create_time DESC";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<RichBoxRecordDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichBoxRecordService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(RichBoxRecordDto source)
    {
      string sql = "INSERT INTO `richbox_record` (\n                `member_fk`, `affect`, `balance`, `src`, `create_time`, `interest_rate`)\n                VALUES (@member_fk, @affect, @balance, @src, @create_time, @interest_rate);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichBoxRecordService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<RichBoxRecordDto> FindAllByMember(int member_fk)
    {
      string sql = "\n                        SELECT *\n                        FROM `richbox_record`\n                        WHERE `member_fk` = @member_fk\n                        ORDER BY `create_time`,`pk`\n                        ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return (readConnection != null ? readConnection.Query<RichBoxRecordDto>(sql, (object) parameters) : (IEnumerable<RichBoxRecordDto>) null).AsList<RichBoxRecordDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichBoxRecordService][FindAllByMember]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static RichBoxRecordDto FindLastInterestRec(int member_fk)
    {
      string sql = "SELECT *\n                           FROM `richbox_record`\n                           WHERE member_fk = @member_fk AND src = 2\n                           ORDER BY create_time desc LIMIT 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QuerySingleOrDefault<RichBoxRecordDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichBoxRecordService][FindLastInterestRec]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<RichBoxRecordDto> FindAllExcludeInterest()
    {
      string sql = "SELECT * FROM `richbox_record` WHERE src != 2 ORDER BY create_time";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RichBoxRecordDto>(sql).AsList<RichBoxRecordDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichBoxRecordService][FindAllExcludeInterest]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int InsertRecords(List<RichBoxRecordDto> records)
    {
      List<string> values = new List<string>();
      foreach (RichBoxRecordDto record in records)
        values.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}')", (object) record.member_fk, (object) record.affect, (object) record.balance, (object) 2, (object) record.create_time.ToString("yyyy-MM-dd HH:mm:ss"), (object) record.interest_rate));
      StringBuilder stringBuilder = new StringBuilder("INSERT INTO `richbox_record` (\n                `member_fk`, `affect`, `balance`, `src`, `create_time`, `interest_rate`)\n                VALUES ");
      stringBuilder.Append(string.Join(",", (IEnumerable<string>) values));
      stringBuilder.Append(";");
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(stringBuilder.ToString());
      }
      catch (Exception ex)
      {
        LogLib.Error("[RichBoxRecordService][InsertRecords]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static Decimal GetTotalWithdraw(int member_fk)
    {
      string sql = "SELECT -IFNULL(SUM(affect),0) FROM richbox_record WHERE member_fk = @member_fk AND src = 1 AND affect < 0";
      try
      {
        using (IDbConnection connection = DapperMysql.GetConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return connection.QuerySingle<Decimal>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletRecordService][GetTotalWithdraw]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
