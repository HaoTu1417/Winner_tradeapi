// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.VerityService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Member;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class VerityService
  {
    public VerifyResponse GetByEmail(string email)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM verify WHERE email = @Email;";
          var data = new{ Email = email };
          return readConnection.QuerySingleOrDefault<VerifyResponse>(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[VerityService][GetByEmail]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public void Insert(VerifyResponse verifyResponse)
    {
      string sql = "\n        INSERT INTO `verify` \n        (`code`, `send_time`, `type`, `email`) \n        VALUES \n        (@code, @send_time, @type, @email)";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql, (object) verifyResponse);
      }
      catch (Exception ex)
      {
        LogLib.Error("[VerityService][Insert]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public void Update(VerifyResponse verifyResponse)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE verify \n                SET send_time = @send_time, code = @code \n                WHERE email = @email";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            send_time = verifyResponse.send_time,
            code = verifyResponse.code,
            email = verifyResponse.email
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[VerityService][Update]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public int Delete(string email)
    {
      string sql = "DELETE from verify WHERE email = @Email";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          var data = new{ Email = email };
          return writeConntion.Execute(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[VerityService][Delete]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
