// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.InfoBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.Info;
using tradeapi.Services;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
  public class InfoBiz
  {
    public static List<QuestionCatalogResponse> GetQuestionCatalog(string lang)
    {
      return CmsQuestionCategoryService.GetQuestionCatalog(lang);
    }

    public static List<PromotionListResponse> GetPromotionList(string lang)
    {
      return CmsPromotionService.GetPromotionList(lang);
    }

    public static PromotionContentResponse GetPromotionContent(int pk)
    {
      PromotionContentResponse promotionContent = CmsPromotionService.GetPromotionContent(pk);
      promotionContent.topic_content = UploadImageLib.AddHostName(promotionContent.topic_content);
      return promotionContent;
    }

    public static List<QuestionListResponse> GetQuestionList(string lang, int catalog_pk)
    {
      return CmsQuestionService.GetQuestionList(lang, catalog_pk);
    }

    public static AnswerResponse GetAnswer(string lang, int question_pk)
    {
      AnswerResponse questionAnswer = CmsQuestionService.GetQuestionAnswer(lang, question_pk);
      questionAnswer.answer = UploadImageLib.AddHostName(questionAnswer.answer);
      return questionAnswer;
    }

    public static ExchangeRateResponse ExchangeRate(string target_currency, string base_currency)
    {
      return new ExchangeRateResponse()
      {
        exchange = target_currency + "-" + base_currency,
        exchange_rate = ExchangeHelper.GetRate(target_currency, base_currency)
      };
    }

    public static List<List<ExchangeResponse>> Exchange()
    {
      List<List<ExchangeResponse>> rates = new List<List<ExchangeResponse>>();
      DateTime lastDate = MoneyDailyExchangeService.GetLastDate();
      Dictionary<string, SysCountryDto> countries = SysCountryService.FindAll().Where<SysCountryDto>((Func<SysCountryDto, bool>) (x => x.pk != "OTHER")).ToDictionary<SysCountryDto, string>((Func<SysCountryDto, string>) (x => x.currency));
      countries.Add("KVND", countries["VND"]);
      MoneyDailyExchangeService.Find("USD", lastDate).ForEach((Action<MoneyDailyExchangeDto>) (x =>
      {
        if (!countries.ContainsKey(x.currency_symbol) || !countries.ContainsKey(x.base_symbol))
          return;
        rates.Add(new List<ExchangeResponse>()
        {
          new ExchangeResponse()
          {
            base_currency = x.currency_symbol,
            quote_currency = x.base_symbol,
            base_currency_flag = countries[x.currency_symbol].flag,
            quote_currency_flag = countries[x.base_symbol].flag,
            exchange_rate = x.inward_rate
          },
          new ExchangeResponse()
          {
            base_currency = x.base_symbol,
            quote_currency = x.currency_symbol,
            base_currency_flag = countries[x.base_symbol].flag,
            quote_currency_flag = countries[x.currency_symbol].flag,
            exchange_rate = x.outward_rate
          }
        });
      }));
      return rates;
    }

    public static List<BulletinResponse> Bulletin(string lang)
    {
      List<BulletinResponse> bulletin = CmsBulletinService.GetBulletin(lang);
      foreach (BulletinResponse bulletinResponse in bulletin)
        bulletinResponse.summary = UploadImageLib.AddHostName(bulletinResponse.summary);
      return bulletin;
    }

    public static BulletinContentResponse BulletinContent(int pk)
    {
      BulletinContentResponse bulletinContent = CmsBulletinService.GetBulletinContent(pk);
      bulletinContent.topic_content = UploadImageLib.AddHostName(bulletinContent.topic_content);
      return bulletinContent;
    }

    public static DocResponse Doc(DocRequest req)
    {
      DocResponse doc = CmsDocumentService.GetDoc(req.id, req.lang);
      doc.content = UploadImageLib.AddHostName(doc.content);
      return doc;
    }

    public static DocResponse GetDocByCid(string cid, string lang)
    {
      DocResponse docByCid = new DocResponse();
      CmsDocumentDto cmsDocumentDto = CmsDocumentService.GetDocByCid(cid, lang) ?? CmsDocumentService.GetDocByCid(cid, "EN");
      if (cmsDocumentDto != null)
      {
        CmsDocumentService.UpdateDocView(cmsDocumentDto.pk);
        docByCid.title = cmsDocumentDto.title;
        docByCid.content = cmsDocumentDto.content;
        docByCid.view = cmsDocumentDto.view;
        docByCid.trash = cmsDocumentDto.trash ? 1 : 0;
        docByCid.status = new int?(cmsDocumentDto.status ? 1 : 0);
      }
      return docByCid;
    }

    public static ServiceResponse Service(string lang) => CmsSupportService.GetService(lang);

    public static List<ServiceQuestioResponse> GetServiceQuestion(string lang, int catalog_pk)
    {
      return CmsQuestionService.GetServiceQuestionList(lang, catalog_pk);
    }

    public static ServiceAnswerResponse GetServiceAnswer(string lang, int question_pk)
    {
      ServiceAnswerResponse serviceAnswer = CmsQuestionService.GetServiceAnswer(lang, question_pk);
      serviceAnswer.answer = UploadImageLib.AddHostName(serviceAnswer.answer);
      return serviceAnswer;
    }

    public static AdResponse GetAd(string lang)
    {
      List<CmsBannerDto> byLang = CmsBannerService.FindByLang(lang);
      List<string> all = CmsMarqService.FindAll(lang);
      CmsPopinfoDto cmsPopinfoDto = CmsPopinfoService.Find(lang);
      return new AdResponse()
      {
        bnr = byLang,
        marq = all,
        pop_msg = cmsPopinfoDto?.info,
        show_pop = cmsPopinfoDto?.enable
      };
    }

    public static AdvertiseResponse GetAdvertise(string lang)
    {
      CmsAdvertiseDto byLang = CmsAdvertiseService.FindByLang(lang);
      return new AdvertiseResponse() { ad = byLang };
    }
  }
}
