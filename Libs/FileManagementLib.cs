// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.FileManagementLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models;
using RestSharp;
using System;
using System.Threading.Tasks;
using tradeapi.Common;
using tradeapi.Models;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Libs
{
  public class FileManagementLib
  {
    public static async Task<GetFilesResponse> GetFilesInDirectory(FileManagementLib.Folder dir_name)
    {
      GetFilesResponse filesInDirectory;
      try
      {
        RestClient client = new RestClient(ConfigLib.Get("fileserver") + "/api/");
        RestRequest request = new RestRequest("ApiFile/getfilesindirectory");
        request.AddParameter(nameof (dir_name), dir_name.ToString());
        APIResponse<GetFilesResponse> apiResponse = await client.PostAsync<APIResponse<GetFilesResponse>>(request);
        filesInDirectory = apiResponse != null ? apiResponse.data : throw new AppException(apiResponse.status, apiResponse.message);
      }
      catch (Exception ex)
      {
        throw new AppException(2100, "upload_service_exception");
      }
      return filesInDirectory;
    }

    public static async Task<GetFilesResponse> GetAllFiles()
    {
      GetFilesResponse allFiles;
      try
      {
        APIResponse<GetFilesResponse> apiResponse = await new RestClient(ConfigLib.Get("fileserver") + "/api/").PostAsync<APIResponse<GetFilesResponse>>(new RestRequest("ApiFile/getallfiles"));
        allFiles = apiResponse != null ? apiResponse.data : throw new AppException(apiResponse.status, apiResponse.message);
      }
      catch (Exception ex)
      {
        throw new AppException(2100, "upload_service_exception");
      }
      return allFiles;
    }

    public static async Task CreateDirectory(FileManagementLib.Folder dir_name)
    {
      try
      {
        RestClient client = new RestClient(ConfigLib.Get("fileserver") + "/api/");
        RestRequest request = new RestRequest("ApiFile/createdirectory");
        request.AddParameter(nameof (dir_name), dir_name.ToString());
        APIResponse apiResponse = await client.PostAsync<APIResponse>(request);
        if ((apiResponse != null ? (apiResponse.status != 200 ? 1 : 0) : 1) != 0)
          throw new AppException(apiResponse.status, apiResponse.message);
      }
      catch (Exception ex)
      {
        throw new AppException(2100, "upload_service_exception");
      }
    }

    public static async Task DeleteFile(FileManagementLib.Folder dir_name, string file_name)
    {
      try
      {
        RestClient client = new RestClient(ConfigLib.Get("fileserver") + "/api/");
        RestRequest request = new RestRequest("ApiFile/deletefile");
        request.AddParameter(nameof (dir_name), dir_name.ToString());
        request.AddParameter(nameof (file_name), file_name);
        APIResponse apiResponse = await client.PostAsync<APIResponse>(request);
        if ((apiResponse != null ? (apiResponse.status != 200 ? 1 : 0) : 1) != 0)
          throw new AppException(apiResponse.status, apiResponse.message);
      }
      catch (Exception ex)
      {
        throw new AppException(2100, "upload_service_exception");
      }
    }

    public static DownloadAppResponse GetDownloadUrl(int device)
    {
      try
      {
        string str1 = ConfigLib.Get("app_download_host_name");
        string str2 = str1 + "/" + AppFilesService.FindLatest(0);
        string str3 = str1 + "/" + AppFilesService.FindLatest(1);
        string str4 = str1 + "/" + AppFilesService.FindLatest(device);
        return new DownloadAppResponse()
        {
          ios = str2,
          android = str3,
          qrcode = str4
        };
      }
      catch (Exception ex)
      {
        LogLib.Log("[FileManagementLib][GetDownloadUrl]" + ex.Message);
        throw new AppException(2100, "upload_service_exception");
      }
    }

    public enum Folder
    {
      flag = 1,
      id = 2,
      banner = 3,
      article = 4,
      message = 5,
      bank_book = 6,
    }
  }
}
