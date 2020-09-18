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
	public class DM_ReadTaskService : RepositoryFactory
	{
		private ICache redisCache = CacheFactory.CaChe();

		private string fieldSql;

		public DM_ReadTaskService()
		{
			fieldSql = "    t.id,    t.tasktitle, t.ischeckmode,   t.taskremark,    t.taskurl,    t.taskimage,    t.peoplecount,    t.createtime,    t.createcode,    t.updatetime,    t.appid";
		}

		public IEnumerable<dm_readtaskEntity> GetList(string queryJson)
		{
			try
			{
				JObject queryParam = queryJson.ToJObject();
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_readtask t ");
				if (!queryParam["appid"].IsEmpty())
				{
					strSql.Append(" where t.appid='" + queryParam["appid"].ToString() + "'");
				}
				return BaseRepository("dm_data").FindList<dm_readtaskEntity>(strSql.ToString());
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

		public IEnumerable<dm_readtaskEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				JObject param = queryJson.ToJObject();
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_readtask t ");
				if (!param["keyword"].IsEmpty())
				{
					strSql.Append(" where tasktitle like '%" + param["keyword"].ToString() + "%'");
				}
				return BaseRepository("dm_data").FindList<dm_readtaskEntity>(strSql.ToString(), pagination);
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

		public IEnumerable<dm_readtaskEntity> GetPageListByCache(Pagination pagination, string appid)
		{
			string cacheKey = "ReadEarn" + appid;
			IEnumerable<dm_readtaskEntity> dm_ReadtaskEntities = redisCache.Read<IEnumerable<dm_readtaskEntity>>(cacheKey, 7L);
			if (dm_ReadtaskEntities != null)
			{
				return dm_ReadtaskEntities.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
			}
			IEnumerable<dm_readtaskEntity> AllReadTaskEntityList = GetList("{\"appid\":\"" + appid + "\"}").AsList();
			redisCache.Write(cacheKey, AllReadTaskEntityList, 7L);
			return AllReadTaskEntityList.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
		}

		public dm_readtaskEntity GetEntity(int keyValue)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity<dm_readtaskEntity>(keyValue);
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
				BaseRepository("dm_data").Delete((dm_readtaskEntity t) => t.id == (int?)keyValue);
				UserInfo userInfo = LoginUserInfo.Get();
				string cacheKey = "ReadEarn" + userInfo.companyId;
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

		public void SaveEntity(int keyValue, dm_readtaskEntity entity)
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
				string cacheKey = "ReadEarn" + userInfo.companyId;
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

		public void AddClickReadEarnTask(int id, int count, string appid)
		{
			try
			{
				dm_readtaskEntity entity = GetEntity(id);
				if (entity != null)
				{
					entity.peoplecount += count;
				}
				entity.Modify(id);
				BaseRepository("dm_data").Update(entity);
				string cacheKey = "ReadEarn" + appid;
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
