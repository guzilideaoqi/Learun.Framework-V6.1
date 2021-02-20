using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using HYG.CommonHelper.ShoppingAPI;
using JDModel;
using HYG.CommonHelper.PDDModel;
using System.Web;
using Top.Api.Request;
using Top.Api;
using Top.Api.Response;
using System.Data;
using Newtonsoft.Json;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;
using Learun.Application.TwoDevelopment.Common;
using Hyg.Common.DTKTools.DTKResponse;
using Hyg.Common.DTKTools.DTKRequest;
using Hyg.Common.DTKTools.DTKModel;
using Hyg.Common.DTKTools;
using Hyg.Common.Model;
using Hyg.Common.PDDTools;
using Hyg.Common.PDDTools.PDDRequest;
using Hyg.Common.PDDTools.PDDResponse;
using Hyg.Common.JDTools;
using Hyg.Common.JDTools.JDRequest;
using Hyg.Common.JDTools.JDResponse;
using HYG.CommonHelper.JDModel;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_GoodController : MvcAPIControllerBase
    {
        private DM_IntergralChangeGoodIBLL dM_IntergralChangeGoodIBLL = new DM_IntergralChangeGoodBLL();

        private DM_IntergralChangeRecordIBLL dM_IntergralChangeRecordIBLL = new DM_IntergralChangeRecordBLL();

        private DM_UserIBLL dm_userIBLL = new DM_UserBLL();

        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

        private DM_PidIBLL dM_PidIBLL = new DM_PidBLL();

        private ICache redisCache = CacheFactory.CaChe();

        string tb_url = "http://gw.api.taobao.com/router/rest";

        #region 获取商品类别(一个月更新一次)
        public ActionResult GetGoodType()
        {
            try
            {
                string appid = CheckAPPID();
                List<CategoryItem> categoryItems = GoodTypeByCache(appid);
                return SuccessList("获取成功", categoryItems);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        public ActionResult GetGoodTypeByCache()
        {
            try
            {
                string cacheKey = "SuperCategory";
                List<CategoryItem> categoryItems = redisCache.Read<List<CategoryItem>>(cacheKey, 7L);
                return Success("获取成功", categoryItems);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取商品二级分类
        public ActionResult GetSubGoodType(int cid = 6)
        {
            try
            {
                string appid = CheckAPPID();
                List<CategoryItem> categoryItems = GoodTypeByCache(appid);
                CategoryItem categoryItem = categoryItems.Where(t => t.cid == cid).FirstOrDefault();
                if (categoryItem == null) throw new Exception("分类id错误!");
                return SuccessList("获取成功", categoryItem.subcategories);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 商品分类处理
        List<CategoryItem> GoodTypeByCache(string appid)
        {
            try
            {
                string cacheKey = "SuperCategory";
                List<CategoryItem> categoryItems = redisCache.Read<List<CategoryItem>>(cacheKey, 7L);
                if (categoryItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Super_CategoryRequest dTK_Super_CategoryRequest = new DTK_Super_CategoryRequest();
                    DTK_Super_CategoryResponse dTK_Super_CategoryResponse = dTK_ApiManage.GetSuperCategory(dTK_Super_CategoryRequest);
                    if (dTK_Super_CategoryResponse.code != 0)
                    {
                        throw new Exception(dTK_Super_CategoryResponse.msg);
                    }
                    categoryItems = dTK_Super_CategoryResponse.data;
                    redisCache.Write(cacheKey, categoryItems, DateTime.Now.AddMonths(1), 7L);
                }
                return categoryItems;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 热搜记录(每天更新一次)
        public ActionResult GetTop100()
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = "Top100";
                List<string> top100List = redisCache.Read<List<string>>(cacheKey, 7L);
                if (top100List == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Top100_Request dTK_Top100_Request = new DTK_Top100_Request();
                    DTK_Top100_Response dTK_Top100_Response = dTK_ApiManage.GetTop100(dTK_Top100_Request);
                    if (dTK_Top100_Response.code != 0)
                    {
                        //return Fail(dTK_Top100_Response.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    top100List = dTK_Top100_Response.data.hotWords;
                    redisCache.Write(cacheKey, top100List, DateTime.Now.AddDays(1.0), 7L);
                }
                return SuccessList("获取成功", top100List);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取猜你喜欢
        public ActionResult GetGuessGood(int PageNo)
        {
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(base.Request.Headers);

                string imei = base.Request.Headers["IMEI_Code"];
                string platform = base.Request.Headers["platform"];

                string cacheKey = Md5Helper.Hash(imei + PageNo);
                List<CommonGoodInfoEntity> commonGoodInfoEntities = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7);
                if (commonGoodInfoEntities.IsEmpty())
                {
                    ITopClient client = new DefaultTopClient(CommonConfig.tb_api_url, "24625691", "9f852d35d2dc24028be48f99bf9bcf8a");
                    TbkDgOptimusMaterialRequest req = new TbkDgOptimusMaterialRequest();
                    req.PageSize = 10L;
                    req.AdzoneId = 141890944;
                    req.PageNo = PageNo;
                    req.MaterialId = 6708;
                    //req.DeviceValue = "ab307116f4d176b32a41df086c216cd7";//我的
                    if (platform.ToUpper() == "IOS")
                    {
                        if (!imei.IsEmpty())
                            req.DeviceValue = Md5Helper.Encrypt(imei, 32);//苹果
                        req.DeviceEncrypt = "MD5";
                        req.DeviceType = "IDFA";
                    }
                    else
                    {
                        req.DeviceValue = imei;//安卓的
                        req.DeviceType = "UTDID";
                    }
                    //req.ContentId = 323L;
                    //req.ContentSource = "xxx";
                    //req.ItemId = 33243L;
                    //req.FavoritesId = "123445";
                    TbkDgOptimusMaterialResponse rsp = client.Execute(req);

                    if (rsp.IsError)
                    {
                        throw new Exception(rsp.SubErrMsg);
                    }
                    else
                    {
                        commonGoodInfoEntities = GuessGoodConvert(rsp.ResultList, dm_UserEntity, dm_BasesettingEntity, cacheKey);

                        if (commonGoodInfoEntities.Count > 0)
                            redisCache.Write<List<CommonGoodInfoEntity>>(cacheKey, commonGoodInfoEntities, DateTime.Now.AddHours(2), 7);
                        else
                            return Fail(CommonConfig.NoDataTip);
                    }
                }

                return SuccessList("获取成功", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, commonGoodInfoEntities));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 各大榜单(4个小时更新一次)
        /// <summary>
        /// 各大榜单
        /// </summary>
        /// <param name="cateid">仅对实时榜单、全天榜单有效</param>
        /// <param name="RandType">1.实时榜 2.全天榜 3.热推榜 4.复购榜 5.热词飙升榜 6.热词排行榜 7.综合热搜榜</param>
        /// <returns></returns>
        public ActionResult GetRankingList(int User_ID, int cateid = 1, int RandType = 1, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                string appid = CheckAPPID();
                string imei = base.Request.Headers["IMEI_Code"];
                string platform = base.Request.Headers["platform"];

                string cacheKey = "RankingList" + RandType.ToString() + PageNo + cateid + PageSize + imei + platform;
                List<CommonGoodInfoEntity> RankingList = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);

                if (RankingList.IsEmpty())
                {
                    //猜你喜欢
                    if (RandType == 3)
                    {
                        ITopClient client = new DefaultTopClient(CommonConfig.tb_api_url, "24625691", "9f852d35d2dc24028be48f99bf9bcf8a");
                        TbkDgOptimusMaterialRequest req = new TbkDgOptimusMaterialRequest();
                        req.PageSize = PageSize;
                        req.AdzoneId = 141890944;
                        req.PageNo = PageNo;
                        req.MaterialId = 6708;
                        //req.DeviceValue = "ab307116f4d176b32a41df086c216cd7";//我的
                        if (platform.ToUpper() == "IOS")
                        {
                            if (!imei.IsEmpty())
                                req.DeviceValue = Md5Helper.Encrypt(imei, 32);//苹果
                            req.DeviceEncrypt = "MD5";
                            req.DeviceType = "IDFA";
                        }
                        else
                        {
                            req.DeviceValue = imei;//安卓的
                            req.DeviceType = "UTDID";
                        }


                        TbkDgOptimusMaterialResponse rsp = client.Execute(req);

                        if (rsp.IsError)
                        {
                            throw new Exception(rsp.SubErrMsg);
                        }
                        else
                        {
                            RankingList = GuessGoodConvert(rsp.ResultList, dm_UserEntity, dm_BasesettingEntity, cacheKey);

                            if (RankingList.Count > 0)
                                redisCache.Write<List<CommonGoodInfoEntity>>(cacheKey, RankingList, DateTime.Now.AddHours(2), 7);
                            else
                                return Fail(CommonConfig.NoDataTip);
                        }
                    }
                    else
                    {
                        DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                        DTK_Ranking_ListRequest dTK_Ranking_ListRequest = new DTK_Ranking_ListRequest();
                        dTK_Ranking_ListRequest.rankType = RandType;
                        dTK_Ranking_ListRequest.pageId = PageNo;
                        dTK_Ranking_ListRequest.pageSize = PageSize;
                        //dTK_Ranking_ListRequest.cid = cateid;
                        dTK_Ranking_ListRequest.IsReturnCommonInfo = true;
                        DTK_Ranking_ListResponse dTK_Ranking_ListResponse = dTK_ApiManage.GetRankingList(dTK_Ranking_ListRequest);
                        if (dTK_Ranking_ListResponse.code != 0)
                        {
                            //return Fail(dTK_Ranking_ListResponse.msg);
                            return Fail(CommonConfig.NoDataTip);
                        }
                        RankingList = dTK_Ranking_ListResponse.CommonGoodInfoList;
                        redisCache.Write(cacheKey, RankingList, DateTime.Now.AddHours(4.0), 7L);
                    }
                }

                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, RankingList));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取首页小类
        public ActionResult GetGoodSmallCate()
        {
            try
            {
                string cacheKey = "SmallCate";
                List<SmallCate> smallCateList = redisCache.Read<List<SmallCate>>(cacheKey, 7L);
                if (smallCateList == null)
                {
                    smallCateList = new List<SmallCate>();
                    smallCateList.Add(new SmallCate
                    {
                        Title = "推荐",
                        SubTitle = "猜你喜欢",
                        SmallType = 3
                    });
                    smallCateList.Add(new SmallCate
                    {
                        Title = "精品",
                        SubTitle = "非你莫属",
                        SmallType = 1
                    });
                    smallCateList.Add(new SmallCate
                    {
                        Title = "热销",
                        SubTitle = "超值好货",
                        SmallType = 4
                    });
                    smallCateList.Add(new SmallCate
                    {
                        Title = "实惠",
                        SubTitle = "品质生活",
                        SmallType = 2
                    });
                    redisCache.Write(cacheKey, smallCateList, 7L);
                }
                return SuccessList("获取成功!", smallCateList);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 今日推荐(2个小时更新一次)
        [NoNeedLogin]
        public ActionResult GetTodayGood(int User_ID = 0)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = "TodayGood";
                List<CommonGoodInfoEntity> RankingList = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);

                if (RankingList == null)
                {
                    Hyg.Common.DTKTools.DTK_ApiManage dTK_ApiManage = new Hyg.Common.DTKTools.DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    if (false)
                    {
                        //更改为每日爆款
                        DTK_Explosive_Goods_ListRequest dTK_Explosive_Goods_ListRequest = new DTK_Explosive_Goods_ListRequest();
                        //dTK_Explosive_Goods_ListRequest.cids = "4";
                        dTK_Explosive_Goods_ListRequest.IsReturnCommonInfo = true;
                        dTK_Explosive_Goods_ListRequest.pageId = "1";
                        //dTK_Explosive_Goods_ListRequest.PriceCid = 1;
                        dTK_Explosive_Goods_ListRequest.pageSize = 50;
                        DTK_Explosive_Goods_ListResponse dTK_Explosive_Goods_ListResponse = dTK_ApiManage.GetDTK_ExplosiveGoods(dTK_Explosive_Goods_ListRequest);

                        if (dTK_Explosive_Goods_ListResponse.code != 0)
                        {
                            //return Fail(dTK_OP_ListResponse.msg);
                            return Fail(CommonConfig.NoDataTip);
                        }


                        RankingList = dTK_Explosive_Goods_ListResponse.CommonGoodInfoList;//ConvertCommonGoodEntityByOPGood(dTK_Explosive_Goods_ListResponse.data, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    else
                    {
                        DTK_History_Low_Price_ListRequest dTK_History_Low_Price_ListRequest = new DTK_History_Low_Price_ListRequest();
                        dTK_History_Low_Price_ListRequest.pageId = "1";
                        dTK_History_Low_Price_ListRequest.pageSize = 50;
                        dTK_History_Low_Price_ListRequest.IsReturnCommonInfo = true;
                        DTK_History_Low_Price_ListResponse dTK_History_Low_Price_ListResponse = dTK_ApiManage.GetHistoryLowPriceListResponse(dTK_History_Low_Price_ListRequest);
                        if (dTK_History_Low_Price_ListResponse.code != 0)
                        {
                            return Fail(CommonConfig.NoDataTip);
                        }

                        RankingList = dTK_History_Low_Price_ListResponse.CommonGoodInfoList;

                        /*DTK_Ranking_ListRequest dTK_Ranking_ListRequest = new DTK_Ranking_ListRequest();
                        dTK_Ranking_ListRequest.rankType = 7;
                        DTK_Ranking_ListResponse dTK_Ranking_ListResponse = dTK_ApiManage.GetRankingList(dTK_Ranking_ListRequest);
                        if (dTK_Ranking_ListResponse.code != 0)
                        {
                            return Fail(dTK_Ranking_ListResponse.msg);
                        }
                        RankingList = ConvertCommonGoodEntityByRank(dTK_Ranking_ListResponse.data, dm_UserEntity, dm_BasesettingEntity, cacheKey);*/
                    }

                    redisCache.Write(cacheKey, RankingList, DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 10:00:00")), 7L);
                }
                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, RankingList));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 三合一超级搜索接口
        [NoNeedLogin]
        public ActionResult CommonSearchGood(int user_id = 0, int PlaformType = 1, int PageNo = 1, int PageSize = 10, string KeyWords = "20", int sort = 0)
        {
            try
            {
                //KeyWords = KeyWords == "" ? "潮流" : KeyWords.Substring(0, 1);
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("CommonSearchGood" + PlaformType + PageNo + PageSize + KeyWords + sort);
                List<CommonGoodInfoEntity> superGoodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                if (superGoodItems == null)
                {
                    if (PlaformType == 1)
                    {
                        DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);

                        if (dm_BasesettingEntity.goodsource == 0)
                        {
                            DTK_TB_Service_GoodRequest dTK_TB_Service_GoodRequest = new DTK_TB_Service_GoodRequest();
                            dTK_TB_Service_GoodRequest.pageNo = PageNo;
                            dTK_TB_Service_GoodRequest.pageSize = PageSize;
                            dTK_TB_Service_GoodRequest.keyWords = GetNumid(KeyWords);
                            //dTK_TB_Service_GoodRequest.startPrice = 0;
                            //dTK_TB_Service_GoodRequest.endPrice = 0;
                            //dTK_TB_Service_GoodRequest.startTkRate = 0;
                            //dTK_TB_Service_GoodRequest.startTkRate = 0;
                            dTK_TB_Service_GoodRequest.sort = GetSort(PlaformType, sort);
                            DTK_TB_Service_GoodResponse dTK_TB_Service_GoodResponse = dTK_ApiManage.GetDTK_TBServiceGood(dTK_TB_Service_GoodRequest);

                            if (dTK_TB_Service_GoodResponse.code != 0)
                                return Fail(dTK_TB_Service_GoodResponse.msg);
                            superGoodItems = ConvertCommonGoodEntityByTBServiceGood(dTK_TB_Service_GoodResponse.data, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                        }
                        else
                        {
                            DTK_Super_GoodRequest dTK_Super_GoodRequest = new DTK_Super_GoodRequest();
                            dTK_Super_GoodRequest.type = 0;
                            dTK_Super_GoodRequest.pageId = PageNo;
                            dTK_Super_GoodRequest.pageSize = PageSize;
                            dTK_Super_GoodRequest.keyWords = GetNumid(KeyWords);
                            dTK_Super_GoodRequest.tmall = 0;
                            dTK_Super_GoodRequest.haitao = 0;
                            dTK_Super_GoodRequest.sort = GetSort(PlaformType, sort);
                            DTK_Super_GoodResponse dTK_Super_GoodResponse = dTK_ApiManage.GetSuperGood(dTK_Super_GoodRequest);
                            if (dTK_Super_GoodResponse.code != 0)
                            {
                                //return Fail(dTK_Super_GoodResponse.msg);
                                return Fail(CommonConfig.NoDataTip);
                            }
                            superGoodItems = ConvertCommonGoodEntityBySuperGood(dTK_Super_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                        }
                    }
                    else if (PlaformType == 3)
                    {
                        JD_ApiManage jD_ApiManage = new JD_ApiManage(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);
                        Super_GoodQueryRequest super_GoodQueryRequest = new Super_GoodQueryRequest();
                        Super_GoodQueryDetailReq super_GoodQueryDetailReq = new Super_GoodQueryDetailReq();
                        super_GoodQueryDetailReq.keyword = KeyWords;
                        super_GoodQueryDetailReq.pageIndex = PageNo;
                        super_GoodQueryDetailReq.pageSize = PageSize;
                        super_GoodQueryRequest.goodsReqDTO = super_GoodQueryDetailReq;

                        Super_Jd_GoodInfo_Reponse super_Jd_GoodInfo_Reponse = jD_ApiManage.Super_GetGoodQueryResultByKeyWord(super_GoodQueryRequest);
                        if (super_Jd_GoodInfo_Reponse.IsError)
                        {
                            throw new Exception(super_Jd_GoodInfo_Reponse.message);
                        }
                        else
                        {
                            superGoodItems = ConvertCommonGoodEntityByJF(super_Jd_GoodInfo_Reponse.jFGoodsRespRows, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                        }

                        //JDApi jDApi = new JDApi(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);
                        //superGoodItems = ConvertCommonGoodEntityByJF(jDApi.GetJTT_SearchGoodItemList(KeyWords, PageNo, PageSize, 0, 0, GetSort(PlaformType, sort)), dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    else if (PlaformType == 4)
                    {
                        PDD_ApiManage pDD_ApiManage = new PDD_ApiManage(dm_BasesettingEntity.pdd_clientid, dm_BasesettingEntity.pdd_clientsecret, "");
                        Good_SearchRequest good_SearchRequest = new Good_SearchRequest();
                        good_SearchRequest.keyword = KeyWords;
                        good_SearchRequest.page = PageNo;
                        good_SearchRequest.page_size = PageSize;
                        good_SearchRequest.sort_type = int.Parse(GetSort(PlaformType, sort));
                        good_SearchRequest.with_coupon = false;
                        good_SearchRequest.cat_id = -1;
                        good_SearchRequest.IsReturnCommonInfo = true;
                        good_SearchRequest.pid = "1912666_177495987";
                        good_SearchRequest.custom_parameters = "guzilideaoqi";
                        Good_Search_ListResponse good_Search_ListResponse = pDD_ApiManage.Good_Search_List(good_SearchRequest);
                        if (good_Search_ListResponse.IsError)
                            throw new Exception(good_Search_ListResponse.error_response.sub_msg);
                        else
                        {
                            superGoodItems = good_Search_ListResponse.CommonGoodInfoList;
                        }
                        //PDDApi pDDApi = new PDDApi(dm_BasesettingEntity.pdd_clientid, dm_BasesettingEntity.pdd_clientsecret, "");
                        //superGoodItems = ConvertCommonGoodEntityByPDD(pDDApi.SearchGood(KeyWords, PageNo, PageSize, int.Parse(GetSort(PlaformType, sort)), false, -1), dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    if (superGoodItems.Count > 0)
                        redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }

                return SuccessList("获取成功", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, superGoodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        string GetSort(int plaform, int sort)
        {
            string sortName = "";
            if (plaform == 1)
            {
                switch (sort)
                {
                    case 0:
                        sortName = "total_sales_des";
                        break;
                    case 1:
                        sortName = "price_asc";
                        break;
                    case 2:
                        sortName = "price_des";
                        break;
                    case 3:
                        sortName = "total_sales_asc";
                        break;
                    case 4:
                        sortName = "total_sales_des";
                        break;
                    default:
                        sortName = "total_sales_des";
                        break;

                }
            }
            else if (plaform == 3)
            {
                switch (sort)
                {
                    case 1:
                        sortName = "1";
                        break;
                    case 2:
                        sortName = "2";
                        break;
                    case 3:
                        sortName = "3";
                        break;
                    case 4:
                        sortName = "4";
                        break;
                }
            }
            else if (plaform == 4)
            {
                switch (sort)
                {
                    case 0:
                        sortName = "0";
                        break;
                    case 1:
                        sortName = "3";
                        break;
                    case 2:
                        sortName = "4";
                        break;
                    case 3:
                        sortName = "5";
                        break;
                    case 4:
                        sortName = "6";
                        break;
                }
            }

            return sortName;
        }
        #endregion

        #region 超级搜索商品(2个小时更新一次)
        /// <summary>
        /// 超级搜索商品
        /// </summary>
        /// <param name="type">0-综合结果，1-大淘客商品，2-联盟商品</param>
        /// <param name="pageId">请求的页码，默认参数1</param>
        /// <param name="pageSize">默认为20，最大值100</param>
        /// <param name="keyWords">关键词搜索</param>
        /// <param name="tmall">1-天猫商品，0-所有商品，不填默认为0</param>
        /// <param name="haitao">1-海淘商品，0-所有商品，不填默认为0</param>
        /// <param name="sort">排序字段信息 销量（total_sales） 价格（price），排序_des（降序），排序_asc（升序）</param>
        /// <returns></returns>
        public ActionResult GetSuperSerachGood(int user_id, int type = 0, int pageId = 1, int pageSize = 20, string keyWords = "20", int tmall = 0, int haitao = 0, string sort = "total_sales_des")
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("SuperSerachGood" + type + pageId + pageSize + keyWords + tmall + haitao + sort);
                List<CommonGoodInfoEntity> superGoodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                if (superGoodItems == null)
                {
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Super_GoodRequest dTK_Super_GoodRequest = new DTK_Super_GoodRequest();
                    dTK_Super_GoodRequest.type = type;
                    dTK_Super_GoodRequest.pageId = pageId;
                    dTK_Super_GoodRequest.pageSize = pageSize;
                    dTK_Super_GoodRequest.keyWords = GetNumid(keyWords);
                    dTK_Super_GoodRequest.tmall = tmall;
                    dTK_Super_GoodRequest.haitao = haitao;
                    dTK_Super_GoodRequest.sort = sort;
                    DTK_Super_GoodResponse dTK_Super_GoodResponse = dTK_ApiManage.GetSuperGood(dTK_Super_GoodRequest);
                    if (dTK_Super_GoodResponse.code != 0)
                    {
                        //return Fail(dTK_Super_GoodResponse.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    superGoodItems = ConvertCommonGoodEntityBySuperGood(dTK_Super_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }


                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, superGoodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 大淘客搜索商品(2个小时更新一次)
        //public ActionResult GetDTKSearchGood(int user_id, int pageId = 1, int pageSize = 20, string cids = "", int subcid = -1, int juHuaSuan = -1, int taoQiangGou = -1, string keyWords = "", int tmall = -1, int tchaoshi = -1,int goldSeller=-1,int haitao=-1,int brand=-1,string brandIds="", string sort = "total_sales_des")
        public ActionResult GetDTKSearchGood(int user_id, int pageId = 1, int pageSize = 20, string cids = "", int subcid = -1, string keyWords = "20", string sort = "0")
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("DtkSerachGood" + pageId + pageSize + cids + subcid + keyWords + sort);
                List<CommonGoodInfoEntity> superGoodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                if (superGoodItems == null)
                {
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Get_dtk_Search_GoodRequest dtk_Get_dtk_Search_GoodRequest = new DTK_Get_dtk_Search_GoodRequest();
                    dtk_Get_dtk_Search_GoodRequest.pageId = pageId.ToString();
                    dtk_Get_dtk_Search_GoodRequest.pageSize = pageSize;
                    if (subcid > -1)
                    {
                        dtk_Get_dtk_Search_GoodRequest.subcid = subcid;
                    }
                    else
                    {
                        if (cids != "")
                            dtk_Get_dtk_Search_GoodRequest.cids = cids;
                    }
                    dtk_Get_dtk_Search_GoodRequest.keyWords = GetNumid(keyWords);
                    dtk_Get_dtk_Search_GoodRequest.sort = sort;
                    DTK_Get_dtk_Search_GoodResponse dTK_Get_dtk_Search_GoodResponse = dTK_ApiManage.GetDtkSearchGood(dtk_Get_dtk_Search_GoodRequest);
                    if (dTK_Get_dtk_Search_GoodResponse.code != 0)
                    {
                        //return Fail(dTK_Get_dtk_Search_GoodResponse.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    superGoodItems = ConvertCommonGoodEntityByDTK_SearchGoodItem(dTK_Get_dtk_Search_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }


                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, superGoodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取首页不同类别的商品(9.9包邮、超级券)
        public ActionResult GetRecommendGoodByTB(string ChannelType, int User_ID = 0, int PageNo = 1, int PageSize = 10)
        {
            try
            {
                string appid = CheckAPPID();

                string keyWords = "";
                #region 获取关键词
                List<CategoryItem> categoryItems = redisCache.Read<List<CategoryItem>>("SuperCategory", 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);

                #endregion

                if (ChannelType == "99zhuanqu")
                {
                    CategoryItem categoryItem = categoryItems.Where(t => t.cid == dm_BasesettingEntity.goodtype).FirstOrDefault();
                    keyWords = GetNumid(categoryItem.IsEmpty() ? "潮" : categoryItem.cname.Substring(0, 2));
                }
                else if (ChannelType == "chaojiquan")
                {
                    CategoryItem categoryItem = categoryItems.Where(t => t.cid == dm_BasesettingEntity.super_coupon_goodtype).FirstOrDefault();
                    keyWords = GetNumid(categoryItem.IsEmpty() ? "潮" : categoryItem.cname);
                }

                string cacheKey = Md5Helper.Hash("GetRecommendGoodByTB" + keyWords + ChannelType + PageNo + PageSize);
                List<CommonGoodInfoEntity> superGoodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);

                if (superGoodItems == null)
                {
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);

                    if (ChannelType == "99zhuanqu")//使用联盟搜索
                    {
                        int? min_price = dm_BasesettingEntity.min_price, max_price = dm_BasesettingEntity.max_price, min_tk_rate = dm_BasesettingEntity.min_tk_rate, max_tk_rate = dm_BasesettingEntity.max_tk_rate;
                        DTK_TB_Service_GoodRequest dTK_TB_Service_GoodRequest = new DTK_TB_Service_GoodRequest();
                        dTK_TB_Service_GoodRequest.pageNo = PageNo;
                        dTK_TB_Service_GoodRequest.pageSize = PageSize;
                        dTK_TB_Service_GoodRequest.keyWords = keyWords;
                        if (min_price > 0)
                            dTK_TB_Service_GoodRequest.startPrice = min_price;
                        if (max_price > 0)
                            dTK_TB_Service_GoodRequest.endPrice = max_price;
                        if (min_tk_rate > 0)
                            dTK_TB_Service_GoodRequest.startTkRate = min_tk_rate;
                        if (max_tk_rate > 0)
                            dTK_TB_Service_GoodRequest.endTkRate = max_tk_rate;

                        //dTK_TB_Service_GoodRequest.sort = GetSort(PlaformType, sort);
                        DTK_TB_Service_GoodResponse dTK_TB_Service_GoodResponse = dTK_ApiManage.GetDTK_TBServiceGood(dTK_TB_Service_GoodRequest);

                        if (dTK_TB_Service_GoodResponse.code != 0)
                            //return Fail(dTK_TB_Service_GoodResponse.msg);
                            return Fail(CommonConfig.NoDataTip);
                        superGoodItems = ConvertCommonGoodEntityByTBServiceGood(dTK_TB_Service_GoodResponse.data, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    else if (ChannelType == "chaojiquan")//使用大淘客搜索
                    {
                        DTK_Get_dtk_Search_GoodRequest dtk_Get_dtk_Search_GoodRequest = new DTK_Get_dtk_Search_GoodRequest();
                        dtk_Get_dtk_Search_GoodRequest.pageId = PageNo.ToString();
                        dtk_Get_dtk_Search_GoodRequest.pageSize = PageSize;
                        dtk_Get_dtk_Search_GoodRequest.keyWords = keyWords;
                        if (dm_BasesettingEntity.super_coupon_min_price > 0)
                            dtk_Get_dtk_Search_GoodRequest.priceLowerLimit = dm_BasesettingEntity.super_coupon_min_price;
                        if (dm_BasesettingEntity.super_coupon_max_price > 0)
                            dtk_Get_dtk_Search_GoodRequest.priceUpperLimit = dm_BasesettingEntity.super_coupon_max_price;
                        if (dm_BasesettingEntity.super_coupon_goodtype > 0)
                            dtk_Get_dtk_Search_GoodRequest.cids = dm_BasesettingEntity.super_coupon_goodtype.ToString();
                        if (dm_BasesettingEntity.super_coupon_min_tk_rate > 0)
                            dtk_Get_dtk_Search_GoodRequest.commissionRateLowerLimit = dm_BasesettingEntity.super_coupon_min_tk_rate;
                        if (dm_BasesettingEntity.super_coupon_couponPriceLowerLimit > 0)
                            dtk_Get_dtk_Search_GoodRequest.couponPriceLowerLimit = dm_BasesettingEntity.super_coupon_couponPriceLowerLimit;
                        DTK_Get_dtk_Search_GoodResponse dTK_Get_dtk_Search_GoodResponse = dTK_ApiManage.GetDtkSearchGood(dtk_Get_dtk_Search_GoodRequest);
                        if (dTK_Get_dtk_Search_GoodResponse.code != 0)
                        {
                            //return Fail(dTK_Get_dtk_Search_GoodResponse.msg);
                            return Fail(CommonConfig.NoDataTip);
                        }

                        superGoodItems = ConvertCommonGoodEntityByDTK_SearchGoodItem(dTK_Get_dtk_Search_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    else if (ChannelType == "chihuo")
                    {
                        DTK_OP_ListRequest dTK_OP_ListRequest = new DTK_OP_ListRequest();
                        dTK_OP_ListRequest.nineCid = "2";
                        dTK_OP_ListRequest.pageId = PageNo.ToString();
                        dTK_OP_ListRequest.pageSize = PageSize;
                        dTK_OP_ListRequest.IsReturnCommonInfo = true;
                        DTK_OP_ListResponse dTK_OP_ListResponse = dTK_ApiManage.GetOPGood(dTK_OP_ListRequest);

                        superGoodItems = dTK_OP_ListResponse.CommonGoodInfoList;
                        //superGoodItems = GetActivityGoodData(User_ID, 38, PageNo.ToString(), PageSize, 4).ToList();
                    }

                    if (superGoodItems.Count > 0)
                        redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }

                if (IsPreview(dm_BasesettingEntity))
                {
                    superGoodItems = superGoodItems.Select(p => { p.SuperCommission = 0M; p.LevelCommission = 0M; return p; }).ToList();
                }

                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, superGoodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 9.9包邮精选(两个小时更新一次)
        /// <summary>
        /// 9.9包邮精选
        /// </summary>
        /// <param name="pageId">默认为20，最大值100</param>
        /// <param name="pageSize">常规分页方式，请直接传入对应页码</param>
        /// <param name="nineCid">9.9精选的类目id，分类id请求详情：-1-精选，1 -居家百货，2 -美食，3 -服饰，4 -配饰，5 -美妆，6 -内衣，7 -母婴，8 -箱包，9 -数码配件，10 -文娱车品</param>
        /// <returns></returns>
        public ActionResult GetOPGood(int user_id, string pageId = "1", int pageSize = 20, string nineCid = "1")
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("OPGood" + pageId + pageSize + nineCid);
                List<CommonGoodInfoEntity> oPGoodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                if (oPGoodItems == null)
                {
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_OP_ListRequest dTK_OP_ListRequest = new DTK_OP_ListRequest();
                    dTK_OP_ListRequest.nineCid = nineCid;
                    dTK_OP_ListRequest.pageId = pageId;
                    dTK_OP_ListRequest.pageSize = pageSize;
                    DTK_OP_ListResponse dTK_OP_ListResponse = dTK_ApiManage.GetOPGood(dTK_OP_ListRequest);
                    if (dTK_OP_ListResponse.code != 0)
                    {
                        //return Fail(dTK_OP_ListResponse.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    oPGoodItems = ConvertCommonGoodEntityByOPGood(dTK_OP_ListResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, oPGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }


                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, oPGoodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 联想词(每天更新一次)
        /// <summary>
        /// 联想词
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <param name="type">当前搜索API类型：1.大淘客搜索 2.联盟搜索 3.超级搜索</param>
        /// <returns></returns>
        public ActionResult SearchSuggestion(string keyWords, int type = 3)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("Suggestion" + keyWords + type);
                List<SuggestionItem> SuggestionList = redisCache.Read<List<SuggestionItem>>(cacheKey, 7L);
                if (SuggestionList == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Search_SuggestionRequest dTK_Search_SuggestionRequest = new DTK_Search_SuggestionRequest();
                    dTK_Search_SuggestionRequest.type = type;
                    dTK_Search_SuggestionRequest.keyWords = keyWords;
                    DTK_Search_SuggestionResponse dTK_Search_SuggestionResponse = dTK_ApiManage.GetSearchSuggestion(dTK_Search_SuggestionRequest);
                    if (dTK_Search_SuggestionResponse.code != 0)
                    {
                        //return Fail(dTK_Search_SuggestionResponse.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    SuggestionList = dTK_Search_SuggestionResponse.data;
                    redisCache.Write(cacheKey, SuggestionList, DateTime.Now.AddDays(1.0), 7L);
                }
                return SuccessList("获取成功!", SuggestionList);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 热门活动(每天更新一次)
        /// <summary>
        /// 热门活动
        /// </summary>
        /// <returns></returns>
        public ActionResult GetActivityCatalogue()
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = "ActivityCatalogue";
                List<ActivityCatalogueItem> activityCatalogueItems = redisCache.Read<List<ActivityCatalogueItem>>(cacheKey, 7L);
                if (activityCatalogueItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Activity_CatalogueRequest dTK_Activity_CatalogueRequest = new DTK_Activity_CatalogueRequest();
                    DTK_Activity_CatalogueResponse dTK_Activity_CatalogueResponse = dTK_ApiManage.GetActivityCatalogueList(dTK_Activity_CatalogueRequest);
                    if (dTK_Activity_CatalogueResponse.code != 0)
                    {
                        //return Fail(dTK_Activity_CatalogueResponse.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    activityCatalogueItems = dTK_Activity_CatalogueResponse.data;
                    redisCache.Write(cacheKey, activityCatalogueItems, DateTime.Now.AddDays(1.0), 7L);
                }
                return SuccessList("获取成功!", activityCatalogueItems);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 热门活动商品(2个小时更新一次)
        public ActionResult GetActivityGoodList(int activityId, int user_id = -1, string pageId = "1", int pageSize = 20, int cid = 1)
        {
            try
            {
                return SuccessList("获取成功!", GetActivityGoodData(user_id, activityId, pageId, pageSize, cid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        IEnumerable<CommonGoodInfoEntity> GetActivityGoodData(int user_id, int activityId, string pageId = "1", int pageSize = 20, int cid = 1)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("ActivityGoodList" + activityId + pageId + pageSize + cid);
                List<CommonGoodInfoEntity> activityGoodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                if (activityGoodItems == null)
                {
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Activity_GoodListRequest dTK_Activity_GoodListRequest = new DTK_Activity_GoodListRequest();
                    dTK_Activity_GoodListRequest.pageId = pageId;
                    dTK_Activity_GoodListRequest.pageSize = pageSize;
                    if (cid > -1)
                        dTK_Activity_GoodListRequest.cid = cid;
                    dTK_Activity_GoodListRequest.activityId = activityId;
                    dTK_Activity_GoodListRequest.IsReturnCommonInfo = true;
                    DTK_Activity_GoodListResponse dTK_Activity_GoodListResponse = dTK_ApiManage.GetActivityGoodList(dTK_Activity_GoodListRequest);
                    if (dTK_Activity_GoodListResponse.code != 0)
                    {
                        throw new Exception(dTK_Activity_GoodListResponse.msg);
                    }
                    activityGoodItems = ConvertCommonGoodEntityByActivityGood(dTK_Activity_GoodListResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, activityGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }
                return CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, activityGoodItems);
            }
            catch (Exception)
            {
                return new List<CommonGoodInfoEntity>();
            }

        }
        #endregion

        #region 精选专辑
        /// <summary>
        /// 精选专辑
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTopicCatalogue()
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = "TopicCatalogue";
                List<BannerInfo> topicCatalogueItems = redisCache.Read<List<BannerInfo>>(cacheKey, 7L);
                if (topicCatalogueItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Topic_CatalogueRequest dTK_Topic_CatalogueRequest = new DTK_Topic_CatalogueRequest();
                    DTK_Topic_CatalogueResponse dTK_Topic_CatalogueResponse = dTK_ApiManage.GetTopicCatalogue(dTK_Topic_CatalogueRequest);
                    if (dTK_Topic_CatalogueResponse.code != 0)
                    {
                        return Fail(dTK_Topic_CatalogueResponse.msg);
                    }
                    topicCatalogueItems = new List<BannerInfo>();
                    foreach (TopicCatalogueItem item in dTK_Topic_CatalogueResponse.data)
                    {
                        if (item.banner.Count > 0)
                        {
                            topicCatalogueItems.Add(new BannerInfo
                            {
                                banner = item.banner[0],
                                topicId = item.topicId,
                                topicName = item.topicName
                            });
                        }
                    }
                    if (topicCatalogueItems.Count > 0)
                        redisCache.Write(cacheKey, topicCatalogueItems, DateTime.Now.AddDays(1.0), 7L);
                }
                return SuccessList("获取成功!", topicCatalogueItems);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 精选专辑商品
        public ActionResult GetTopicGoodList(int user_id, int topicId, string pageId = "1", int pageSize = 20)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("TopicGoodList" + topicId + pageId + pageSize);
                List<CommonGoodInfoEntity> topicGoodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                if (topicGoodItems == null)
                {
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Topic_GoodListRequest dTK_Topic_GoodListRequest = new DTK_Topic_GoodListRequest();
                    dTK_Topic_GoodListRequest.pageId = pageId;
                    dTK_Topic_GoodListRequest.pageSize = pageSize;
                    dTK_Topic_GoodListRequest.topicId = topicId;
                    DTK_Topic_GoodListResponse dTK_Topic_GoodListResponse = dTK_ApiManage.GetTopicGoodList(dTK_Topic_GoodListRequest);
                    if (dTK_Topic_GoodListResponse.code != 0)
                    {
                        //return Fail(dTK_Topic_GoodListResponse.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    topicGoodItems = ConvertCommonGoodEntityByTopicGood(dTK_Topic_GoodListResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, topicGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }

                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, topicGoodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 官方活动推广
        public ActionResult GetTBTopicList(int userid, int type = 0, string pageId = "1", int pageSize = 20)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("TBTopicList" + type + pageId + pageSize);
                List<TB_TopicItem> tB_TopicItems = redisCache.Read<List<TB_TopicItem>>(cacheKey, 7L);
                if (tB_TopicItems == null)
                {
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(userid);
                    if (dm_UserEntity.tb_relationid.IsEmpty())
                    {
                        return NoRelationID("渠道未备案!");
                    }
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_TB_Topic_ListRequest dTK_TB_Topic_ListRequest = new DTK_TB_Topic_ListRequest();
                    dTK_TB_Topic_ListRequest.type = type;
                    dTK_TB_Topic_ListRequest.pageId = pageId;
                    dTK_TB_Topic_ListRequest.pageSize = pageSize;
                    dTK_TB_Topic_ListRequest.channelID = long.Parse(dm_UserEntity.tb_relationid.ToString());
                    DTK_TB_Topic_ListResponse dTK_TB_Topic_ListResponse = dTK_ApiManage.GettTBTopicList(dTK_TB_Topic_ListRequest);
                    if (dTK_TB_Topic_ListResponse.code != 0)
                    {
                        //return Fail(dTK_TB_Topic_ListResponse.msg);
                        return Fail(CommonConfig.NoDataTip);
                    }
                    tB_TopicItems = dTK_TB_Topic_ListResponse.data;
                    redisCache.Write(cacheKey, tB_TopicItems, DateTime.Now.AddDays(1.0), 7L);
                }
                return SuccessList("获取成功!", tB_TopicItems);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        public ActionResult Get_TB_GoodList()
        {
            return View();
        }

        #region 淘宝高佣转链
        /// <summary>
        /// 淘宝高佣转链
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="originid">商品id</param>
        /// <param name="couponid">优惠券id</param>
        /// <returns></returns>
        public ActionResult ConvertLinkByTB(int user_id, string originid, string couponid)
        {
            try
            {
                string appid = CheckAPPID();

                if (user_id <= 0)
                    return FailNoLogin();

                #region 解析出来优惠券ID
                if (couponid.Contains("activityId="))
                {
                    Regex reg = new Regex("(?<=(activityId=)).*?(?=(\n|$))");
                    couponid = reg.Match(couponid).Value;
                }
                #endregion

                string cacheKey = Md5Helper.Hash(user_id.ToString() + originid + couponid);
                PrivilegeLinkResult ConvertLinkResult = redisCache.Read<PrivilegeLinkResult>(cacheKey, 7L);
                if (ConvertLinkResult == null)
                {
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                    if (dm_UserEntity.tb_relationid.IsEmpty())
                    {
                        return NoRelationID("渠道未备案!");
                    }
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Privilege_LinkRequest dTK_Privilege_LinkRequest = new DTK_Privilege_LinkRequest();
                    dTK_Privilege_LinkRequest.goodsId = originid;
                    dTK_Privilege_LinkRequest.pid = dm_BasesettingEntity.tb_relation_pid;
                    dTK_Privilege_LinkRequest.channelId = dm_UserEntity.tb_relationid.ToString();
                    dTK_Privilege_LinkRequest.couponId = couponid.Contains("//") ? "" : couponid;
                    DTK_Privilege_LinkResponse dTK_Privilege_LinkResponse = dTK_ApiManage.GetPrivilegeLink(dTK_Privilege_LinkRequest);
                    if (dTK_Privilege_LinkResponse.code != 0)
                    {
                        return Fail(dTK_Privilege_LinkResponse.msg);
                    }
                    ConvertLinkResult = dTK_Privilege_LinkResponse.data;
                    /*DTK_Good_DetailsItem dTK_Good_DetailsItem = GetGoodDetail(appid, originid);
                    if (dTK_Good_DetailsItem != null)
                    {
                        ConvertLinkResult.tpwd = string.Format(@"{0}
【原价】{1}元
【券后价】{2}元
复制这条信息{3}打开手机淘宝领券下单", dTK_Good_DetailsItem.title, (dTK_Good_DetailsItem.actualPrice + dTK_Good_DetailsItem.couponPrice), dTK_Good_DetailsItem.actualPrice, ConvertLinkResult.tpwd);
                    }
                    else
                    {
                        ConvertLinkResult.tpwd = string.Format(@"复制这条信息{0}打开手机淘宝领券下单", ConvertLinkResult.tpwd);
                    }*/

                    ConvertLinkResult.tpwd = string.Format(@"复制这条信息{0}打开手机淘宝领券下单", ConvertLinkResult.tpwd);

                    redisCache.Write(cacheKey, ConvertLinkResult, DateTime.Now.AddHours(1.0), 7L);
                }
                return Success("转链成功!", ConvertLinkResult);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        DTK_Good_DetailsItem GetGoodDetail(string appid, string goodid)
        {
            try
            {
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                DTK_Get_Good_DetailsRequest dTK_Get_Good_DetailsRequest = new DTK_Get_Good_DetailsRequest();
                dTK_Get_Good_DetailsRequest.goodsId = goodid;
                DTK_Get_Good_DetailsResponse dTK_Get_Good_DetailsResponse = dTK_ApiManage.GetGoodDetail(dTK_Get_Good_DetailsRequest);
                if (dTK_Get_Good_DetailsResponse.code == 0)
                    return dTK_Get_Good_DetailsResponse.data;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 私域备案
        public ActionResult SavePublisherInfo(string SessionKey)
        {
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                //dm_BasesettingEntity.tb_appkey = "29236073";
                //dm_BasesettingEntity.tb_appsecret = "29de7a8560d773736ef5bf568a7961bd";
                ITopClient client = new DefaultTopClient(tb_url, dm_BasesettingEntity.tb_appkey, dm_BasesettingEntity.tb_appsecret);
                TbkScPublisherInfoSaveRequest req = new TbkScPublisherInfoSaveRequest();
                req.RelationFrom = "1";
                req.OfflineScene = "1";
                req.OnlineScene = "1";
                req.InviterCode = dm_BasesettingEntity.tb_relation_invitecode;
                req.InfoType = 1L;
                req.Note = "哆来米代理申请";
                req.RegisterInfo = "{\"phoneNumber\":\"18801088599\",\"city\":\"江苏省\",\"province\":\"南京市\",\"location\":\"玄武区花园小区\",\"detailAddress\":\"5号楼3单元101室\"}";
                TbkScPublisherInfoSaveResponse rsp = client.Execute(req, SessionKey);
                if (rsp.Data == null)
                    throw new Exception(rsp.SubErrMsg);

                return Success("备案成功!", rsp.Data);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }

        }
        #endregion

        #region 备案查询
        public ActionResult GetPublisherInfo(string SessionKey, TbkScPublisherInfoGetRequest req)
        {
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);

                ITopClient client = new DefaultTopClient(tb_url, dm_BasesettingEntity.tb_appkey, dm_BasesettingEntity.tb_appsecret);
                req.InfoType = 1L;
                req.RelationApp = "common";
                //req.SpecialId = "1212";
                TbkScPublisherInfoGetResponse rsp = client.Execute(req, SessionKey);

                return SuccessList("备案查询成功!", rsp.Data.RootPidChannelList);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取京东频道列表
        /// <summary>
        /// 获取京东频道列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Get_JD_EliteIDList()
        {
            try
            {
                return Fail("该接口已废弃,请使用获取Banner图接口");
                List<JDEliteIDInfo> jDEliteIDInfos = redisCache.Read<List<JDEliteIDInfo>>("JDEliteIDList", 7);
                if (jDEliteIDInfos == null)
                {
                    jDEliteIDInfos = new List<JDEliteIDInfo>();
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 1, name = "好券商品" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 2, name = "超级大卖场" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 10, name = "9.9专区" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 22, name = "热销爆品" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 23, name = "为你推荐" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 24, name = "数码家电" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 25, name = "超市" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 26, name = "母婴玩具" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 27, name = "家具日用" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 28, name = "美妆穿搭" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 29, name = "医药保健" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 30, name = "图书文具" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 31, name = "今日必推" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 32, name = "品牌好货" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 33, name = "秒杀商品" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 34, name = "拼购商品" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 109, name = "新品首发" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 110, name = "自营" });
                    jDEliteIDInfos.Add(new JDEliteIDInfo { id = 125, name = "首购商品" });

                    redisCache.Write<List<JDEliteIDInfo>>("JDEliteIDList", jDEliteIDInfos, 7);
                }

                return SuccessList("京东频道获取成功!", jDEliteIDInfos);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取京东商品列表(每两个小时更新一次)
        /// <summary>
        /// 获取京东商品列表
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="eliteId">频道id：1-好券商品,2-超级大卖场,10-9.9专区,22-热销爆品,23-为你推荐,24-数码家电,25-超市,26-母婴玩具,27-家具日用,28-美妆穿搭,29-医药保健,30-图书文具,31-今日必推,32-品牌好货,33-秒杀商品,34-拼购商品,109-新品首发,110-自营,125-首购商品</param>
        /// <param name="pageIndex">页码，默认1</param>
        /// <param name="pageSize">每页数量，默认20，上限50</param>
        /// <param name="sortname">排序字段(price：单价, commissionShare：佣金比例, commission：佣金， inOrderCount30DaysSku：sku维度30天引单量，comments：评论数，goodComments：好评数)</param>
        /// <param name="sort">asc,desc升降序,默认降序</param>
        /// <returns></returns>
        public ActionResult Get_JD_GoodList(int user_id = 0, int eliteId = 1, int pageIndex = 1, int pageSize = 20, string sortname = "price", string sort = "asc")
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("JDGoodList" + eliteId + pageIndex + pageSize + sortname + sort);
                List<CommonGoodInfoEntity> jFGoodsRespRows = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                if (jFGoodsRespRows == null)
                {
                    GoodQueryRequest goodQueryRequest = new GoodQueryRequest();
                    GoodQueryDetailReq goodQueryDetailReq = new GoodQueryDetailReq();
                    goodQueryDetailReq.eliteId = eliteId;
                    goodQueryDetailReq.pageIndex = pageIndex;
                    goodQueryDetailReq.pageSize = pageSize;
                    goodQueryDetailReq.sortName = sortname;
                    goodQueryDetailReq.sort = sort;
                    goodQueryRequest.goodsReq = goodQueryDetailReq;
                    JD_ApiManage jD_ApiManage = new JD_ApiManage(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);

                    Hyg.Common.JDTools.JDResponse.Jd_GoodInfo_Reponse jd_GoodInfo_Reponse = jD_ApiManage.GetGoodQueryResultByKeyWord(goodQueryRequest);

                    if (jd_GoodInfo_Reponse.IsError)
                    {
                        throw new Exception(jd_GoodInfo_Reponse.message);
                    }
                    else
                    {
                        jFGoodsRespRows = ConvertCommonGoodEntityByJF(jd_GoodInfo_Reponse.jFGoodsRespRows, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }

                    //JDApi jDApi = new JDApi(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);
                    //jFGoodsRespRows = ConvertCommonGoodEntityByJF(jDApi.GetGoodList(eliteId, pageIndex, pageSize, sortname, sort), dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    //jFGoodsRespRows= ConvertCommonGoodEntityByJTT(jDApi.GetJTT_GoodItemInfoList("女装", 1, 20, 0, 0, -1, ""), dm_UserEntity, dm_BasesettingEntity, cacheKey);

                    if (jFGoodsRespRows.Count > 0)
                    {
                        redisCache.Write(cacheKey, jFGoodsRespRows, DateTime.Now.AddHours(2.0), 7L);
                    }
                    else
                    {
                        return Fail("没有更多数据了");
                    }
                }

                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, jFGoodsRespRows));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 京东商品转链
        public ActionResult ConvertLinkByJD(int user_id, string skuid, string couponlink)
        {
            try
            {
                string appid = CheckAPPID();
                if (user_id <= 0)
                    return FailNoLogin();
                string cacheKey = Md5Helper.Hash(user_id.ToString() + skuid + couponlink + "2");
                JDLinkInfo jDLinkInfo = redisCache.Read<JDLinkInfo>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                if (jDLinkInfo == null)
                {
                    JD_ApiManage jD_ApiManage = new JD_ApiManage(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);

                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                    if (dm_UserEntity.jd_pid.IsEmpty())
                    {
                        #region 自动分配京东pid
                        dm_UserEntity = dM_PidIBLL.AutoAssignJDPID(dm_UserEntity);
                        #endregion
                    }

                    Super_PromotionByToolRequest super_PromotionByToolRequest = new Super_PromotionByToolRequest();
                    super_PromotionByToolRequest.promotionCodeReq = new Super_PromotionByTool
                    {
                        chainType = 2,
                        couponUrl = HttpUtility.UrlEncode(couponlink),
                        materialId = "http://item.jd.com/" + skuid + ".html",
                        positionId = dm_UserEntity.jd_pid,
                        unionId = dm_BasesettingEntity.jd_accountid.ToString()
                    };
                    Hyg.Common.JDTools.JDModel.ConvertLinkResultEntity convertLinkResultEntity = jD_ApiManage.GetConvertLinkByTool(super_PromotionByToolRequest);
                    jDLinkInfo = new JDLinkInfo();
                    jDLinkInfo.clickURL = convertLinkResultEntity.shortURL;
                    if (jDLinkInfo != null)
                    {
                        jDLinkInfo.tpwd = "下单链接:" + jDLinkInfo.clickURL;
                        redisCache.Write(cacheKey, jDLinkInfo, DateTime.Now.AddMinutes(5.0), 7L);
                    }
                }

                return Success("获取成功!", jDLinkInfo);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 京东商品搜索查询
        public ActionResult Get_JD_SearchGoodList(int User_ID = 0, string keyWord = "1", int PageNo = 1, int PageSize = 10, decimal price_start = 0, decimal price_end = 0, int cate_id = 0, string sort = "finally")
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("JDSearchGoodList" + keyWord + PageNo + PageSize + price_start + price_end + cate_id + sort);
                List<CommonGoodInfoEntity> jFGoodsRespRows = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);
                if (jFGoodsRespRows == null)
                {
                    JDApi jDApi = new JDApi(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);
                    jFGoodsRespRows = ConvertCommonGoodEntityByJTT(jDApi.GetJTT_GoodItemInfoList(keyWord, PageNo, PageSize, price_start, price_end, cate_id, sort), dm_UserEntity, dm_BasesettingEntity, cacheKey);

                    if (jFGoodsRespRows.Count > 0)
                    {
                        redisCache.Write(cacheKey, jFGoodsRespRows, DateTime.Now.AddHours(2.0), 7L);
                    }
                    else
                    {
                        return Fail("没有更多数据了");
                    }
                }

                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, jFGoodsRespRows), new { RequestDetailID = cacheKey });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        public ActionResult Get_JD_Order()
        {
            return View();
        }

        #region 获取拼多多商品类目
        public ActionResult Get_PDD_CatList()
        {
            try
            {
                List<PDDCatInfo> jDEliteIDInfos = redisCache.Read<List<PDDCatInfo>>("PDD_CatList", 7);
                if (jDEliteIDInfos == null)
                {
                    jDEliteIDInfos = new List<PDDCatInfo>();
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 15, name = "百货" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 4, name = "母婴" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 1, name = "食品" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 14, name = "女装" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 18, name = "电器" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 1281, name = "鞋包" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 1282, name = "内衣" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 16, name = "美妆" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 743, name = "男装" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 13, name = "水果" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 818, name = "家纺" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 2478, name = "文具" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 1451, name = "运动" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 590, name = "虚拟" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 2048, name = "汽车" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 1917, name = "家装" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 2974, name = "家具" });
                    jDEliteIDInfos.Add(new PDDCatInfo { id = 3279, name = "医药" });

                    redisCache.Write<List<PDDCatInfo>>("PDD_CatList", jDEliteIDInfos, 7);
                }

                return SuccessList("拼多多类目获取成功!", jDEliteIDInfos);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取拼多多商品列表
        /// <summary>
        /// 获取拼多多商品列表
        /// </summary>
        /// <param name="keyWord">关键词</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="sort">排序方式:0-综合排序;1-按佣金比率升序;2-按佣金比例降序;3-按价格升序;4-按价格降序;5-按销量升序;6-按销量降序;7-优惠券金额排序升序;8-优惠券金额排序降序;9-券后价升序排序;10-券后价降序排序;11-按照加入多多进宝时间升序;12-按照加入多多进宝时间降序;13-按佣金金额升序排序;14-按佣金金额降序排序;15-店铺描述评分升序;16-店铺描述评分降序;17-店铺物流评分升序;18-店铺物流评分降序;19-店铺服务评分升序;20-店铺服务评分降序;27-描述评分击败同类店铺百分比升序，28-描述评分击败同类店铺百分比降序，29-物流评分击败同类店铺百分比升序，30-物流评分击败同类店铺百分比降序，31-服务评分击败同类店铺百分比升序，32-服务评分击败同类店铺百分比降序</param>
        /// <returns></returns>
        public ActionResult Get_PDD_GoodList(int user_id = 0, string keyWord = "", int pageIndex = 1, int pageSize = 20, int sort = 0, bool with_coupon = false, int cat_id = 1)
        {
            try
            {
                keyWord = "";//不传空的话需要备案
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("PDDGoodList" + keyWord + pageIndex + pageSize + sort + with_coupon + cat_id);
                List<CommonGoodInfoEntity> goodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                if (goodItems == null)
                {
                    PDDApi pDDApi = new PDDApi(dm_BasesettingEntity.pdd_clientid, dm_BasesettingEntity.pdd_clientsecret, "");
                    //goodItems = ConvertCommonGoodEntityByPDD(pDDApi.GetRecommendGood(4, user_id.ToString(), pageSize, pageIndex.ToString(), pageSize, dm_UserEntity.pdd_pid, cat_id), dm_UserEntity, dm_BasesettingEntity, cacheKey);// (keyWord, pageIndex, pageSize, sort, with_coupon, cat_id), dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    goodItems = ConvertCommonGoodEntityByPDD(pDDApi.SearchGood(keyWord, pageIndex, pageSize, sort, with_coupon, cat_id), dm_UserEntity, dm_BasesettingEntity, cacheKey);

                    if (goodItems.Count > 0)
                    {
                        redisCache.Write(cacheKey, goodItems, DateTime.Now.AddHours(2.0), 7L);
                    }
                    else
                    {
                        return Fail("没有更多数据了");
                    }
                }

                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, goodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 多多进宝频道推广商品
        /// <summary>
        /// 多多进宝频道推广商品
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="channel_type">0-1.9包邮, 1-今日爆款, 2-品牌清仓,3-相似商品推荐,4-猜你喜欢,5-实时热销,6-实时收益,7-今日畅销,8-高佣榜单，默认1</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public ActionResult GetRecommendGoodByPDD(int user_id = 0, int channel_type = 0, int pageIndex = 1, int pageSize = 20, long cat_id = 0)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("PDDRecommendGoodList" + channel_type + "" + pageIndex + "" + pageSize + "" + cat_id);
                List<CommonGoodInfoEntity> goodItems = redisCache.Read<List<CommonGoodInfoEntity>>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                if (goodItems == null)
                {
                    PDDApi pDDApi = new PDDApi(dm_BasesettingEntity.pdd_clientid, dm_BasesettingEntity.pdd_clientsecret, "");
                    goodItems = ConvertCommonGoodEntityByPDD(pDDApi.GetRecommendGood(channel_type, "", pageSize, "", pageSize * pageIndex, "", cat_id), dm_UserEntity, dm_BasesettingEntity, cacheKey);

                    if (goodItems.Count > 0)
                    {
                        redisCache.Write(cacheKey, goodItems, DateTime.Now.AddHours(2.0), 7L);
                    }
                    else
                    {
                        return Fail("没有更多数据了");
                    }
                }

                return SuccessList("获取成功!", CheckPreviewOutGood(cacheKey, dm_BasesettingEntity, dm_UserEntity, goodItems));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 拼多多商品转链
        public ActionResult ConvertLinkByPDD(int user_id, string skuid)
        {
            try
            {
                string appid = CheckAPPID();
                if (user_id <= 0)
                    return FailNoLogin();
                string cacheKey = Md5Helper.Hash(user_id.ToString() + skuid + "3");
                GeneralUrl pDDLinkInfo = redisCache.Read<GeneralUrl>(cacheKey, 7L);
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                if (pDDLinkInfo == null)
                {
                    PDDApi pDDApi = new PDDApi(dm_BasesettingEntity.pdd_clientid, dm_BasesettingEntity.pdd_clientsecret, "");
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    if (dm_UserEntity.pdd_pid.IsEmpty())
                    {
                        #region 自动分配拼多多pid
                        dm_UserEntity = dM_PidIBLL.AutoAssignPDDPID(dm_UserEntity);
                        #endregion
                    }

                    pDDLinkInfo = pDDApi.GeneralUrl(skuid, dm_UserEntity.pdd_pid);

                    if (pDDLinkInfo != null)
                    {
                        pDDLinkInfo.tpwd = "下单链接:" + pDDLinkInfo.mobile_short_url;
                        redisCache.Write(cacheKey, pDDLinkInfo, DateTime.Now.AddHours(2.0), 7L);
                    }
                }

                return Success("转链成功!", pDDLinkInfo);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        public ActionResult Get_PDD_Order()
        {
            return View();
        }

        #region 获取公用的商品详情
        public ActionResult GetGoodImageDetail(int User_ID, string SkuID)
        {
            try
            {
                if (User_ID <= 0)
                    return FailNoLogin();

                string[] imageDetailByApi = GetGoodImageDetailByApi(SkuID);
                return SuccessList("获取成功!", imageDetailByApi);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        public ActionResult GetGoodDetailByTB(int User_ID, string SkuID)
        {
            try
            {
                if (User_ID <= 0)
                    return FailNoLogin();

                string resultContent = GetGoodDetailByApi(SkuID);
                return Success("获取成功!", resultContent);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        public ActionResult GetCommonGoodDetail(string CacheKey, string SkuID, int User_ID = 0)
        {
            try
            {
                List<CommonGoodInfoEntity> CommonGoodInfoEntityList = redisCache.Read<List<CommonGoodInfoEntity>>(CacheKey, 7);
                if (CommonGoodInfoEntityList == null)
                    throw new Exception("商品加载出现异常,请返回上一页刷新重试!");
                else
                {
                    string appid = CheckAPPID();

                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);

                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);

                    List<CommonGoodInfoEntity> CommonGoodInfoEntityList_New = CommonGoodInfoEntityList.Where(t => t.skuid == SkuID).ToList();

                    CommonGoodInfoEntity CommonGoodInfoEntity = CheckPreviewOutGood(CacheKey, dm_BasesettingEntity, dm_UserEntity, CommonGoodInfoEntityList_New).FirstOrDefault();

                    if (CommonGoodInfoEntity.PlaformType == 1)
                    {
                        string[] imageDetailByApi = GetGoodImageDetailByApi(SkuID);
                        CommonGoodInfoEntity.detail_images = imageDetailByApi.IsEmpty() ? CommonGoodInfoEntity.detail_images : imageDetailByApi;
                    }

                    if (CommonGoodInfoEntity.IsEmpty())
                        throw new Exception("商品信息加载异常,请重试!");
                    else
                        return Success("获取成功", CommonGoodInfoEntity);
                }
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        public string[] GetGoodImageDetailByApi(string SkuID)
        {
            string cacheKey = "GoodImageDetail" + SkuID;
            string[] imageDetail = redisCache.Read<string[]>(cacheKey, 7);
            if (imageDetail == null)
            {
                #region 获取商品详情图片
                string resultContent = HttpMethods.Get("https://h5api.m.taobao.com/h5/mtop.taobao.detail.getdesc/6.0?api=mtop.taobao.detail.getdesc&v=6.0&jsv=2.4.11&data={%22id%22:%22" + SkuID + "%22}&callback=jsonp_1f7f8047e1fead0");
                #endregion

                List<string> ImageDetailByApi = getValues("(?<=src=\\\\[\'\"]).*?jpg(?=\\\\)", resultContent);

                if (ImageDetailByApi.Count > 0)
                {
                    imageDetail = ImageDetailByApi.ToArray();
                    redisCache.Write<string[]>(cacheKey, imageDetail, DateTime.Now.AddDays(3), 7);
                }
            }

            return imageDetail;
        }

        public string GetGoodDetailByApi(string SkuID)
        {
            string resultContent = "";
            try
            {
                string cacheKey = "TB_GoodDetailApi" + SkuID;
                resultContent = redisCache.Read<string>(cacheKey, 7);
                if (string.IsNullOrWhiteSpace(resultContent))
                {
                    resultContent = HttpMethods.Get("https://h5api.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data=%7B%22itemNumId%22%3A%22" + SkuID + "%22%7D&callback=jsonp_1290ab1b928e600");
                    if (resultContent.Contains("SUCCESS::调用成功"))
                    {
                        redisCache.Write<string>(cacheKey, resultContent, DateTime.Now.AddDays(3), 7);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("当前访问量过大，请稍后重试!");
            }

            return resultContent;
        }

        public List<string> getValues(string parn, string content)
        {
            List<string> values = new List<string>();
            Regex reg = new Regex(parn);
            MatchCollection matchs = reg.Matches(content);
            foreach (Match item in matchs)
            {

                values.Add(GetImage(item.Value));
            }
            return values;
        }
        #endregion

        #region 获取积分兑换商品
        /// <summary>
        /// 获取积分兑换商品
        /// </summary>
        /// <param name="PageNo">页码</param>
        /// <param name="PageSize">每页显示数量</param>
        /// <returns></returns>
        public ActionResult GetIntergralChangeGood(int PageNo, int PageSize)
        {
            try
            {
                string appid = CheckAPPID();
                return SuccessList("获取成功", dM_IntergralChangeGoodIBLL.GetPageListByCache(new Pagination
                {
                    page = PageNo,
                    rows = PageSize
                }, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 申请积分兑换
        public ActionResult ApplyChangeGood(dm_intergralchangerecordEntity dm_IntergralchangerecordEntity)
        {
            try
            {
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(dm_IntergralchangerecordEntity.user_id.ToInt());
                if (dm_UserEntity == null)
                {
                    return Fail("用户信息异常!");
                }
                dM_IntergralChangeRecordIBLL.ApplyChangeGood(dm_IntergralchangerecordEntity, dm_UserEntity);
                return Success("商品兑换成功,我们将会在7个工作日内发货!", new
                {

                });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取我的积分兑换记录
        public ActionResult GetMyIntegralGoodRecord(int user_id, int PageNo = 1, int PageSize = 10)
        {
            try
            {
                if (user_id <= 0)
                    return FailNoLogin();

                string cacheKey = "MyIntegralGoodRecord" + user_id + PageNo.ToString() + PageSize;
                DataTable dataTable = redisCache.Read(cacheKey, 7);
                if (dataTable == null)
                {
                    dataTable = dM_IntergralChangeRecordIBLL.GetMyIntegralGoodRecord(user_id, new Pagination
                    {
                        page = PageNo,
                        rows = PageSize,
                        sidx = "createtime",
                        sord = "desc"
                    });

                    if (dataTable.Rows.Count >= PageSize)
                    {
                        redisCache.Write(cacheKey, dataTable, DateTime.Now.AddDays(1), 7);
                    }
                    else
                    {
                        redisCache.Write(cacheKey, dataTable, DateTime.Now.AddMinutes(2), 7);
                    }
                }

                return SuccessList("获取成功!", dataTable);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 生成淘宝授权地址
        public ActionResult Get_TB_Author_Address(int user_id)
        {
            try
            {
                if (user_id <= 0)
                    return FailNoLogin();

                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);

                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                string authorAddress = "https://oauth.taobao.com/authorize?response_type=code&client_id=" + dm_BasesettingEntity.tb_appkey + "&redirect_uri=" + CommonConfig.tb_auth_address + "&state=" + user_id + "&view=wap";

                return Success("获取成功!", new { authoraddress = authorAddress, isbeian = dm_UserEntity.isrelation_beian, tb_nickname = dm_UserEntity.tb_nickname });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        public string CheckAPPID()
        {
            if (base.Request.Headers["appid"].IsEmpty())
            {
                throw new Exception("缺少参数appid");
            }
            return base.Request.Headers["appid"].ToString();
        }

        #region 商品数据转公用类
        /// <summary>
        /// 京东商品转公用商品类
        /// </summary>
        /// <param name="jFGoodsRespRows"></param>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByJF(IEnumerable<Hyg.Common.JDTools.JDResponse.JFGoodsRespRow> jFGoodsRespRows, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (Hyg.Common.JDTools.JDResponse.JFGoodsRespRow item in jFGoodsRespRows)
            {
                string[] images = item.images;
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.skuId,
                    title = item.skuName,
                    shopId = item.shopId,
                    shopName = item.shopName,
                    coupon_after_price = (item.price - item.discount).ToString(),
                    coupon_price = item.discount.ToString(),
                    origin_price = item.price.ToString(),
                    coupon_end_time = Time.GetDateTimeFrom1970Ticks(item.getEndTime, true).ToString("yyyy-MM-dd HH:mm:ss"),
                    coupon_start_time = Time.GetDateTimeFrom1970Ticks(item.getStartTime, true).ToString("yyyy-MM-dd HH:mm:ss"),
                    detail_images = images,
                    images = images,
                    image = images != null ? images[0] : "",
                    month_sales = item.inOrderCount30Days,
                    TotalCommission = item.discount > 0 ? item.couponCommission : item.commission,
                    PlaformType = 3,
                    afterServiceScore = "4.9",
                    logisticsLvyueScore = "4.85",
                    userEvaluateScore = "4.8",
                    remark = item.brandName,
                    coupon_link = item.link,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByJTT(List<JTT_GoodItemInfo> jTT_GoodItemInfos, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (JTT_GoodItemInfo item in jTT_GoodItemInfos)
            {
                List<string> detailImage = new List<string>();
                List<UrlInfo> urlInfos = JsonConvert.DeserializeObject<List<UrlInfo>>(item.jd_imageList);
                for (int i = 0; i < urlInfos.Count; i++)
                {
                    detailImage.Add(urlInfos[i].url);
                }

                Score score = null;
                if (item.score != null)
                    score = JsonConvert.DeserializeObject<Score>(item.score);

                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goods_id,
                    title = item.goods_name,
                    shopId = item.shopid,
                    shopName = item.shop_name,
                    coupon_after_price = item.coupon_price.ToString(),
                    coupon_price = item.discount_price.ToString(),
                    origin_price = item.goods_price.ToString(),
                    coupon_end_time = Time.GetDateTimeFrom1970Ticks(item.discount_end, true).ToString("yyyy-MM-dd HH:mm:ss"),
                    coupon_start_time = Time.GetDateTimeFrom1970Ticks(item.discount_start, true).ToString("yyyy-MM-dd HH:mm:ss"),
                    detail_images = detailImage.ToArray(),
                    images = JsonConvert.DeserializeObject<string[]>(item.img_list),
                    image = item.goods_img,
                    month_sales = item.inOrderCount30Days,
                    TotalCommission = Math.Round((double)(item.coupon_price * item.commission / 100), 2),
                    PlaformType = 3,
                    afterServiceScore = score == null ? "-" : GetScore(score.afterServiceScore),
                    logisticsLvyueScore = score == null ? "-" : GetScore(score.logisticsLvyueScore),
                    userEvaluateScore = score == null ? "-" : GetScore(score.userEvaluateScore),
                    remark = item.goods_content,
                    coupon_link = item.discount_link,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        /// <summary>
        /// 拼多多商品转公用商品类
        /// </summary>
        /// <param name="goodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByPDD(IEnumerable<GoodItem> goodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (GoodItem item in goodItemList)
            {
                string[] images = new string[] { item.goods_image_url };
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goods_id,
                    title = item.goods_name,
                    shopId = item.mall_id,
                    shopName = item.mall_name,
                    coupon_after_price = Math.Round((item.min_group_price - item.coupon_discount) / 100, 2).ToString(),
                    coupon_price = Math.Round(item.coupon_discount / 100, 2).ToString(),
                    origin_price = Math.Round(item.min_group_price / 100, 2).ToString(),
                    coupon_end_time = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),
                    coupon_start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    detail_images = images,
                    images = images,
                    image = item.goods_thumbnail_url,
                    month_sales = item.coupon_total_quantity,
                    TotalCommission = Math.Round((double)((item.min_group_price - item.coupon_discount) * item.promotion_rate) / 100000, 2),
                    PlaformType = 4,
                    afterServiceScore = GetScore(item.serv_txt),
                    logisticsLvyueScore = GetScore(item.lgst_txt),
                    userEvaluateScore = GetScore(item.desc_txt),
                    remark = item.goods_desc,
                    coupon_link = item.mall_coupon_id,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        /// <summary>
        /// 各大榜单转换
        /// </summary>
        /// <param name="rankingItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByRank(List<RankingItem> rankingItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (RankingItem item in rankingItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goodsId.ToString(),
                    title = item.title,
                    shopId = item.sellerId,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.sellerId,
                    shopName = item.shopName,
                    coupon_after_price = item.actualPrice.ToString(),
                    coupon_price = item.couponPrice.ToString(),
                    origin_price = item.originalPrice.ToString(),
                    coupon_end_time = item.couponEndTime,
                    coupon_start_time = item.couponStartTime,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.mainPic),
                    month_sales = item.monthSales,
                    TotalCommission = Math.Round((double)(item.actualPrice * item.commissionRate) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(null),
                    logisticsLvyueScore = GetScore(null),
                    userEvaluateScore = GetScore(null),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        /// <summary>
        /// 超级搜索接口
        /// </summary>
        /// <param name="superGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityBySuperGood(List<SuperGoodItem> superGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (SuperGoodItem item in superGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goodsId.ToString(),
                    title = item.title,
                    shopId = item.sellerId,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.sellerId,
                    shopName = item.shopName,
                    coupon_after_price = item.actualPrice.ToString(),
                    coupon_price = item.couponPrice.ToString(),
                    origin_price = item.originalPrice.ToString(),
                    coupon_end_time = item.couponEndTime,
                    coupon_start_time = item.couponStartTime,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.mainPic),
                    month_sales = item.monthSales,
                    TotalCommission = Math.Round((double)(item.actualPrice * item.commissionRate) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }
        /// <summary>
        /// 大淘客搜索接口转换
        /// </summary>
        /// <param name="dTK_SearchGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByDTK_SearchGoodItem(List<DTK_SearchGoodItem> dTK_SearchGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (DTK_SearchGoodItem item in dTK_SearchGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goodsId.ToString(),
                    title = item.title,
                    shopId = item.sellerId,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.sellerId,
                    shopName = item.shopName,
                    coupon_after_price = item.actualPrice.ToString(),
                    coupon_price = item.couponPrice.ToString(),
                    origin_price = item.originalPrice.ToString(),
                    coupon_end_time = item.couponEndTime,
                    coupon_start_time = item.couponStartTime,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.mainPic),
                    month_sales = item.monthSales,
                    TotalCommission = Math.Round((double)(item.actualPrice * item.commissionRate) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        /// <summary>
        /// 热门活动商品转换
        /// </summary>
        /// <param name="activityGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByActivityGood(List<ActivityGoodItem> activityGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (ActivityGoodItem item in activityGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goodsId.ToString(),
                    title = item.title,
                    shopId = item.sellerId,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.sellerId,
                    shopName = item.shopName,
                    coupon_after_price = item.actualPrice.ToString(),
                    coupon_price = item.couponPrice.ToString(),
                    origin_price = item.originalPrice.ToString(),
                    coupon_end_time = item.couponEndTime,
                    coupon_start_time = item.couponStartTime,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.mainPic),
                    month_sales = item.monthSales,
                    TotalCommission = Math.Round((double)(item.actualPrice * item.commissionRate) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        /// <summary>
        /// 精选专辑商品
        /// </summary>
        /// <param name="topicGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByTopicGood(List<TopicGoodItem> topicGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (TopicGoodItem item in topicGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goodsId.ToString(),
                    title = item.title,
                    shopId = item.sellerId,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.sellerId,
                    shopName = item.shopName,
                    coupon_after_price = item.actualPrice.ToString(),
                    coupon_price = item.couponPrice.ToString(),
                    origin_price = item.originalPrice.ToString(),
                    coupon_end_time = item.couponEndTime,
                    coupon_start_time = item.couponStartTime,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.mainPic),
                    month_sales = item.monthSales,
                    TotalCommission = Math.Round((double)(item.actualPrice * item.commissionRate) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        /// <summary>
        /// 9.9商品专区
        /// </summary>
        /// <param name="oPGoodItemList"></param>
        /// <param name="dm_UserEntity"></param>
        /// <param name="dm_BasesettingEntity"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByOPGood(List<OPGoodItem> oPGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (OPGoodItem item in oPGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.goodsId.ToString(),
                    title = item.title,
                    shopId = item.sellerId,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.sellerId,
                    shopName = item.shopName,
                    coupon_after_price = item.actualPrice.ToString(),
                    coupon_price = item.couponPrice.ToString(),
                    origin_price = item.originalPrice.ToString(),
                    coupon_end_time = item.couponEndTime,
                    coupon_start_time = item.couponStartTime,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.mainPic),
                    month_sales = item.monthSales,
                    TotalCommission = Math.Round((double)(item.actualPrice * item.commissionRate) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        /// <summary>
        /// 淘宝联盟搜索的商品转换
        /// </summary>
        /// <param name="dTK_TB_Service_GoodItemList"></param>
        /// <param name="dm_UserEntity"></param>
        /// <param name="dm_BasesettingEntity"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        List<CommonGoodInfoEntity> ConvertCommonGoodEntityByTBServiceGood(List<DTK_TB_Service_GoodItem> dTK_TB_Service_GoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (DTK_TB_Service_GoodItem item in dTK_TB_Service_GoodItemList)
            {
                string[] images = new string[] { GetImage(item.pict_url) };
                double coupon_after_price = Math.Round((double.Parse(item.zk_final_price) - double.Parse(item.coupon_amount.IsEmpty() ? "0" : item.coupon_amount)), 2);
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.item_id.ToString(),
                    title = item.title,
                    shopId = item.seller_id,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.seller_id,
                    shopName = item.nick,
                    coupon_after_price = coupon_after_price.ToString(),
                    coupon_price = item.coupon_amount.ToString(),
                    origin_price = item.zk_final_price,
                    coupon_end_time = item.coupon_end_time,
                    coupon_start_time = item.coupon_start_time,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.pict_url),
                    month_sales = item.volume,
                    TotalCommission = Math.Round((coupon_after_price * double.Parse(item.commission_rate)) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.shop_dsr.ToString()),
                    logisticsLvyueScore = GetScore(item.shop_dsr.ToString()),
                    userEvaluateScore = GetScore(item.shop_dsr.ToString()),
                    remark = item.short_title,
                    coupon_link = item.coupon_id,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        List<CommonGoodInfoEntity> GuessGoodConvert(List<TbkDgOptimusMaterialResponse.MapDataDomain> resultList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfoEntity> CommonGoodInfoEntityList = new List<CommonGoodInfoEntity>();
            foreach (TbkDgOptimusMaterialResponse.MapDataDomain item in resultList)
            {
                string[] images = GetImages(item.SmallImages);
                double coupon_after_price = Math.Round((double.Parse(item.ZkFinalPrice) - double.Parse(item.CouponAmount <= 0 ? "0" : item.CouponAmount.ToString())), 2);
                CommonGoodInfoEntityList.Add(new CommonGoodInfoEntity
                {
                    skuid = item.ItemId.ToString(),
                    title = item.Title,
                    shopId = item.SellerId.ToString(),
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.SellerId.ToString(),
                    shopName = item.ShopTitle,
                    coupon_after_price = coupon_after_price.ToString(),
                    coupon_price = item.CouponAmount.ToString(),
                    origin_price = item.ZkFinalPrice,
                    coupon_end_time = item.CouponEndTime.IsEmpty() ? "" : Time.GetDateTimeFrom1970Ticks(long.Parse(item.CouponEndTime), true).ToString(),
                    coupon_start_time = item.CouponStartTime.IsEmpty() ? "" : Time.GetDateTimeFrom1970Ticks(long.Parse(item.CouponStartTime), true).ToString(),
                    detail_images = images,
                    images = images,
                    image = GetImage(item.PictUrl),
                    month_sales = item.Volume,
                    TotalCommission = Math.Round((coupon_after_price * double.Parse(item.CommissionRate)) / 100, 2),
                    PlaformType = 1,
                    afterServiceScore = GetScore(null),
                    logisticsLvyueScore = GetScore(null),
                    userEvaluateScore = GetScore(null),
                    remark = item.ShortTitle,
                    coupon_link = item.CouponClickUrl,
                    cacheKey = cacheKey
                });
            }

            return CommonGoodInfoEntityList;
        }

        string GetScore(string score)
        {
            if (score == null)
            {
                string[] scoreList = new string[] { "4.8", "4.9", "5.0" };
                return scoreList[new Random().Next(scoreList.Length)];
            }
            else
            {
                decimal scoreValue = 0;

                if (decimal.TryParse(score, out scoreValue))
                {
                    if (scoreValue <= 0)
                        return "低";
                    else if (scoreValue >= 10)
                        return "高";
                    else
                        return score;
                }
                else
                {
                    return score;
                }
            }
        }

        string GetImage(string img)
        {
            if (!img.StartsWith("http"))
            {
                img = "http:" + img;
            }
            return img;
        }

        string[] GetImages(List<string> Images)
        {
            List<string> imageList = new List<string>();
            for (int i = 0; i < Images.Count; i++)
            {
                imageList.Add(GetImage(Images[i]));
            }

            return imageList.ToArray();
        }
        #endregion

        #region 正则解析获取商品id
        string GetNumid(string keyWord)
        {
            string pwdContent = "";
            try
            {
                #region 正则获取链接中的商品id
                pwdContent = getValue("(?<=(id=))[0-9]{5,}", keyWord);//识别出来的商品ID
                if (string.IsNullOrWhiteSpace(pwdContent))
                {
                    //string pwdReg = @"[^a-zA-Z=/\d@<\u4E00-\u9FA5]([a-zA-Z0-9]{11})[^a-zA-Z=.\d@>\u4E00-\u9FA5]";
                    string pwdReg = @"((^s*)|[^a-zA-Z])([a-zA-Z0-9]{11})($|[^a-zA-Z])";
                    pwdContent = getValue(pwdReg, keyWord);
                    if (pwdContent != "")
                    {
                        #region 解析淘口令
                        pwdContent = ParsePwd("http://cloud.jinglm.com/TB_OpenApi/TKL_Query", string.Format("PasswordContent={0}&AdzoneId={1}&SiteId={2}&AccountID={3}", keyWord, "110249350265", "967450311", "127267155"));
                        #endregion
                        if (!pwdContent.IsEmpty())
                            pwdContent = "https://item.taobao.com/item.htm?id=" + pwdContent;
                    }
                }
                else
                {
                    pwdContent = "https://item.taobao.com/item.htm?id=" + pwdContent;
                    Regex tb_regex = new Regex(@"^(http|https)(://detail.tmall.com|://item.taobao.com|://chaoshi.detail.tmall.com)");
                    if (!tb_regex.IsMatch(keyWord))
                    {
                        pwdContent = "";
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                pwdContent = "";
            }

            if (pwdContent == "")
                return new string[] { "数码家电", "家装家纺", "文娱车品" }.Contains(keyWord) ? keyWord.Substring(0, 1) : keyWord;
            else
                return pwdContent;
        }

        public string getValue(string parn, string content)
        {
            Regex reg = new Regex(parn);
            return reg.Match(content).Value;
        }

        HttpWebResponse response = null;
        public string ParsePwd(string Url, string postDataStr)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = postDataStr.Length;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

                request.Headers.Add("apikey", "a5a553f7d21647a5a95bef69546894f3");


                byte[] byteData = Encoding.UTF8.GetBytes(postDataStr);
                int length = byteData.Length;
                request.ContentLength = length;
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();

                response = (HttpWebResponse)request.GetResponse();

                string encoding = response.ContentEncoding; if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8";
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retstring = reader.ReadToEnd();

                PwdResult pwdResult = JsonConvert.DeserializeObject<PwdResult>(retstring);

                return pwdResult.data.Data.NumIid;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region 审核模式判断
        bool IsPreview(dm_basesettingEntity dm_BasesettingEntity)
        {
            if (dm_BasesettingEntity.openchecked == "1")
            { //开启审核模式
                string version = CheckVersion();
                string platform = CheckPlaform();
                if ((platform == "ios" && version == dm_BasesettingEntity.previewversion) || (platform == "android" && version == dm_BasesettingEntity.previewversionandroid))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public string CheckPlaform()
        {
            if (base.Request.Headers["platform"].IsEmpty())
            {
                throw new Exception("缺少参数platform");
            }
            return base.Request.Headers["platform"].ToString();
        }

        public string CheckVersion()
        {
            if (base.Request.Headers["version"].IsEmpty())
            {
                throw new Exception("缺少参数version");
            }
            return base.Request.Headers["version"].ToString();
        }
        #endregion

        #region 检测审核并输出商品
        /// <summary>
        /// 检测审核并输出商品
        /// </summary>
        IEnumerable<CommonGoodInfoEntity> CheckPreviewOutGood(string cacheKey, dm_basesettingEntity dm_BasesettingEntity, dm_userEntity dm_UserEntity, List<CommonGoodInfoEntity> superGoodItems)
        {
            IEnumerable<CommonGoodInfoEntity> newGoodItemList = null;
            if (!superGoodItems.IsEmpty() && superGoodItems.Count > 0)
            {
                if (IsPreview(dm_BasesettingEntity))
                {
                    newGoodItemList = superGoodItems.Select(p => { p.SuperCommission = 0M; p.LevelCommission = 0M; p.cacheKey = cacheKey; return p; });
                }
                else
                {
                    int? userLevel = dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel;
                    double userRate = 0.00;
                    switch (userLevel)
                    {
                        case 0:
                            userRate = dm_BasesettingEntity.shopping_pay_junior;
                            break;
                        case 1:
                            userRate = dm_BasesettingEntity.shopping_pay_middle;
                            break;
                        case 2:
                            userRate = dm_BasesettingEntity.shopping_pay_senior;
                            break;
                    }

                    newGoodItemList = superGoodItems.Select(p => { p.SuperCommission = ConvertDecimal(p.TotalCommission * dm_BasesettingEntity.shopping_pay_senior); p.LevelCommission = ConvertDecimal(p.TotalCommission * userRate); p.cacheKey = cacheKey; return p; });
                }
            }

            return newGoodItemList;
        }

        decimal ConvertDecimal(double comission)
        {
            return Math.Round((decimal)comission / 100, 2);
        }
        #endregion
    }

    public class PwdResult
    {
        public int code { get; set; }
        public string info { get; set; }
        public PwdData data { get; set; }
    }

    public class PwdData
    {
        public PwdDataDetail Data { get; set; }
    }

    public class PwdDataDetail
    {
        public string ClickUrl { get; set; }
        public string NumIid { get; set; }
        public string OriginPid { get; set; }
        public string OriginUrl { get; set; }
    }
}
