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
	public class DM_IntergralChangeGoodService : RepositoryFactory
	{
		private ICache redisCache = CacheFactory.CaChe();

		private string fieldSql;

		public DM_IntergralChangeGoodService()
		{
			fieldSql = "    t.id,    t.goodtitle,    t.goodremark,    t.needintergral,    t.goodimage,    t.goodprice,    t.createtime,    t.createcode,    t.isexpress,    t.expressprice,    t.appid";
		}

		public IEnumerable<dm_intergralchangegoodEntity> GetList(string queryJson)
		{
			try
			{
				JObject queryParam = queryJson.ToJObject();
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_intergralchangegood t ");
				if (!queryParam["appid"].IsEmpty())
				{
					strSql.Append(" where t.appid='" + queryParam["appid"].ToString() + "'");
				}
				return BaseRepository("dm_data").FindList<dm_intergralchangegoodEntity>(strSql.ToString());
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

		public IEnumerable<dm_intergralchangegoodEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				JObject param = queryJson.ToJObject();
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_intergralchangegood t ");
				if (!param["keyword"].IsEmpty())
				{
					strSql.Append(" where goodtitle like '%" + param["keyword"].ToString() + "%'");
				}
				return BaseRepository("dm_data").FindList<dm_intergralchangegoodEntity>(strSql.ToString(), pagination);
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

		public IEnumerable<dm_intergralchangegoodEntity> GetPageListByCache(Pagination pagination, string appid)
		{
			string cacheKey = "IntergralChangeGood" + appid;
			IEnumerable<dm_intergralchangegoodEntity> dm_IntergralChangeGoodEntities = redisCache.Read<IEnumerable<dm_intergralchangegoodEntity>>(cacheKey, 7L);
			if (dm_IntergralChangeGoodEntities != null)
			{
				return dm_IntergralChangeGoodEntities.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
			}
			IEnumerable<dm_intergralchangegoodEntity> AllIntergralChangeGoodEntityList = GetList("{\"appid\":\"" + appid + "\"}").AsList();
			redisCache.Write(cacheKey, AllIntergralChangeGoodEntityList, 7L);
			return AllIntergralChangeGoodEntityList.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows);
		}

		public dm_intergralchangegoodEntity GetEntity(int keyValue)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity<dm_intergralchangegoodEntity>(keyValue);
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
				BaseRepository("dm_data").Delete((dm_intergralchangegoodEntity t) => t.id == (int?)keyValue);
				UserInfo userInfo = LoginUserInfo.Get();
				string cacheKey = "IntergralChangeGood" + userInfo.companyId;
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

		public void SaveEntity(int keyValue, dm_intergralchangegoodEntity entity)
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
				string cacheKey = "IntergralChangeGood" + userInfo.companyId;
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
