// Decompiled with JetBrains decompiler
// Type: tradeapi.Common.APIResponse`1
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace tradeapi.Common
{
    public class APIResponse<T>
    {
        public int status { get; set; }

        public string message { get; set; }

        public T data { get; set; }

        public APIResponse()
        {
            this.status = 200;
            this.message = "";
        }

        public APIResponse(T obj)
        {
            this.status = 200;
            this.message = "";
            this.data = obj;
        }

        public APIResponse(T obj, string message)
        {
            this.status = 200;
            this.message = message;
            this.data = obj;
        }

        public APIResponse(int status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public static APIResponse<T> Error(int status, string message)
        {
            return new APIResponse<T>()
            {
                status = status,
                message = message
            };
        }

        public static APIResponse<T> Ok(T obj, string message = "")
        {
            return new APIResponse<T>()
            {
                status = 200,
                message = message,
                data = obj
            };
        }
    }
}