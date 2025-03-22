// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.MessageServices
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Message;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class MessageServices
  {
    public static List<ListResponse> GetMessageList(int stype, int memberFk, int classify)
    {
      string sql = "\n                   select pk pk,title Title, info Content ,create_time Date ,read_status IsRead \n                   from message_record\n                   where read_status != 2 and send_type = @stype and ( ( receiver_fk = @memberFk and receiver_table =1) )\n                   AND classify = @classify\n                   ORDER BY pk desc\n                    ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            stype = stype,
            memberFk = memberFk,
            classify = classify
          });
          return readConnection.Query<ListResponse>(sql, (object) parameters).ToList<ListResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][GetMessageList]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int GetUnreadMessageCount(int stype, int memberFk)
    {
      string sql = "\n                   select count(*) from message_record\n                   where send_type = @stype and read_status = 0 and ( ( receiver_fk = @memberFk and receiver_table =1) )\n                   AND classify = 2\n                    ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            stype = stype,
            memberFk = memberFk
          });
          return readConnection.ExecuteScalar<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][GetMessageList]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<ListPublicResponse> GetListPublic(int member_fk)
    {
      string sql = "\n                   select pk pk,title Title, info Content ,create_time Date ,read_status IsRead \n                   from message_record\n                   where receiver_table= 1 AND receiver_fk = @member_fk  AND send_status = 1   \n                    AND send_type = 1    AND read_status <> 2  AND classify = 1\n                   ORDER BY pk desc\n                    ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<ListPublicResponse>(sql, (object) parameters).ToList<ListPublicResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][GetListPublic]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int GetUnreadPublicCount(int member_fk)
    {
      string sql = "\n                   select count(*)\n                   from message_record\n                   where receiver_table= 1  AND receiver_fk = @member_fk   AND send_status = 1   \n                    AND send_type = 1    AND read_status = 0 AND classify = 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.ExecuteScalar<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][GetListPublic]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static InternalMessageResponse GetInternalMessage(int pk)
    {
      string sql = "\n                   select pk pk,title Title, info Content ,create_time Date ,read_status IsRead \n                   from message_record\n                   where pk = @pk and classify = 2";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.Query<InternalMessageResponse>(sql, (object) parameters).FirstOrDefault<InternalMessageResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][GetInternalMessage]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static GetPublicResponse GetPublicMessage(int pk)
    {
      string sql = "select\n                   title Title, info Content ,create_time Date, read_status\n                   from message_record\n                   where pk =  @pk and classify = 1\n                    ";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<GetPublicResponse>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][GetInternalMessage]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void SetStatusReaded(int pk)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(167, 1);
      interpolatedStringHandler.AppendLiteral("\n                UPDATE message_record SET\n                read_status = 1\n                ,read_time = UTC_TIMESTAMP()\n                WHERE pk = ");
      interpolatedStringHandler.AppendFormatted<int>(pk);
      interpolatedStringHandler.AppendLiteral(" AND read_status = 0");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][SetStatusReaded]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void BatchSetStatusReaded(string list)
    {
      string sql = "\n                UPDATE message_record SET\n                read_status = 1\n                ,read_time = UTC_TIMESTAMP()\n                WHERE pk in " + list + " AND read_status = 0";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][BatchSetStatusReaded]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetMaxPk()
    {
      string sql = "\n                 select max(pk) pk from message_record ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
          });
          return writeConntion.Query<int>(sql, (object) parameters).FirstOrDefault<int>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][GetMaxPk]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(MessageRecordDto item)
    {
      string sql = "\n                INSERT INTO message_record (isbatch,receiver_table,receiver_fk,sender_table,sender_fk,title,info\n                ,read_status,type,send_status,send_type,create_time,classify,sent_time)\n                VALUES (@isbatch,@receiver_table, @receiver_fk, @sender_table, @sender_fk, @title, @info\n                , @read_status, @type, @send_status, @send_type,@create_time,2,@sent_time);\n                ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            isbatch = item.isbatch,
            receiver_table = item.receiver_table,
            receiver_fk = item.receiver_fk,
            sender_table = item.sender_table,
            sender_fk = item.sender_fk,
            title = item.title,
            info = item.info,
            read_status = item.read_status,
            Type = item.type,
            send_status = item.send_status,
            send_type = item.send_type,
            create_time = item.create_time,
            sent_time = item.sent_time
          });
          writeConntion.Execute(sql, (object) parameters);
        }
        return MessageServices.GetMaxPk();
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][UpdateReadStatus]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    internal static void BatchDelete(string list)
    {
      string sql = "\n                UPDATE message_record SET\n                read_status = 2\n                WHERE pk in " + list;
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          writeConntion.Execute(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MessageServices][BatchDelete]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
