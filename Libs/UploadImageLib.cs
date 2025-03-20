// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.UploadImageLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Http;
using Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using tradeapi.Common;
using tradeapi.Models.Dto;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Libs
{
  public class UploadImageLib
  {
    public static async Task<List<int>> UploadImages(
      FileManagementLib.Folder img_folder,
      string table_name,
      int pk,
      bool change_name,
      List<IFormFile> images)
    {
      List<int> intList1;
      try
      {
        RestClient client = new RestClient(ConfigLib.Get("fileserver") + "/api/");
        RestRequest request = new RestRequest("ApiImg/uploadimages");
        request.AddParameter(nameof (img_folder), img_folder.ToString());
        request.AddParameter<bool>(nameof (change_name), change_name);
        foreach (IFormFile image in images)
        {
          MemoryStream target = new MemoryStream();
          image.CopyTo((Stream) target);
          request.AddFile(image.Name, target.ToArray(), image.FileName, (ContentType) image.ContentType);
        }
        APIResponse<UploadImagesResponse> apiResponse = await client.PostAsync<APIResponse<UploadImagesResponse>>(request);
        List<int> intList2 = new List<int>();
        if (apiResponse != null)
        {
          foreach (string imgUrl in apiResponse.data.img_urls)
            intList2.Add(CmsFilesServices.InsertCmsFiles(new CmsFilesDto()
            {
              url = imgUrl,
              file_type = 1,
              table = table_name,
              key = pk
            }));
        }
        intList1 = intList2;
      }
      catch (Exception ex)
      {
        throw new AppException(2100, "upload_service_exception");
      }
      return intList1;
    }

    public static string AddHostName(string content)
    {
      try
      {
        string newValue = ConfigLib.Get("filesite");
        return content.Replace("#0#", newValue);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw new AppException(2100, "upload_service_exception");
      }
    }
  }
}
