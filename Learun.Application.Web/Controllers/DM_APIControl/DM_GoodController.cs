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

namespace Learun.Application.Web.Controllers.DM_APIControl
{
	public class DM_GoodController : MvcAPIControllerBase
	{
		private DM_IntergralChangeGoodIBLL dM_IntergralChangeGoodIBLL = new DM_IntergralChangeGoodBLL();

		private DM_IntergralChangeRecordIBLL dM_IntergralChangeRecordIBLL = new DM_IntergralChangeRecordBLL();

		private DM_UserIBLL dm_userIBLL = new DM_UserBLL();

		private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

		private ICache redisCache = CacheFactory.CaChe();

		public ActionResult GetGoodType()
		{
			try
			{
				string appid = CheckAPPID();
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
						return Fail(dTK_Super_CategoryResponse.msg);
					}
					categoryItems = dTK_Super_CategoryResponse.data;
					redisCache.Write(cacheKey, categoryItems, DateTime.Now.AddMonths(1), 7L);
				}
				return SuccessList("获取成功", categoryItems);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

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
				return Fail(ex.Message);
			}
		}

		public ActionResult GetRankingList(int cateid = 1, int RandType = 1)
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "RankingList" + RandType.ToString();
				List<RankingItem> RankingList = redisCache.Read<List<RankingItem>>(cacheKey, 7L);
				if (RankingList == null)
				{
					dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
					DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
					DTK_Ranking_ListRequest dTK_Ranking_ListRequest = new DTK_Ranking_ListRequest();
					dTK_Ranking_ListRequest.version = "v1.1.2";
					dTK_Ranking_ListRequest.rankType = RandType;
					dTK_Ranking_ListRequest.cid = cateid;
					DTK_Ranking_ListResponse dTK_Ranking_ListResponse = dTK_ApiManage.GetRankingList(dTK_Ranking_ListRequest);
					if (dTK_Ranking_ListResponse.code != 0)
					{
						return Fail(dTK_Ranking_ListResponse.msg);
					}
					RankingList = dTK_Ranking_ListResponse.data;
					redisCache.Write(cacheKey, RankingList, DateTime.Now.AddHours(2.0), 7L);
				}
				return SuccessList("获取成功!", RankingList);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

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
						SmallType = 6
					});
					redisCache.Write(cacheKey, smallCateList, 7L);
				}
				return SuccessList("获取成功!", smallCateList);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

		public ActionResult GetTodayGood()
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "TodayGood";
				List<RankingItem> RankingList = redisCache.Read<List<RankingItem>>(cacheKey, 7L);
				if (RankingList == null)
				{
					dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
					DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
					DTK_Ranking_ListRequest dTK_Ranking_ListRequest = new DTK_Ranking_ListRequest();
					dTK_Ranking_ListRequest.version = "v1.1.2";
					dTK_Ranking_ListRequest.rankType = 7;
					DTK_Ranking_ListResponse dTK_Ranking_ListResponse = dTK_ApiManage.GetRankingList(dTK_Ranking_ListRequest);
					if (dTK_Ranking_ListResponse.code != 0)
					{
						return Fail(dTK_Ranking_ListResponse.msg);
					}
					RankingList = dTK_Ranking_ListResponse.data;
					redisCache.Write(cacheKey, RankingList, DateTime.Now.AddHours(2.0), 7L);
				}
				return SuccessList("获取成功!", RankingList);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

		public ActionResult GetSuperSerachGood(int type = 0, int pageId = 1, int pageSize = 20, string keyWords = "", int tmall = 0, int haitao = 0, string sort = "total_sales_des")
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "SuperSerachGood";
				List<SuperGoodItem> superGoodItems = redisCache.Read<List<SuperGoodItem>>(cacheKey, 7L);
				if (superGoodItems == null)
				{
					dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
					DTK_ApiManage dTK_ApiManage = new DTK_ApiManage(dm_BasesettingEntity.dtk_appkey, dm_BasesettingEntity.dtk_appsecret);
					DTK_Super_GoodRequest dTK_Super_GoodRequest = new DTK_Super_GoodRequest();
					dTK_Super_GoodRequest.version = "v1.2.1";
					dTK_Super_GoodRequest.type = type;
					dTK_Super_GoodRequest.pageId = pageId;
					dTK_Super_GoodRequest.pageSize = pageSize;
					dTK_Super_GoodRequest.keyWords = keyWords;
					dTK_Super_GoodRequest.tmall = tmall;
					dTK_Super_GoodRequest.haitao = haitao;
					dTK_Super_GoodRequest.sort = sort;
					DTK_Super_GoodResponse dTK_Super_GoodResponse = dTK_ApiManage.GetSuperGood(dTK_Super_GoodRequest);
					if (dTK_Super_GoodResponse.code != 0)
					{
						return Fail(dTK_Super_GoodResponse.msg);
					}
					superGoodItems = dTK_Super_GoodResponse.data.list;
					redisCache.Write(cacheKey, superGoodItems, DateTime.Now.AddHours(2.0), 7L);
				}
				return SuccessList("获取成功!", superGoodItems);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

		public ActionResult GetOPGood(string pageId = "1", int pageSize = 20, string nineCid = "1")
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "OPGood";
				List<OPGoodItem> oPGoodItems = redisCache.Read<List<OPGoodItem>>(cacheKey, 7L);
				if (oPGoodItems == null)
				{
					dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
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
					oPGoodItems = dTK_OP_ListResponse.data.list;
					redisCache.Write(cacheKey, oPGoodItems, DateTime.Now.AddHours(2.0), 7L);
				}
				return SuccessList("获取成功!", oPGoodItems);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

		public ActionResult SearchSuggestion(string keyWords, int type = 3)
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "Suggestion" + keyWords;
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
				return Fail(ex.Message);
			}
		}

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
				return Fail(ex.Message);
			}
		}

		public ActionResult GetActivityGoodList(int activityId, string pageId = "1", int pageSize = 20, int cid = 1)
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "ActivityGoodList";
				DTK_Activity_GoodListResult activityGoodItems = redisCache.Read<DTK_Activity_GoodListResult>(cacheKey, 7L);
				if (activityGoodItems == null)
				{
					dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
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
					activityGoodItems = dTK_Activity_GoodListResponse.data;
					redisCache.Write(cacheKey, activityGoodItems, DateTime.Now.AddHours(2.0), 7L);
				}
				return SuccessList("获取成功!", activityGoodItems);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

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
					redisCache.Write(cacheKey, topicCatalogueItems, DateTime.Now.AddDays(1.0), 7L);
				}
				return SuccessList("获取成功!", topicCatalogueItems);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

		public ActionResult GetTopicGoodList(int topicId, string pageId = "1", int pageSize = 20)
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "TopicGoodList";
				List<TopicGoodItem> topicGoodItems = redisCache.Read<List<TopicGoodItem>>(cacheKey, 7L);
				if (topicGoodItems == null)
				{
					dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
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
					topicGoodItems = dTK_Topic_GoodListResponse.data.list;
					redisCache.Write(cacheKey, topicGoodItems, DateTime.Now.AddHours(2.0), 7L);
				}
				return SuccessList("获取成功!", topicGoodItems);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

		public ActionResult GetTBTopicList(int userid, int type = 0, string pageId = "1", int pageSize = 20)
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = "TBTopicList";
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
				return Fail(ex.Message);
			}
		}

		public ActionResult Get_TB_GoodList()
		{
			return View();
		}

		public ActionResult ConvertLinkByTB(int userid, string originid, string couponid)
		{
			try
			{
				string appid = CheckAPPID();
				string cacheKey = Md5Helper.Hash(userid.ToString() + originid + couponid);
				PrivilegeLinkResult ConvertLinkResult = redisCache.Read<PrivilegeLinkResult>(cacheKey, 7L);
				if (ConvertLinkResult == null)
				{
					dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(userid);
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
					redisCache.Write(cacheKey, ConvertLinkResult, DateTime.Now.AddHours(1.0), 7L);
				}
				return Success("转链成功!", ConvertLinkResult);
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}

		public ActionResult Get_TB_Order()
		{
			return View();
		}

		public ActionResult Get_JD_GoodList()
		{
			return View();
		}

		public ActionResult ConvertLinkByJD()
		{
			return View();
		}

		public ActionResult Get_JD_Order()
		{
			return View();
		}

		public ActionResult Get_PDD_GoodList()
		{
			return View();
		}

		public ActionResult ConvertLinkByPDD()
		{
			return View();
		}

		public ActionResult Get_PDD_Order()
		{
			return View();
		}

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
				return Fail(ex.Message);
			}
		}

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
				return Fail(ex.InnerException.Message);
			}
		}

		public string CheckAPPID()
		{
			if (base.Request.Headers["appid"].IsEmpty())
			{
				throw new Exception("缺少参数appid");
			}
			return base.Request.Headers["appid"].ToString();
		}
	}
}
