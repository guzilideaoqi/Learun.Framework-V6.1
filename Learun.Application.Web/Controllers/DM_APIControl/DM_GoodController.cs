using DaTaoKe;
using HYG.DTK.Helper.DTKRequest;
using HYG.DTK.Helper.DTKResponse;
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
using HYG.CommonHelper.JDModel;
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
        public ActionResult GetSubGoodType(int cid)
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
                    dTK_Super_CategoryRequest.version = "v1.1.0";
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
                    dTK_Top100_Request.version = "v1.0.1";
                    DTK_Top100_Response dTK_Top100_Response = dTK_ApiManage.GetTop100(dTK_Top100_Request);
                    if (dTK_Top100_Response.code != 0)
                    {
                        return Fail(dTK_Top100_Response.msg);
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
                string cacheKey = "RankingList" + RandType.ToString();
                List<CommonGoodInfo> RankingList = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);
                if (RankingList == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);

                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Ranking_ListRequest dTK_Ranking_ListRequest = new DTK_Ranking_ListRequest();
                    dTK_Ranking_ListRequest.version = "v1.1.2";
                    dTK_Ranking_ListRequest.rankType = RandType;
                    //dTK_Ranking_ListRequest.cid = cateid;
                    DTK_Ranking_ListResponse dTK_Ranking_ListResponse = dTK_ApiManage.GetRankingList(dTK_Ranking_ListRequest);
                    if (dTK_Ranking_ListResponse.code != 0)
                    {
                        return Fail(dTK_Ranking_ListResponse.msg);
                    }
                    RankingList = ConvertCommonGoodEntityByRank(dTK_Ranking_ListResponse.data, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, RankingList, DateTime.Now.AddHours(4.0), 7L);
                }

                IEnumerable<CommonGoodInfo> rankItemList = null;
                if (RankingList != null)
                {
                    rankItemList = RankingList.Skip((PageNo - 1) * PageSize).Take(PageSize);
                }

                return SuccessList("获取成功!", rankItemList);
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
                        SubTitle = "为您推荐",
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
        public ActionResult GetTodayGood(int User_ID = 0)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = "TodayGood";
                List<CommonGoodInfo> RankingList = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);
                if (RankingList == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);

                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);

                    if (true)
                    {//数据切换  此处先定死
                        DTK_OP_ListRequest dTK_OP_ListRequest = new DTK_OP_ListRequest();
                        dTK_OP_ListRequest.version = "v1.2.2";
                        dTK_OP_ListRequest.nineCid = "1";
                        dTK_OP_ListRequest.pageId = "1";
                        dTK_OP_ListRequest.pageSize = 10;
                        DTK_OP_ListResponse dTK_OP_ListResponse = dTK_ApiManage.GetOPGood(dTK_OP_ListRequest);

                        if (dTK_OP_ListResponse.code != 0)
                        {
                            return Fail(dTK_OP_ListResponse.msg);
                        }


                        RankingList = ConvertCommonGoodEntityByOPGood(dTK_OP_ListResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    else
                    {
                        DTK_Ranking_ListRequest dTK_Ranking_ListRequest = new DTK_Ranking_ListRequest();
                        dTK_Ranking_ListRequest.version = "v1.1.2";
                        dTK_Ranking_ListRequest.rankType = 7;
                        DTK_Ranking_ListResponse dTK_Ranking_ListResponse = dTK_ApiManage.GetRankingList(dTK_Ranking_ListRequest);
                        if (dTK_Ranking_ListResponse.code != 0)
                        {
                            return Fail(dTK_Ranking_ListResponse.msg);
                        }
                        RankingList = ConvertCommonGoodEntityByRank(dTK_Ranking_ListResponse.data, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }



                    redisCache.Write(cacheKey, RankingList, DateTime.Now.AddHours(2.0), 7L);
                }
                return SuccessList("获取成功!", RankingList);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 三合一超级搜索接口
        public ActionResult CommonSearchGood(int user_id = 0, int PlaformType = 1, int PageNo = 1, int PageSize = 10, string KeyWords = "20", int sort = 0)
        {
            try
            {
                //KeyWords = KeyWords == "" ? "潮流" : KeyWords.Substring(0, 1);
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("CommonSearchGood" + PlaformType + PageNo + PageSize + KeyWords + sort);
                List<CommonGoodInfo> superGoodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);
                if (superGoodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                    if (PlaformType == 1)
                    {
                        DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);

                        if (dm_BasesettingEntity.goodsource == 0)
                        {
                            DTK_TB_Service_GoodRequest dTK_TB_Service_GoodRequest = new DTK_TB_Service_GoodRequest();
                            dTK_TB_Service_GoodRequest.version = "v2.0.0";
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
                            dTK_Super_GoodRequest.version = "v1.2.1";
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
                                return Fail(dTK_Super_GoodResponse.msg);
                            }
                            superGoodItems = ConvertCommonGoodEntityBySuperGood(dTK_Super_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                        }
                    }
                    else if (PlaformType == 3)
                    {
                        JDApi jDApi = new JDApi(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);
                        superGoodItems = ConvertCommonGoodEntityByJF(jDApi.GetJTT_SearchGoodItemList(KeyWords, PageNo, PageSize, 0, 0, GetSort(PlaformType, sort)), dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    else if (PlaformType == 4)
                    {
                        PDDApi pDDApi = new PDDApi(dm_BasesettingEntity.pdd_clientid, dm_BasesettingEntity.pdd_clientsecret, "");
                        superGoodItems = ConvertCommonGoodEntityByPDD(pDDApi.SearchGood(KeyWords, PageNo, PageSize, int.Parse(GetSort(PlaformType, sort)), false, -1), dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    if (superGoodItems.Count > 0)
                        redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }

                return SuccessList("获取成功", superGoodItems);
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
                List<CommonGoodInfo> superGoodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);


                if (superGoodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Super_GoodRequest dTK_Super_GoodRequest = new DTK_Super_GoodRequest();
                    dTK_Super_GoodRequest.version = "v1.2.1";
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
                        return Fail(dTK_Super_GoodResponse.msg);
                    }
                    superGoodItems = ConvertCommonGoodEntityBySuperGood(dTK_Super_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }


                return SuccessList("获取成功!", superGoodItems);
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
                List<CommonGoodInfo> superGoodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);


                if (superGoodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Get_dtk_Search_GoodRequest dtk_Get_dtk_Search_GoodRequest = new DTK_Get_dtk_Search_GoodRequest();
                    dtk_Get_dtk_Search_GoodRequest.version = "v2.1.2";
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
                        return Fail(dTK_Get_dtk_Search_GoodResponse.msg);
                    }
                    superGoodItems = ConvertCommonGoodEntityByDTK_SearchGoodItem(dTK_Get_dtk_Search_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }


                return SuccessList("获取成功!", superGoodItems);
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
                string cacheKey = Md5Helper.Hash("GetRecommendGoodByTB" + ChannelType + PageNo + PageSize);
                List<CommonGoodInfo> superGoodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);

                if (superGoodItems == null)
                {
                    #region 获取关键词
                    List<CategoryItem> categoryItems = redisCache.Read<List<CategoryItem>>("SuperCategory", 7L);
                    #endregion

                    string appid = CheckAPPID();
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);
                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);

                    if (ChannelType == "99zhuanqu")//使用联盟搜索
                    {
                        CategoryItem categoryItem = categoryItems.Where(t => t.cid == dm_BasesettingEntity.goodtype).FirstOrDefault();

                        int? min_price = dm_BasesettingEntity.min_price, max_price = dm_BasesettingEntity.max_price, min_tk_rate = dm_BasesettingEntity.min_tk_rate, max_tk_rate = dm_BasesettingEntity.max_tk_rate;

                        DTK_TB_Service_GoodRequest dTK_TB_Service_GoodRequest = new DTK_TB_Service_GoodRequest();
                        dTK_TB_Service_GoodRequest.version = "v2.0.0";
                        dTK_TB_Service_GoodRequest.pageNo = PageNo;
                        dTK_TB_Service_GoodRequest.pageSize = PageSize;
                        dTK_TB_Service_GoodRequest.keyWords = GetNumid(categoryItem.IsEmpty() ? "潮" : categoryItem.cname.Substring(0, 1));
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
                            return Fail(dTK_TB_Service_GoodResponse.msg);
                        superGoodItems = ConvertCommonGoodEntityByTBServiceGood(dTK_TB_Service_GoodResponse.data, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }
                    else if (ChannelType == "chaojiquan")//使用大淘客搜索
                    {
                        CategoryItem categoryItem = categoryItems.Where(t => t.cid == dm_BasesettingEntity.super_coupon_goodtype).FirstOrDefault();
                        DTK_Get_dtk_Search_GoodRequest dtk_Get_dtk_Search_GoodRequest = new DTK_Get_dtk_Search_GoodRequest();
                        dtk_Get_dtk_Search_GoodRequest.version = "v2.1.2";
                        dtk_Get_dtk_Search_GoodRequest.pageId = PageNo.ToString();
                        dtk_Get_dtk_Search_GoodRequest.pageSize = PageSize;
                        dtk_Get_dtk_Search_GoodRequest.keyWords = GetNumid(categoryItem.IsEmpty() ? "潮" : categoryItem.cname);
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
                            return Fail(dTK_Get_dtk_Search_GoodResponse.msg);
                        }

                        superGoodItems = ConvertCommonGoodEntityByDTK_SearchGoodItem(dTK_Get_dtk_Search_GoodResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    }

                    if (superGoodItems.Count > 0)
                        redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }

                return SuccessList("获取成功!", superGoodItems);
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
                List<CommonGoodInfo> oPGoodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);

                if (oPGoodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_OP_ListRequest dTK_OP_ListRequest = new DTK_OP_ListRequest();
                    dTK_OP_ListRequest.version = "v1.1.0";
                    dTK_OP_ListRequest.nineCid = nineCid;
                    dTK_OP_ListRequest.pageId = pageId;
                    dTK_OP_ListRequest.pageSize = pageSize;
                    DTK_OP_ListResponse dTK_OP_ListResponse = dTK_ApiManage.GetOPGood(dTK_OP_ListRequest);
                    if (dTK_OP_ListResponse.code != 0)
                    {
                        return Fail(dTK_OP_ListResponse.msg);
                    }
                    oPGoodItems = ConvertCommonGoodEntityByOPGood(dTK_OP_ListResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, oPGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }


                return SuccessList("获取成功!", oPGoodItems);
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
                    dTK_Search_SuggestionRequest.version = "v1.0.2";
                    dTK_Search_SuggestionRequest.type = type;
                    dTK_Search_SuggestionRequest.keyWords = keyWords;
                    DTK_Search_SuggestionResponse dTK_Search_SuggestionResponse = dTK_ApiManage.GetSearchSuggestion(dTK_Search_SuggestionRequest);
                    if (dTK_Search_SuggestionResponse.code != 0)
                    {
                        return Fail(dTK_Search_SuggestionResponse.msg);
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
                    dTK_Activity_CatalogueRequest.version = "v1.1.0";
                    DTK_Activity_CatalogueResponse dTK_Activity_CatalogueResponse = dTK_ApiManage.GetActivityCatalogueList(dTK_Activity_CatalogueRequest);
                    if (dTK_Activity_CatalogueResponse.code != 0)
                    {
                        return Fail(dTK_Activity_CatalogueResponse.msg);
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
        public ActionResult GetActivityGoodList(int user_id, int activityId, string pageId = "1", int pageSize = 20, int cid = 1)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("ActivityGoodList" + activityId + pageId + pageSize + cid);
                List<CommonGoodInfo> activityGoodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);

                if (activityGoodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Activity_GoodListRequest dTK_Activity_GoodListRequest = new DTK_Activity_GoodListRequest();
                    dTK_Activity_GoodListRequest.version = "v1.2.0";
                    dTK_Activity_GoodListRequest.pageId = pageId;
                    dTK_Activity_GoodListRequest.pageSize = pageSize;
                    dTK_Activity_GoodListRequest.cid = cid;
                    dTK_Activity_GoodListRequest.activityId = activityId;
                    DTK_Activity_GoodListResponse dTK_Activity_GoodListResponse = dTK_ApiManage.GetActivityGoodList(dTK_Activity_GoodListRequest);
                    if (dTK_Activity_GoodListResponse.code != 0)
                    {
                        return Fail(dTK_Activity_GoodListResponse.msg);
                    }
                    activityGoodItems = ConvertCommonGoodEntityByActivityGood(dTK_Activity_GoodListResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, activityGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }
                return SuccessList("获取成功!", activityGoodItems);
            }
            catch (Exception ex)
            {
                return FailException(ex);
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
                    dTK_Topic_CatalogueRequest.version = "v1.1.0";
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
                List<CommonGoodInfo> topicGoodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);

                if (topicGoodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
                    DTK_Topic_GoodListRequest dTK_Topic_GoodListRequest = new DTK_Topic_GoodListRequest();
                    dTK_Topic_GoodListRequest.version = "v1.2.0";
                    dTK_Topic_GoodListRequest.pageId = pageId;
                    dTK_Topic_GoodListRequest.pageSize = pageSize;
                    dTK_Topic_GoodListRequest.topicId = topicId;
                    DTK_Topic_GoodListResponse dTK_Topic_GoodListResponse = dTK_ApiManage.GetTopicGoodList(dTK_Topic_GoodListRequest);
                    if (dTK_Topic_GoodListResponse.code != 0)
                    {
                        return Fail(dTK_Topic_GoodListResponse.msg);
                    }
                    topicGoodItems = ConvertCommonGoodEntityByTopicGood(dTK_Topic_GoodListResponse.data.list, dm_UserEntity, dm_BasesettingEntity, cacheKey);
                    redisCache.Write(cacheKey, topicGoodItems, DateTime.Now.AddHours(2.0), 7L);
                }

                return SuccessList("获取成功!", topicGoodItems);
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
                    dTK_TB_Topic_ListRequest.version = "v1.2.0";
                    dTK_TB_Topic_ListRequest.type = type;
                    dTK_TB_Topic_ListRequest.pageId = pageId;
                    dTK_TB_Topic_ListRequest.pageSize = pageSize;
                    dTK_TB_Topic_ListRequest.channelID = long.Parse(dm_UserEntity.tb_relationid.ToString());
                    DTK_TB_Topic_ListResponse dTK_TB_Topic_ListResponse = dTK_ApiManage.GettTBTopicList(dTK_TB_Topic_ListRequest);
                    if (dTK_TB_Topic_ListResponse.code != 0)
                    {
                        return Fail(dTK_TB_Topic_ListResponse.msg);
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
                    dTK_Privilege_LinkRequest.version = "v1.1.1";
                    dTK_Privilege_LinkRequest.goodsId = originid;
                    dTK_Privilege_LinkRequest.pid = dm_BasesettingEntity.tb_relation_pid;
                    dTK_Privilege_LinkRequest.channelId = dm_UserEntity.tb_relationid.ToString();
                    dTK_Privilege_LinkRequest.couponId = couponid;
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
                dTK_Get_Good_DetailsRequest.version = "v1.2.2";
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
                req.InviterCode = "PCUMX2";
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
                List<CommonGoodInfo> jFGoodsRespRows = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);
                if (jFGoodsRespRows == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);


                    JDApi jDApi = new JDApi(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);
                    jFGoodsRespRows = ConvertCommonGoodEntityByJF(jDApi.GetGoodList(eliteId, pageIndex, pageSize, sortname, sort), dm_UserEntity, dm_BasesettingEntity, cacheKey);
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

                return SuccessList("获取成功!", jFGoodsRespRows);
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
                    JDApi jDApi = new JDApi(dm_BasesettingEntity.jd_appkey, dm_BasesettingEntity.jd_appsecret, dm_BasesettingEntity.jd_sessionkey);

                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    if (dm_UserEntity.jd_pid.IsEmpty())
                    {
                        #region 自动分配京东pid
                        dm_UserEntity = dM_PidIBLL.AutoAssignJDPID(dm_UserEntity);
                        #endregion
                    }

                    couponlink = HttpUtility.UrlEncode(couponlink);
                    jDLinkInfo = jDApi.ConvertUrl(skuid, dm_UserEntity.jd_site.ToString(), dm_UserEntity.jd_pid, couponlink);

                    if (jDLinkInfo != null)
                    {
                        jDLinkInfo.tpwd = "下单链接:" + jDLinkInfo.clickURL;
                        redisCache.Write(cacheKey, jDLinkInfo, DateTime.Now.AddHours(2.0), 7L);
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
                List<CommonGoodInfo> jFGoodsRespRows = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);
                if (jFGoodsRespRows == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);


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

                return SuccessList("获取成功!", jFGoodsRespRows, new { RequestDetailID = cacheKey });
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
        public ActionResult Get_PDD_GoodList(int user_id = 0, string keyWord = "女装", int pageIndex = 1, int pageSize = 20, int sort = 0, bool with_coupon = false, int cat_id = 1)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("PDDGoodList" + keyWord + pageIndex + pageSize + sort + with_coupon + cat_id);
                List<CommonGoodInfo> goodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);
                if (goodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                    PDDApi pDDApi = new PDDApi(dm_BasesettingEntity.pdd_clientid, dm_BasesettingEntity.pdd_clientsecret, "");
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

                return SuccessList("获取成功!", goodItems);
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
                List<CommonGoodInfo> goodItems = redisCache.Read<List<CommonGoodInfo>>(cacheKey, 7L);
                if (goodItems == null)
                {
                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                    dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

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

                return SuccessList("获取成功!", goodItems);
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

        public ActionResult GetCommonGoodDetail(string CacheKey, string SkuID)
        {
            try
            {
                List<CommonGoodInfo> commonGoodInfoList = redisCache.Read<List<CommonGoodInfo>>(CacheKey, 7);
                if (commonGoodInfoList == null)
                    return Fail("商品加载出现异常,请返回上一页刷新重试!");
                else
                {
                    CommonGoodInfo commonGoodInfo = commonGoodInfoList.Where(t => t.skuid == SkuID).FirstOrDefault();

                    if (commonGoodInfo.PlaformType == 1)
                    {
                        string[] imageDetailByApi = GetGoodImageDetailByApi(SkuID);
                        commonGoodInfo.detail_images = imageDetailByApi.IsEmpty() ? commonGoodInfo.detail_images : imageDetailByApi;
                    }


                    if (commonGoodInfo.IsEmpty())
                        return Fail("商品信息加载异常,请重试!");
                    else
                        return Success("获取成功", commonGoodInfo);
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

        #region 获取佣金比例
        /// <summary>
        /// 淘宝
        /// </summary>
        /// <param name="zkprice"></param>
        /// <param name="commission"></param>
        /// <param name="userlevel"></param>
        /// <param name="dm_BasesettingEntity"></param>
        /// <returns></returns>
        decimal GetCommissionRate(decimal zkprice, decimal commission, int? userlevel, dm_basesettingEntity dm_BasesettingEntity)
        {
            decimal userComission = 0.00M;
            if (!IsPreview(dm_BasesettingEntity))
            {
                switch (userlevel)
                {
                    case 0:
                        userComission = Math.Round(zkprice * commission * dm_BasesettingEntity.shopping_pay_junior / 10000, 2);
                        break;
                    case 1:
                        userComission = Math.Round(zkprice * commission * dm_BasesettingEntity.shopping_pay_middle / 10000, 2);
                        break;
                    case 2:
                        userComission = Math.Round(zkprice * commission * dm_BasesettingEntity.shopping_pay_senior / 10000, 2);
                        break;
                }
            }
            return userComission;
        }

        decimal GetJDCommissionRate(double comission, int? userlevel, dm_basesettingEntity dm_BasesettingEntity)
        {
            double userComission = 0.00;
            if (!IsPreview(dm_BasesettingEntity))
            {
                switch (userlevel)
                {
                    case 0:
                        userComission = Math.Round(comission * dm_BasesettingEntity.shopping_pay_junior / 100, 2);
                        break;
                    case 1:
                        userComission = Math.Round(comission * dm_BasesettingEntity.shopping_pay_middle / 100, 2);
                        break;
                    case 2:
                        userComission = Math.Round(comission * dm_BasesettingEntity.shopping_pay_senior / 100, 2);
                        break;
                }
            }

            return Convert.ToDecimal(userComission);
        }

        decimal GetPDDCommissionRate(decimal zkprice, decimal commission, int? userlevel, dm_basesettingEntity dm_BasesettingEntity)
        {
            decimal userComission = 0.00M;
            if (!IsPreview(dm_BasesettingEntity))
            {
                switch (userlevel)
                {
                    case 0:
                        userComission = Math.Round(zkprice * commission * dm_BasesettingEntity.shopping_pay_junior / 10000000, 2);
                        break;
                    case 1:
                        userComission = Math.Round(zkprice * commission * dm_BasesettingEntity.shopping_pay_middle / 10000000, 2);
                        break;
                    case 2:
                        userComission = Math.Round(zkprice * commission * dm_BasesettingEntity.shopping_pay_senior / 10000000, 2);
                        break;
                }
            }
            return userComission;
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

                string authorAddress = "https://oauth.taobao.com/authorize?response_type=code&client_id=" + dm_BasesettingEntity.tb_appkey + "&redirect_uri=http://wx.sqgsq.cn/TBUserInfoController/AuthorCallBack&state=" + user_id + "&view=wap";

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
        List<CommonGoodInfo> ConvertCommonGoodEntityByJF(IEnumerable<JFGoodsRespRow> jFGoodsRespRows, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (JFGoodsRespRow item in jFGoodsRespRows)
            {
                string[] images = item.images;
                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetJDCommissionRate(item.couponCommission, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetJDCommissionRate(item.couponCommission, 2, dm_BasesettingEntity),
                    PlaformType = 3,
                    afterServiceScore = "4.9",
                    logisticsLvyueScore = "4.85",
                    userEvaluateScore = "4.8",
                    remark = item.brandName,
                    coupon_link = item.link,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        List<CommonGoodInfo> ConvertCommonGoodEntityByJTT(List<JTT_GoodItemInfo> jTT_GoodItemInfos, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
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

                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetJDCommissionRate(Math.Round((double)(item.coupon_price * item.commission / 100), 2), dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetJDCommissionRate(Math.Round((double)(item.coupon_price * item.commission / 100), 2), 2, dm_BasesettingEntity),
                    PlaformType = 3,
                    afterServiceScore = score == null ? "-" : GetScore(score.afterServiceScore),
                    logisticsLvyueScore = score == null ? "-" : GetScore(score.logisticsLvyueScore),
                    userEvaluateScore = score == null ? "-" : GetScore(score.userEvaluateScore),
                    remark = item.goods_content,
                    coupon_link = item.discount_link,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        /// <summary>
        /// 拼多多商品转公用商品类
        /// </summary>
        /// <param name="goodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityByPDD(IEnumerable<GoodItem> goodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (GoodItem item in goodItemList)
            {
                string[] images = new string[] { item.goods_image_url };
                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetPDDCommissionRate(item.min_group_price - item.coupon_discount, item.promotion_rate, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetPDDCommissionRate(item.min_group_price - item.coupon_discount, item.promotion_rate, 2, dm_BasesettingEntity),
                    PlaformType = 4,
                    afterServiceScore = GetScore(item.serv_txt),
                    logisticsLvyueScore = GetScore(item.lgst_txt),
                    userEvaluateScore = GetScore(item.desc_txt),
                    remark = item.goods_desc,
                    coupon_link = item.mall_coupon_id,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        /// <summary>
        /// 各大榜单转换
        /// </summary>
        /// <param name="rankingItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityByRank(List<RankingItem> rankingItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (RankingItem item in rankingItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                commonGoodInfoList.Add(new CommonGoodInfo
                {
                    skuid = item.goodsId.ToString(),
                    title = item.title,
                    shopId = item.sellerId,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.sellerId,
                    shopName = item.guideName,
                    coupon_after_price = item.actualPrice.ToString(),
                    coupon_price = item.couponPrice.ToString(),
                    origin_price = item.originalPrice.ToString(),
                    coupon_end_time = item.couponEndTime,
                    coupon_start_time = item.couponStartTime,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.mainPic),
                    month_sales = item.monthSales,
                    LevelCommission = GetCommissionRate(item.actualPrice, item.commissionRate, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetCommissionRate(item.actualPrice, item.commissionRate, 2, dm_BasesettingEntity),
                    PlaformType = 1,
                    afterServiceScore = GetScore(null),
                    logisticsLvyueScore = GetScore(null),
                    userEvaluateScore = GetScore(null),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        /// <summary>
        /// 超级搜索接口
        /// </summary>
        /// <param name="superGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityBySuperGood(List<SuperGoodItem> superGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (SuperGoodItem item in superGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetCommissionRate(item.actualPrice, item.commissionRate, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetCommissionRate(item.actualPrice, item.commissionRate, 2, dm_BasesettingEntity),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }
        /// <summary>
        /// 大淘客搜索接口转换
        /// </summary>
        /// <param name="dTK_SearchGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityByDTK_SearchGoodItem(List<DTK_SearchGoodItem> dTK_SearchGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (DTK_SearchGoodItem item in dTK_SearchGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetCommissionRate(item.actualPrice, item.commissionRate, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetCommissionRate(item.actualPrice, item.commissionRate, 2, dm_BasesettingEntity),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        /// <summary>
        /// 热门活动商品转换
        /// </summary>
        /// <param name="activityGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityByActivityGood(List<ActivityGoodItem> activityGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (ActivityGoodItem item in activityGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetCommissionRate(item.actualPrice, item.commissionRate, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetCommissionRate(item.actualPrice, item.commissionRate, 2, dm_BasesettingEntity),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        /// <summary>
        /// 精选专辑商品
        /// </summary>
        /// <param name="topicGoodItemList"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityByTopicGood(List<TopicGoodItem> topicGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (TopicGoodItem item in topicGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetCommissionRate(item.actualPrice, item.commissionRate, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetCommissionRate(item.actualPrice, item.commissionRate, 2, dm_BasesettingEntity),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        /// <summary>
        /// 9.9商品专区
        /// </summary>
        /// <param name="oPGoodItemList"></param>
        /// <param name="dm_UserEntity"></param>
        /// <param name="dm_BasesettingEntity"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityByOPGood(List<OPGoodItem> oPGoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (OPGoodItem item in oPGoodItemList)
            {
                string[] images = new string[] { GetImage(item.mainPic) };
                commonGoodInfoList.Add(new CommonGoodInfo
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
                    LevelCommission = GetCommissionRate(item.actualPrice, item.commissionRate, dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetCommissionRate(item.actualPrice, item.commissionRate, 2, dm_BasesettingEntity),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.serviceScore.ToString()),
                    logisticsLvyueScore = GetScore(item.serviceScore.ToString()),
                    userEvaluateScore = GetScore(item.descScore.ToString()),
                    remark = item.desc,
                    coupon_link = item.couponLink,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
        }

        /// <summary>
        /// 淘宝联盟搜索的商品转换
        /// </summary>
        /// <param name="dTK_TB_Service_GoodItemList"></param>
        /// <param name="dm_UserEntity"></param>
        /// <param name="dm_BasesettingEntity"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        List<CommonGoodInfo> ConvertCommonGoodEntityByTBServiceGood(List<DTK_TB_Service_GoodItem> dTK_TB_Service_GoodItemList, dm_userEntity dm_UserEntity, dm_basesettingEntity dm_BasesettingEntity, string cacheKey)
        {
            List<CommonGoodInfo> commonGoodInfoList = new List<CommonGoodInfo>();
            foreach (DTK_TB_Service_GoodItem item in dTK_TB_Service_GoodItemList)
            {
                string[] images = new string[] { GetImage(item.pict_url) };
                commonGoodInfoList.Add(new CommonGoodInfo
                {
                    skuid = item.item_id.ToString(),
                    title = item.title,
                    shopId = item.seller_id,
                    shopLogo = "https://wwc.alicdn.com/avatar/getAvatar.do?userId=" + item.seller_id,
                    shopName = item.nick,
                    coupon_after_price = (decimal.Parse(item.zk_final_price) - decimal.Parse(item.coupon_amount.IsEmpty() ? "0" : item.coupon_amount)).ToString(),
                    coupon_price = item.coupon_amount.ToString(),
                    origin_price = item.reserve_price.ToString(),
                    coupon_end_time = item.coupon_end_time,
                    coupon_start_time = item.coupon_start_time,
                    detail_images = images,
                    images = images,
                    image = GetImage(item.pict_url),
                    month_sales = item.volume,
                    LevelCommission = GetCommissionRate(decimal.Parse(item.zk_final_price), decimal.Parse(item.commission_rate), dm_UserEntity.IsEmpty() ? 0 : dm_UserEntity.userlevel, dm_BasesettingEntity),
                    SuperCommission = GetCommissionRate(decimal.Parse(item.zk_final_price), decimal.Parse(item.commission_rate), 2, dm_BasesettingEntity),
                    PlaformType = 1,
                    afterServiceScore = GetScore(item.shop_dsr.ToString()),
                    logisticsLvyueScore = GetScore(item.shop_dsr.ToString()),
                    userEvaluateScore = GetScore(item.shop_dsr.ToString()),
                    remark = item.short_title,
                    coupon_link = item.coupon_id,
                    cacheKey = cacheKey
                });
            }

            return commonGoodInfoList;
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
                    string pwdReg = @"[^a-zA-Z=/\d@<\u4E00-\u9FA5]([a-zA-Z0-9]{11})[^a-zA-Z=.\d@>\u4E00-\u9FA5]";
                    pwdContent = getValue(pwdReg, keyWord);
                    if (pwdContent != "")
                    {
                        #region 解析淘口令
                        pwdContent = ParsePwd("http://cloud.jinglm.com/TB_OpenApi/TKL_Query", string.Format("PasswordContent={0}&AdzoneId={1}&SiteId={2}&AccountID={3}", keyWord, "110249350265", "967450311", "127267155"));
                        #endregion
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

                return pwdResult.data.NumIid;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

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
    }

    public class PwdResult
    {
        public int code { get; set; }
        public string info { get; set; }
        public PwdData data { get; set; }
    }

    public class PwdData
    {
        public string ClickUrl { get; set; }
        public string NumIid { get; set; }
        public string OriginPid { get; set; }
        public string OriginUrl { get; set; }
    }
}
