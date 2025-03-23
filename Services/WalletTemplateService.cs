// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WalletTemplateService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
    public class WalletTemplateService
    {
        public static WalletTemplateDto GetByTempId(int temp_id, string lang)
        {
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                {
                    string sql = "SELECT * FROM wallet_template WHERE temp_Id = @temp_Id and Lang = @Lang;";
                    var data = new{ temp_Id = temp_id, Lang = lang };
                    return readConnection.QuerySingleOrDefault<WalletTemplateDto>(sql, (object) data);
                }
            }
            catch (Exception ex)
            {
                LogLib.Error("[WalletTemplateService][GetByTempId]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}