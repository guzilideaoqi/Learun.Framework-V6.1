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
	public class DM_AnnouncementService : RepositoryFactory
	{
		private ICache redisCache = CacheFactory.CaChe();

		private string fieldSql;

		public DM_AnnouncementService()
		{
			fieldSql = "    t.id,    t.a_title,    t.a_content,    t.a_image,    t.appid,    t.createtime,    t.createcode";
		}

		public IEnumerable<dm_announcementEntity> GetList(string queryJson)
		{
			try
			{
				JObject queryParam = queryJson.ToJObject();
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_announcement t ");
				if (!queryParam["appid"].IsEmpty())
				{
					strSql.Append(" where t.appid='" + queryParam["appid"].ToString() + "'");
				}
				return BaseRepository("dm_data").FindList<dm_announcementEntity>(strSql.ToString());
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

		public IEnumerable<dm_announcementEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_announcement t ");
				return BaseRepository("dm_data").FindList<dm_announcementEntity>(strSql.ToString(), pagination);
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

		public IEnumerable<dm_announcementEntity> GetPageListByCache(Pagination pagination, string appid)
		{
			string cacheKey = "Announcement" + appid;
			IEnumerable<dm_announcementEntity> dm_AnnounceEntities = redisCache.Read<IEnumerable<dm_announcementEntity>>(cacheKey, 7L);
			if (dm_AnnounceEntities != null)
			{
				return dm_AnnounceEntities.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
			}
			IEnumerable<dm_announcementEntity> AllAnnounceEntityList = GetList("{\"appid\":\"" + appid + "\"}").AsList();
			redisCache.Write(cacheKey, AllAnnounceEntityList, 7L);
			return AllAnnounceEntityList.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
		}

		public dm_announcementEntity GetEntity(int keyValue)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity<dm_announcementEntity>(keyValue);
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
				BaseRepository("dm_data").Delete((dm_announcementEntity t) => t.id == (int?)keyValue);
				UserInfo userInfo = LoginUserInfo.Get();
				string cacheKey = "Announcement" + userInfo.companyId;
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

		public void SaveEntity(int keyValue, dm_announcementEntity entity)
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
				string cacheKey = "Announcement" + userInfo.companyId;
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
