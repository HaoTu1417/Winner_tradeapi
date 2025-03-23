// Decompiled with JetBrains decompiler
// Type: tradeapi.Common.APIResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi2.Common
{
    public class APIResponse
    {
        public int status { get; set; }

        public string message { get; set; }

        public object? data { get; set; }

        public static APIResponse Error(int status, string message)
        {
            return new APIResponse()
            {
                status = status,
                message = message
            };
        }

        public static APIResponse Ok(object? obj, string message = "")
        {
            return new APIResponse()
            {
                status = 200,
                message = message,
                data = obj
            };
        }
    }
}