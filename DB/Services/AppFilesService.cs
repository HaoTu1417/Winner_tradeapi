// Decompiled with JetBrains decompiler
// Type: DB.Services.AppFilesService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
    public class AppFilesService
    {
        public static string Find(string code)
        {
            string sql = "SELECT path FROM `app_files` WHERE `code` = @code";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                {
                    DynamicParameters parameters = DapperMysql.GetParameters((object) new
                    {
                        code = code
                    });
                    return readConnection.ExecuteScalar<string>(sql, (object) parameters);
                }
            }
            catch (Exception ex)
            {
                LogLib.Error("[AppFilesService][Find]" + ex.Message);
                return (string) null;
            }
        }

        public static string FindLatest(int device)
        {
            string sql = "SELECT path FROM `app_files` WHERE `device` = @device and upload_date = (SELECT max(upload_date) from `app_files` WHERE `device` = @device)";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                {
                    DynamicParameters parameters = DapperMysql.GetParameters((object) new
                    {
                        device = device
                    });
                    return readConnection.ExecuteScalar<string>(sql, (object) parameters);
                }
            }
            catch (Exception ex)
            {
                LogLib.Error("[AppFilesService][FindLatest]" + ex.Message);
                return (string) null;
            }
        }
    }
}