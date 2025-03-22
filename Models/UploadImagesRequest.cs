// Decompiled with JetBrains decompiler
// Type: Models.UploadImagesRequest
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

#nullable enable
namespace Models
{
    public class UploadImagesRequest
    {
        public string img_folder { get; set; }

        public string table_name { get; set; }

        public int pk { get; set; }

        public bool change_name { get; set; }

        public List<IFormFile> images { get; set; }
    }
}