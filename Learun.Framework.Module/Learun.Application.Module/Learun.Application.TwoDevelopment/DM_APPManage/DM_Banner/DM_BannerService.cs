using Dapper;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.DataBase.Repository;
using Learun.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_BannerService : RepositoryFactory
	{
		private ICache redisCache = CacheFactory.CaChe();

		private DM_BaseSettingService dM_BaseSettingService = new DM_BaseSettingService();

		private string fieldSql;

		public DM_BannerService()
		{
			fieldSql = "    t.id,    t.b_type,    t.b_title,    t.b_image,    t.b_param,    t.appid,    t.createtime,    t.createcode";
		}

		public IEnumerable<dm_bannerEntity> GetList(string queryJson)
		{
			try
			{
				JObject queryParam = queryJson.ToJObject();
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_banner t ");
				if (!queryParam["appid"].IsEmpty())
				{
					strSql.Append(" where t.appid='" + queryParam["appid"].ToString() + "'");
				}
				return BaseRepository("dm_data").FindList<dm_bannerEntity>(strSql.ToString());
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public IEnumerable<dm_bannerEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_banner t ");
				return BaseRepository("dm_data").FindList<dm_bannerEntity>(strSql.ToString(), pagination);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public IEnumerable<dm_bannerEntity> GetPageListByCache(Pagination pagination, int type, string appid)
		{
			dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingService.GetEntityByCache(appid);
			string cacheKey = "Banner" + appid;
			IEnumerable<dm_bannerEntity> dm_BannerEntities2 = redisCache.Read<IEnumerable<dm_bannerEntity>>(cacheKey, 7L);
			if (dm_BannerEntities2 != null)
			{
				dm_BannerEntities2 = dm_BannerEntities2.Where((dm_bannerEntity t) => t.b_type == type).Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
			}
			else
			{
				IEnumerable<dm_bannerEntity> AllBannerEntityList = GetList("{\"appid\":\"" + appid + "\"}").AsList();
				redisCache.Write(cacheKey, AllBannerEntityList, 7L);
				dm_BannerEntities2 = AllBannerEntityList.Where((dm_bannerEntity t) => t.b_type == type).Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
			}
			return dm_BannerEntities2.Select(delegate(dm_bannerEntity t)
			{
				t.b_image = dm_BasesettingEntity.qianzhui_image + t.b_image;
				return t;
			});
		}

		public dm_bannerEntity GetEntity(int keyValue)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity<dm_bannerEntity>(keyValue);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public void DeleteEntity(int keyValue)
		{
			try
			{
				BaseRepository("dm_data").Delete((dm_bannerEntity t) => t.id == (int?)keyValue);
				UserInfo userInfo = LoginUserInfo.Get();
				string cacheKey = "Banner" + userInfo.companyId;
				redisCache.Remove(cacheKey, 7L);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public void SaveEntity(int keyValue, dm_bannerEntity entity)
		{
			try
			{
				if (keyValue > 0)
				{
					entity.Modify(keyValue);
					BaseRepository("dm_data").Update(entity);
				}
				else
				{
					entity.Create();
					BaseRepository("dm_data").Insert(entity);
				}
				UserInfo userInfo = LoginUserInfo.Get();
				string cacheKey = "Banner" + userInfo.companyId;
				redisCache.Remove(cacheKey, 7L);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}
	}
}
