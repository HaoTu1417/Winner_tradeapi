// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.CmsSupportService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Info;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
    public class CmsSupportService
    {
        public static ServiceResponse GetService(string lang)
        {
            string sql = "SELECT * FROM `cms_support` WHERE lang = @lang";
            var data = new{ lang = lang };
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                    return readConnection.QueryFirstOrDefault<ServiceResponse>(sql, (object) data);
            }
            catch (Exception ex)
            {
                LogLib.Error("[CmsSupportService][GetService]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}