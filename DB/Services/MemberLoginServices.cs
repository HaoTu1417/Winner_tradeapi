// Decompiled with JetBrains decompiler
// Type: DB.Services.MemberLoginServices
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
    public class MemberLoginServices
    {
        public static MemberLoginDto Find(int member_fk)
        {
            string sql = "SELECT * FROM `member_login` WHERE `member_fk` = @member_fk";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                {
                    DynamicParameters parameters = DapperMysql.GetParameters((object) new
                    {
                        member_fk = member_fk
                    });
                    return readConnection.QueryFirstOrDefault<MemberLoginDto>(sql, (object) parameters);
                }
            }
            catch (Exception ex)
            {
                LogLib.Error("[MemberLoginServices][Find]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }

        public static int FindPkAfterInsert(MemberLoginDto member_login)
        {
            string sql = "INSERT INTO `member_login`\n                           (member_fk, ip, ip_country, login_account, device, create_time, status, remark)\n                           VALUES (@member_fk, @ip, @ip_country, @login_account, @device, @create_time, @status, @remark);\n                           select @@IDENTITY;";
            try
            {
                using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
                    return writeConntion.ExecuteScalar<int>(sql, (object) member_login);
            }
            catch (Exception ex)
            {
                LogLib.Error("[MemberLoginServices][FindPkAfterInsert]" + ex.Message);
                throw new AppException(1030, "write_db_exception");
            }
        }

        public static List<MemberLoginDto> GetLoginByDay(DateTime date, long memberFk)
        {
            string sql = @"SELECT * FROM `member_login` 
                   WHERE DATE(`create_time`) = DATE(@date) 
                   AND `member_fk` = @member_fk";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("date", date);
                    parameters.Add("member_fk", memberFk);

                    return readConnection.Query<MemberLoginDto>(sql, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                LogLib.Error("[MemberLoginServices][GetLoginByDay] " + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }


    }
}