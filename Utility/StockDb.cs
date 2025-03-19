// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.StockDb
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using MySqlConnector;
using System.Data;
using System.Threading;
using tradeapi.Libs;

#nullable enable
namespace tradeapi.Utility
{
    public static class StockDb
    {
        private static string? _connectionString;
        private static readonly AsyncLocal<IDbConnection> _conn = new AsyncLocal<IDbConnection>();

        public static void Init(string connectionString)
        {
            LogLib.Debug("StockDb Init " + connectionString);
            StockDb._connectionString = connectionString;
        }

        public static IDbConnection GetReadConnection() => StockDb.GetConnection();

        public static IDbConnection GetWriteConntion() => StockDb.GetConnection();

        public static IDbConnection GetConnection()
        {
            IDbConnection connection;
            if (StockDb._conn.Value == null)
            {
                connection = (IDbConnection) new DbConnection(new MySqlConnection(StockDb._connectionString));
                StockDb._conn.Value = connection;
            }
            else
                connection = StockDb._conn.Value;
            connection.Open();
            return connection;
        }
    }
}