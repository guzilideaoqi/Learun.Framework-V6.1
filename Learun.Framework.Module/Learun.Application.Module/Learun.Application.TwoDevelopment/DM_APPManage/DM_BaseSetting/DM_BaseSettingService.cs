using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_BaseSettingService : RepositoryFactory
	{
		private ICache redisCache = CacheFactory.CaChe();

		private string fieldSql;

		public DM_BaseSettingService()
		{
			fieldSql = "    t.appid,    t.tb_accountid,    t.tb_appkey,    t.tb_appsecret,    t.tb_sessionkey,    t.tb_authorendtime,    t.jd_accountid,    t.jd_appkey,    t.jd_appsecret,    t.jd_sessionkey,    t.pdd_accountid,    t.pdd_clientid,    t.pdd_clientsecret,    t.openagent_one,    t.openagent_two,    t.openagent_partners,    t.task_do,    t.task_one,    t.task_two,    t.task_one_partners,    t.task_two_partners,    t.shopping_pay_junior,    t.shopping_pay_middle,    t.shopping_pay_senior,    t.shopping_one,    t.shopping_two,    t.shopping_one_partners,    t.shopping_two_partners";
		}

		public IEnumerable<dm_basesettingEntity> GetList(string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_basesetting t ");
				return BaseRepository("dm_data").FindList<dm_basesettingEntity>(strSql.ToString());
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

		public IEnumerable<dm_basesettingEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_basesetting t ");
				return BaseRepository("dm_data").FindList<dm_basesettingEntity>(strSql.ToString(), pagination);
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

		public dm_basesettingEntity GetEntity(string keyValue)
		{
			try
			{
				UserInfo userInfo = LoginUserInfo.Get();
				if (string.IsNullOrEmpty(keyValue))
				{
					keyValue = userInfo.companyId;
				}
				return BaseRepository("dm_data").FindEntity<dm_basesettingEntity>(keyValue);
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

		public dm_basesettingEntity GetEntityByCache(string appid)
		{
			string cacheKey = "DM_BaseSetting" + appid;
			dm_basesettingEntity dm_BasesettingEntity = redisCache.Read<dm_basesettingEntity>(cacheKey, 7L);
			if (dm_BasesettingEntity == null)
			{
				dm_BasesettingEntity = GetEntity(appid);
				redisCache.Write(cacheKey, dm_BasesettingEntity, 0L);
			}
			return dm_BasesettingEntity;
		}

		public void DeleteEntity(string keyValue)
		{
			try
			{
				string cacheKey = "DM_BaseSetting" + keyValue;
				BaseRepository("dm_data").Delete((dm_basesettingEntity t) => t.appid == keyValue);
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

		public void SaveEntity(string keyValue, dm_basesettingEntity entity)
		{
			try
			{
				if (!string.IsNullOrEmpty(keyValue))
				{
					entity.Modify(keyValue);
					BaseRepository("dm_data").Update(entity);
				}
				else
				{
					entity.Create();
					BaseRepository("dm_data").Insert(entity);
				}
				string cacheKey = "DM_BaseSetting" + entity.appid;
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
