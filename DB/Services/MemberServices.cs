// Decompiled with JetBrains decompiler
// Type: DB.Services.MemberServices
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Member;
using tradeapi.Utility;

#nullable enable
namespace DB.Services
{
  public class MemberServices
  {
    public static MemberDto Find(int pk)
    {
      string sql = "SELECT * FROM `member` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<MemberDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    internal static MemberDto FindParent(int man)
    {
      try
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 1);
        interpolatedStringHandler.AppendLiteral("SELECT * FROM `member` WHERE pk = (SELECT m.recommend_id FROM `member` m WHERE m.pk = ");
        interpolatedStringHandler.AppendFormatted<int>(man);
        interpolatedStringHandler.AppendLiteral(" )");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<MemberDto>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][FindParent]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static MemberDto FindByIdCard(string id)
    {
      string sql = "SELECT * FROM `member` WHERE `id_card` = @id AND id_auth = 1";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            id = id
          });
          return readConnection.QueryFirstOrDefault<MemberDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][FindByIdCard]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void SetToken(int memid, string token, string ip)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "UPDATE member SET token = @token, last_login_ip = @ip, last_login_time = UTC_TIMESTAMP WHERE pk = @pk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            token = token,
            pk = memid,
            ip = ip
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][SetToken]" + ex.Message);
        throw new AppException(1030, "read_db_exception");
      }
    }

    public static void SetStockChartSetting(int member_fk, int stock_chart_setting)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "UPDATE member SET stock_chart_setting=@stock_chart_setting WHERE pk=@member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            stock_chart_setting = stock_chart_setting
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][SetStockChartSetting]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void SetRichBoxAutoTransfer(int member_fk, int enable_auto_transfer)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "UPDATE member SET enable_auto_transfer=@enable_auto_transfer WHERE pk=@member_fk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk,
            enable_auto_transfer = enable_auto_transfer
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][SetRichBoxAutoTransfer]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static string GetToken(int pk)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "select token from member where pk = @memid";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            memid = pk
          });
          return readConnection.QuerySingleOrDefault<string>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][GetToken]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void ClearToken(string token)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "update member set token = null where token = @token";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            token = token
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][ClearToken]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static double FindMemberBalance(string subAccount)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT (IFNULL(SUM(cost_purchase), 0)+mem_money) AS Total FROM trade_account\nLEFT JOIN trade_position ON trade_position.sub_account = trade_account.sub_account\nWHERE trade_account.sub_account = @account";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            account = subAccount
          });
          return readConnection.QuerySingleOrDefault<double>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][FindMemberBalance]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static bool CheckEmailExists(string email)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT COUNT(*) FROM member WHERE email = @Email AND email_status = 1 and is_del = 0";
          var data = new{ Email = email };
          return readConnection.ExecuteScalar<int>(sql, (object) data) > 0;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][CheckEmailExists]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static bool CheckUsernameExists(string account)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT COUNT(*) FROM member WHERE account = @account AND is_del = 0";
          var data = new{ account = account };
          return readConnection.ExecuteScalar<int>(sql, (object) data) > 0;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][CheckUsernameExists]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int CheckInvitationCodeExists(string invitation_code)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql1 = "SELECT COUNT(*) FROM member WHERE invitation_code = @invitation_code";
          var data1 = new
          {
            invitation_code = invitation_code
          };
          int num = readConnection.QuerySingleOrDefault<int>(sql1, (object) data1);
          if (num == 0)
          {
            string sql2 = "SELECT COUNT(*) FROM admin_user WHERE invitation_code = @invitation_code";
            var data2 = new
            {
              invitation_code = invitation_code
            };
            num = readConnection.QuerySingleOrDefault<int>(sql2, (object) data2);
          }
          return num;
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][CheckInvitationCodeExists]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static MemberResponse GetByUsernameOrEmail(string email)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT `pk`, `email`, `passwd`, `status`, `lang` FROM member \n                    WHERE (account = @Email or email = @Email) AND is_del = 0";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            Email = email
          });
          return readConnection.QuerySingleOrDefault<MemberResponse>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][GetByUsernameOrEmail]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(MemberDto memberDto)
    {
      string sql = "INSERT INTO `member` \n                (`account`, `nickname`, `email`, `passwd`, `create_time`, `create_ip`, `last_login_time`, `last_login_ip`, `invitation_code`, `email_status`, `lang`, `country`,`mobile_country`,`mobile`) \n                VALUES \n                (@account, @nickname, @email, @passwd, @create_time, @create_ip, @last_login_time, @last_login_ip, @invitation_code, @email_status, @lang, @country, @mobile_country, @mobile);\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) memberDto);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void UpdateLang(int pk, string lang)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "\n                UPDATE member \n                SET lang = @lang\n                WHERE pk = @pk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            lang = lang,
            pk = pk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][UpdateLang]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int ResetPassword(int pk, string passwd)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE `member` SET passwd=@passwd, status=1 WHERE pk=@pk", (object) new
          {
            pk = pk,
            passwd = passwd
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][ResetPassword]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int ResetPhoneNumber(int pk, string country_code, string phone_number)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE `member` SET mobile_country=@country_code, mobile=@phone_number WHERE pk=@pk", (object) new
          {
            pk = pk,
            country_code = country_code,
            phone_number = phone_number
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][ResetPhoneNumber]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int ResetPayPassword(int pk, string paypwd)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute("UPDATE `member` SET paywd=@paypwd, status=1 WHERE pk=@pk", (object) new
          {
            pk = pk,
            paypwd = paypwd
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][ResetPayPassword]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static void UploadIdentityVerification(VerifyIdentityRequest req, int member_fk)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "UPDATE member \n                                   SET \n                                   real_name = @real_name,\n                                   nickname = @real_name,\n                                   mobile_country = @mobile_country,\n                                   mobile = @mobile,\n                                   id_card_type = @id_card_type,\n                                   id_card = @id_card,\n                                   auth_time = @auth_time,\n                                   card_pic_front = @card_pic_front,\n                                   id_auth = 3\n\n                                   WHERE pk = @pk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            card_pic_front = req.img_front,
            real_name = req.name,
            mobile_country = req.mobile_country,
            mobile = req.mobile,
            id_card_type = req.id_type,
            id_card = req.id_number,
            auth_time = DateTime.UtcNow,
            pk = member_fk
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][UploadIdentityVerification]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static MemberDto GetMember(int member_pk)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM member WHERE pk = @pk";
          var data = new{ pk = member_pk };
          return readConnection.QuerySingleOrDefault<MemberDto>(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][GetMember]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void SetSubAccount(int member_pk, string sub_account)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "UPDATE member SET sub_account = @sub_account WHERE pk = @member_pk";
          var data = new
          {
            sub_account = sub_account,
            member_pk = member_pk
          };
          writeConntion.Execute(sql, (object) data);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][SetSubAccount]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static MemberDto GetMemberByInvitationCode(string invitation_code)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM `member` WHERE `invitation_code` = @invitation_code";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            invitation_code = invitation_code
          });
          return readConnection.QueryFirstOrDefault<MemberDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][GetMemberByInvitationCode]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static AdminUserDto GetAdminUserByInvitationCode(string invitation_code)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          string sql = "SELECT * FROM `admin_user` WHERE `invitation_code` = @invitation_code";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            invitation_code = invitation_code
          });
          return readConnection.QueryFirstOrDefault<AdminUserDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][GetAdminUserByInvitationCode]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateSubAccount(int pk, string subAccount)
    {
      string sql = "UPDATE `member` SET \n                `sub_account` = @subAccount\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            pk = pk,
            subAccount = subAccount
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][UpdateSubAccount]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    internal static void UpdateRecommend(int memberId, string invitation_code, int recommend)
    {
      string sql = "UPDATE `member` SET\n                `recommend` = @invitation_code,\n                `recommend_id` = @recommend\n                 WHERE `pk` = @memberId";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            invitation_code = invitation_code,
            recommend = recommend,
            memberId = memberId
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][UpdateRecommend]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    internal static void UpdateIInvitationCode(int memberId, string invitationCode)
    {
      string sql = "UPDATE `member` SET\n                `invitation_code` = @invitationCode\n                 WHERE `pk` = @memberId";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            invitationCode = invitationCode,
            memberId = memberId
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][UpdateIInvitationCode]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetReviewMemberCount()
    {
      string sql = "SELECT COUNT(*) FROM `member` where id_auth = 3 AND is_del = 0 AND is_test_account = 0";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>(sql);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][GetReviewMemberCount]" + ex.Message);
        return 0;
      }
    }

    public static Decimal GetRichBoxRate(int member_fk)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
      interpolatedStringHandler.AppendLiteral("SELECT richbox_rate FROM `member` where pk = ");
      interpolatedStringHandler.AppendFormatted<int>(member_fk);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<Decimal>(stringAndClear);
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][GetRichBoxRate]" + ex.Message);
        return 0M;
      }
    }

    public static bool IsAutoTransferEnabled(int member_fk)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
      interpolatedStringHandler.AppendLiteral("SELECT enable_auto_transfer FROM `member` where pk = ");
      interpolatedStringHandler.AppendFormatted<int>(member_fk);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>(stringAndClear) == 1;
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][IsAutoTransferEnabled]" + ex.Message);
        return false;
      }
    }

    public static void SetMemberLang(int pk, string lang)
    {
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          string sql = "UPDATE member\n                                   SET lang = @lang\n                                   WHERE pk = @pk";
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk,
            lang = lang
          });
          writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberServices][SetToken]" + ex.Message);
        throw new AppException(1030, "read_db_exception");
      }
    }

    public static int UpdateMemberServer(int pk, int admin_user_fk)
    {
      string sql = "UPDATE `member` SET \n                `admin_user_fk` = @admin_user_fk\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            pk = pk,
            admin_user_fk = admin_user_fk
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][UpdateSubAccount]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static bool FindPhoneExist(string mobile_country, string mobile)
    {
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.ExecuteScalar<int>("SELECT COUNT(1) FROM `member` WHERE `mobile_country` = @mobile_country AND `mobile` = @mobile ;", (object) new
          {
            mobile_country = mobile_country,
            mobile = mobile
          }) == 1;
      }
      catch (Exception ex)
      {
        LogLib.Error("[MemberService][ResetPayPassword]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
