// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.MessageBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.Message;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Business
{
  public class MessageBiz
  {
    public static GetListResponse GetMessageList(int id, int classify)
    {
      List<ListResponse> messageList = MessageServices.GetMessageList(1, id, classify);
      foreach (ListResponse listResponse in messageList)
      {
        if (listResponse.Content != null && listResponse.Content.Length > 20)
          listResponse.Content = listResponse.Content.Substring(0, 20) + "...";
      }
      return new GetListResponse()
      {
        list = messageList,
        total_unread_lists = MessageServices.GetUnreadMessageCount(1, id),
        total_unread_public_lists = MessageServices.GetUnreadPublicCount(id)
      };
    }

    public static GetListPublicResponse GetListPublic(int id)
    {
      List<ListPublicResponse> listPublic = MessageServices.GetListPublic(id);
      foreach (ListPublicResponse listPublicResponse in listPublic)
      {
        if (listPublicResponse.Content != null && listPublicResponse.Content.Length > 20)
          listPublicResponse.Content = listPublicResponse.Content.Substring(0, 20) + "...";
      }
      return new GetListPublicResponse()
      {
        list = listPublic,
        total_unread_lists = MessageServices.GetUnreadMessageCount(1, id),
        total_unread_public_lists = MessageServices.GetUnreadPublicCount(id)
      };
    }

    public static InternalMessageResponse GetInternalMessage(int pk)
    {
      InternalMessageResponse internalMessage = MessageServices.GetInternalMessage(pk);
      internalMessage.Content = UploadImageLib.AddHostName(internalMessage.Content);
      if (internalMessage.IsRead == 0)
        MessageServices.SetStatusReaded(pk);
      List<CmsFilesDto> cmsFilesByTableKey = CmsFilesServices.GetAllCmsFilesByTableKey("message_record", pk);
      if (cmsFilesByTableKey != null && cmsFilesByTableKey.Count > 0)
      {
        internalMessage.ImgUrls = new List<string>();
        foreach (CmsFilesDto cmsFilesDto in cmsFilesByTableKey)
          internalMessage.ImgUrls.Add(cmsFilesDto.url);
      }
      return internalMessage;
    }

    public static GetPublicResponse GetPublicMessage(int pk)
    {
      GetPublicResponse publicMessage = MessageServices.GetPublicMessage(pk);
      int? nullable = publicMessage != null ? publicMessage.read_status : throw new AppException(1024, "error_message_record_not_found");
      int num = 0;
      if (nullable.GetValueOrDefault() == num & nullable.HasValue)
        MessageServices.SetStatusReaded(pk);
      publicMessage.Content = UploadImageLib.AddHostName(publicMessage.Content);
      List<CmsFilesDto> cmsFilesByTableKey = CmsFilesServices.GetAllCmsFilesByTableKey("message_record", pk);
      if (cmsFilesByTableKey != null && cmsFilesByTableKey.Count > 0)
      {
        publicMessage.ImgUrls = new List<string>();
        foreach (CmsFilesDto cmsFilesDto in cmsFilesByTableKey)
          publicMessage.ImgUrls.Add(cmsFilesDto.url);
      }
      return publicMessage;
    }

    public static async Task<bool> SendMessage(SendRequest request, int member_pk)
    {
      DateTime utcNow = DateTime.UtcNow;
      MemberDto member = MemberServices.GetMember(member_pk);
      string str = "<p>" + request.content + "</p>";
      if (request.img_urls != null)
      {
        foreach (string imgUrl in request.img_urls)
          str = str + "<figure class=\"image\"><img src=\"#0#" + imgUrl + "\"></figure>";
      }
      MessageServices.Insert(new MessageRecordDto()
      {
        title = request.title,
        info = str,
        isbatch = false,
        sender_fk = member_pk,
        sender_table = AccountTypeEnum.Member,
        receiver_fk = member.admin_user_fk,
        receiver_table = AccountTypeEnum.AdminUser,
        read_status = MessageReadStatusEnum.Unread,
        send_status = MessageSendStatus.Draft,
        send_type = MessageTransTypeEnum.Internal,
        type = 1,
        classify = 2,
        create_time = utcNow,
        read_time = new DateTime?(),
        sent_time = utcNow
      });
      MessageBiz.UpdateMessageRecordUnread();
      return true;
    }

    public static bool DeleteInternalMessages(List<int> PkList)
    {
      if (PkList != null && PkList.Count > 0)
      {
        string str = "(";
        foreach (int pk in PkList)
          str = str + pk.ToString() + ",";
        MessageServices.BatchDelete(str.Remove(str.Length - 1, 1) + ")");
      }
      return true;
    }

    public static void Read(List<int> pk_list)
    {
      if (pk_list == null || pk_list.Count <= 0)
        return;
      string str = "(";
      foreach (int pk in pk_list)
        str = str + pk.ToString() + ",";
      MessageServices.BatchSetStatusReaded(str.Remove(str.Length - 1, 1) + ")");
    }

    public static void UpdateMessageRecordUnread()
    {
      long unreadMessages = MessageRecordService.GetUnreadMessages();
      CacheQuery.SelectDB(CacheEnum.admin);
      HomeVm redisValue = CacheQuery.StringGet<HomeVm>("HomeVm");
      redisValue.MessageRecordUnread = unreadMessages.ToString();
      CacheQuery.StringSet<HomeVm>("HomeVm", redisValue);
    }
  }
}
