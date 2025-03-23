// Decompiled with JetBrains decompiler
// Type: Models.UploadImagesResponse
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.Text.Json.Serialization;

#nullable enable
namespace Models
{
    public class UploadImagesResponse
    {
        [JsonPropertyName("server_url")]
        public string server_url { get; set; }

        [JsonPropertyName("img_urls")]
        public string[] img_urls { get; set; }
    }
}