using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_BaseSettingBLL : DM_BaseSettingIBLL
	{
		private DM_BaseSettingService dM_BaseSettingService = new DM_BaseSettingService();

		public IEnumerable<dm_basesettingEntity> GetList(string queryJson)
		{
			try
			{
				return dM_BaseSettingService.GetList(queryJson);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public IEnumerable<dm_basesettingEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_BaseSettingService.GetPageList(pagination, queryJson);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public dm_basesettingEntity GetEntity(string keyValue)
		{
			try
			{
				return dM_BaseSettingService.GetEntity(keyValue);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public dm_basesettingEntity GetEntityByCache(string appid)
		{
			try
			{
				return dM_BaseSettingService.GetEntityByCache(appid);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public void DeleteEntity(string keyValue)
		{
			try
			{
				dM_BaseSettingService.DeleteEntity(keyValue);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public void SaveEntity(string keyValue, dm_basesettingEntity entity)
		{
			try
			{
				dM_BaseSettingService.SaveEntity(keyValue, entity);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}
	}
}
