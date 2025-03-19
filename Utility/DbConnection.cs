// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.DbConnection
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using MySqlConnector;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace tradeapi.Utility
{
    public sealed class DbConnection : IDbConnection, IDisposable
    {
        private readonly MySqlConnection _conn;

        public DbConnection(MySqlConnection conn) => this._conn = conn;

        public string ConnectionString
        {
            get => this._conn.ConnectionString;
            [param: AllowNull] set => this._conn.ConnectionString = value;
        }

        public int ConnectionTimeout => this._conn.ConnectionTimeout;

        public string Database => this._conn.Database;

        public ConnectionState State => this._conn.State;

        public IDbTransaction BeginTransaction() => (IDbTransaction) this._conn.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return (IDbTransaction) this._conn.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName) => this._conn.ChangeDatabase(databaseName);

        public void Close() => this._conn.Close();

        public IDbCommand CreateCommand() => (IDbCommand) this._conn.CreateCommand();

        public void Dispose() => this._conn.Close();

        public void Open() => this._conn.Open();
    }
}