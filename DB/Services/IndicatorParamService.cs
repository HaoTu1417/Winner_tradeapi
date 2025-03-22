// Decompiled with JetBrains decompiler
// Type: DB.Services.IndicatorParamService
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
  public class IndicatorParamService
  {
    public static IndicatorParamDto Find(int member_fk, string name)
    {
      string sql = "\nSELECT `member_fk`, `pk`, `name`, `param1`, `param2`, `param3`, `param4`, `param5`\nFROM `indicator_param`\nWHERE `member_fk` = @member_fk\nAND `name` = @name\n;";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            name = name
          });
          return readConnection.QueryFirstOrDefault<IndicatorParamDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[IndicatorParamService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<IndicatorParamDto> FindAll(int member_fk)
    {
      string sql = "\nSELECT `member_fk`, `pk`, `name`, `param1`, `param2`, `param3`, `param4`, `param5`\nFROM `indicator_param`\nWHERE `member_fk` = @member_fk\n;";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<IndicatorParamDto>(sql, (object) parameters).AsList<IndicatorParamDto>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[IndicatorParamService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static uint Insert(IndicatorParamDto model)
    {
      string sql = "\nINSERT INTO `indicator_param`\n(`member_fk`, `name`, `param1`, `param2`, `param3`, `param4`, `param5`)\nVALUES\n(@member_fk, @name, @param1, @param2, @param3, @param4, @param5);\n\nSELECT LAST_INSERT_ID();\n";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<uint>(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[IndicatorParamService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Update(IndicatorParamDto model)
    {
      string sql = "\nUPDATE `indicator_param`\nSET `param1` = @param1\n,`param2` = @param2\n,`param3` = @param3\n,`param4` = @param4\n,`param5` = @param5\nWHERE `member_fk` = @member_fk \nAND `name` = @name\n;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[IndicatorParamService][Update]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
