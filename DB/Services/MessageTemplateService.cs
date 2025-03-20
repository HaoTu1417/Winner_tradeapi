// Decompiled with JetBrains decompiler
// Type: DB.Services.MessageTemplateService
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
    public class MessageTemplateService
    {
        public static MessageTemplateDto Find(int pk)
        {
            string sql = "SELECT * FROM `message_template` WHERE `pk` = @pk";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                {
                    DynamicParameters parameters = DapperMysql.GetParameters((object) new
                    {
                        pk = pk
                    });
                    return readConnection.QueryFirstOrDefault<MessageTemplateDto>(sql, (object) parameters);
                }
            }
            catch (Exception ex)
            {
                LogLib.Error("[MessageTemplateDto][Find]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }

        public static MessageTemplateDto FindByTemplateId(int temp_id, string lang)
        {
            string sql = "SELECT * FROM message_template WHERE temp_id = @temp_id AND lang = @lang ";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                    return readConnection.QuerySingleOrDefault<MessageTemplateDto>(sql, (object) new
                    {
                        temp_id = temp_id,
                        lang = lang
                    });
            }
            catch (Exception ex)
            {
                LogLib.Error("[MessageTemplateDto][FindByTemplateId]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}