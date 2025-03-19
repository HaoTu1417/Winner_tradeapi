// Decompiled with JetBrains decompiler
// Type: tradeapi.Common.AppException
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using System;

#nullable enable
namespace tradeapi.Common
{
    public class AppException : Exception
    {
        private int _status;
        private string _messageKey;

        public AppException(string message)
            : base(message)
        {
            this._messageKey = message;
        }

        public AppException(int errorCode, string message)
            : base(message)
        {
            this._status = errorCode;
            this._messageKey = message;
        }

        public int GetStatus() => this._status;

        public string GetMessage(string language)
        {
            return "[" + this._status.ToString() + "]" + TranslatorService.ConvertByKey(language, this._messageKey);
        }
    }
}