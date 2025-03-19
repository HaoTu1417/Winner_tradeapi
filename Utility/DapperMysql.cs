// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.DapperMysql
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using Dapper;
using MySqlConnector;
using System.Data;
using System.Reflection;
using System.Threading;
using tradeapi.Libs;

#nullable enable
namespace tradeapi.Utility
{
    public class DapperMysql
    {
        private static string? _connectionString;
        private static readonly AsyncLocal<IDbConnection> _conn = new AsyncLocal<IDbConnection>();

        public static void Init(string connectionString)
        {
            LogLib.Debug("MySql Init " + connectionString);
            DapperMysql._connectionString = connectionString;
        }

        public static IDbConnection GetReadConnection() => DapperMysql.GetConnection();

        public static IDbConnection GetWriteConntion() => DapperMysql.GetConnection();

        public static IDbConnection GetConnection()
        {
            IDbConnection connection;
            if (DapperMysql._conn.Value == null)
            {
                connection = (IDbConnection) new DbConnection(new MySqlConnection(DapperMysql._connectionString));
                DapperMysql._conn.Value = connection;
            }
            else
                connection = DapperMysql._conn.Value;
            connection.Open();
            return connection;
        }

        public static DynamicParameters GetParameters(object obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            foreach (PropertyInfo property in obj.GetType().GetProperties())
                parameters.Add("@" + ((MemberInfo) property).Name, property.GetValue(obj));
            parameters.RemoveUnused = true;
            return parameters;
        }
    }
}