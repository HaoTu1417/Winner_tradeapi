// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.LogLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using NLog;
using System;

#nullable enable
namespace tradeapi.Libs
{
    public class LogLib
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Log(string message) => LogLib._logger.Info(message);

        public static bool IsDebugEnabled => LogLib._logger.IsDebugEnabled;

        public static bool IsInfoEnabled => LogLib._logger.IsInfoEnabled;

        public static bool IsWarnEnabled => LogLib._logger.IsWarnEnabled;

        public static bool IsErrorEnabled => LogLib._logger.IsErrorEnabled;

        public static bool IsFatalEnabled => LogLib._logger.IsFatalEnabled;

        public static void Debug(string message) => LogLib._logger.Debug(message);

        public static void Info(string message) => LogLib._logger.Info(message);

        public static void Warn(string message) => LogLib._logger.Warn(message);

        public static void Error(string message) => LogLib._logger.Error(message);

        public static void Error(Exception exception) => LogLib._logger.Error<Exception>(exception);

        public static void Error(string message, Exception ex) => LogLib._logger.Error(ex, message);

        public static void Fatal(string message, Exception ex) => LogLib._logger.Fatal(ex, message);
    }
}