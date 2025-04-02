using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tradeapi.Models.Wallet;
using tradeApi2.Models.JYPay;
using tradeApi2.Utility;

namespace tradeapi.Business
{
    public class WalletJYPayBiz : WalletBiz
    {
        private   HttpClient _httpClient;
        
        private static readonly string _baseUrl= "https://shapi.jypay666.top";
        public static readonly string _key = "cKhXSSMGp6EDKbI7IkFLOTnrIp6OxXp1Ex9aU2yyetP2594LD6UnhdUw2UUDUiAn";
        private static string getUrl(string url)=> $"{_baseUrl}/{url}";

        public WalletJYPayBiz(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string tesstSign = "";
        public  async Task<(string?,bool)> GetPaymentUrlAsync(JYPayAddRequest request)
        {   
            try
            {
                string fullUrl = getUrl("v1/dsapi/add2");
                string formContent = ToFormUrlEncoded(request);

                var content = new StringContent(formContent, Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                response.EnsureSuccessStatusCode(); // Throws if not 2xx

                string responseContent = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseContent);
                
                JsonElement root = doc.RootElement;

                // Giả định response có dạng: { "success": true, "url": "https://payment.url" }
                bool isSuccess = root.GetProperty("status").GetString().ToLower().Equals("success");
                if (isSuccess)
                {
                    string url = root.GetProperty("pay_url").GetString()!;
                    return (url,isSuccess);
                }
                //TODO: neeus vi ly do gi do that bai thi return loi=> bao user vaf ghi log
                string error = root.GetProperty("msg").GetString()!;
                return (error,isSuccess);
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần
                return (ex.GetBaseException().ToString(),false);
            }
        }
        private  string ToFormUrlEncoded(JYPayAddRequest request)
        {
            var keyValues = new Dictionary<string, string>
            {
                // { "attach", request.attach },
                { "applydate", request.applydate },
                // { "client_ip", request.client_ip },
                { "code", request.code },
                { "mchid", request.mchid },
                { "money", request.money.ToString("F2").Replace(",",".") }, // format float to 2 decimal places
                { "notifyurl", request.notifyurl },
                { "out_trade_no", request.out_trade_no },
                // { "productname", request.productname },
                // { "returnurl", request.returnurl },
                { "sign", GetSign(request) },
                // { "submitname", request.submitname },
              
            };

            // var sb = new StringBuilder();
            // foreach (var kv in keyValues)
            // {
            //     if (!string.IsNullOrWhiteSpace(kv.Value))
            //     {
            //         if (sb.Length > 0)
            //             sb.Append('&');
            //
            //         sb.Append(WebUtility.UrlEncode(kv.Key));
            //         sb.Append('=');
            //         sb.Append(WebUtility.UrlEncode(kv.Value));
            //     }
            // }
            StringBuilder sb = new StringBuilder();
            sb.Append($"applydate={request.applydate}&");
            sb.Append($"code={request.code}&");
            sb.Append($"mchid=6126&");
            sb.Append($"money={request.money.ToString("F2").Replace(",",".") }&");
            sb.Append($"notifyurl={request.notifyurl}&");
            sb.Append($"out_trade_no={request.out_trade_no}&");
            sb.Append($"sign={GetSign(request)}");

            return sb.ToString();
        }
        
        
        /*
         *  { "attach", request.attach },
                          { "applydate", request.applydate },
                          { "client_ip", request.client_ip },
                          { "code", request.code },
                          { "mchid", request.mchid },
                          { "money", request.money.ToString("F2").Replace(",",".") }, // format float to 2 decimal places
                          { "notifyurl", request.notifyurl },
                          { "out_trade_no", request.out_trade_no },
                          { "productname", request.productname },
                          { "returnurl", request.returnurl },
                          { "sign", GetSign(request) },
                          { "submitname", request.submitname },
         */
        
        private  string GetSign(JYPayAddRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"applydate={request.applydate}&");
            sb.Append($"code={request.code}&");
            sb.Append($"mchid=6126&");
            sb.Append($"money={request.money.ToString("F2").Replace(",",".") }&");
            sb.Append($"notifyurl={request.notifyurl}&");
            sb.Append($"out_trade_no={request.out_trade_no}&");
            sb.Append($"key={WalletJYPayBiz._key}");
            string result = HashHelper.ToMd5String(sb.ToString()).ToUpper();
            tesstSign = sb.ToString();
            return result;
        }

        private  string GetOutTradeNo()
        {
            return $"ORD{DateTime.Now.Ticks}";
        }
    }
}