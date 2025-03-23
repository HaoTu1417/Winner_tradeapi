// Decompiled with JetBrains decompiler
// Type: DB.Services.MessageRecordService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class MessageRecordService
  {
    public static int FindPkAfterInsert(MessageRecordDto source)
    {
      string sql = "INSERT INTO `message_record` (\n                `isbatch`, `receiver_table`, `receiver_fk`, `sender_table`, `sender_fk`, `title`, `info`, `read_status`, `type`, `send_status`, `send_type`, `create_time`, `read_time`, `sent_time`, `classify`)\n                VALUES (@isbatch, @receiver_table, @receiver_fk, @sender_table, @sender_fk, @title, @info, @read_status, @type, @send_status, @send_type, @create_time, @read_time, @sent_time, @classify);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageRecordService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static long GetUnreadMessages()
    {
      try
      {
        string sql = "\n                    SELECT COUNT(*) FROM `message_record`\n                    INNER JOIN member ON member.pk = message_record.sender_fk\n                    WHERE classify = 2 AND receiver_table = 2 AND read_status = 0";
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<long>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageRecordService][GetUnreadMessages]" + ex.Message);
        return 0;
      }
    }
  }
}
