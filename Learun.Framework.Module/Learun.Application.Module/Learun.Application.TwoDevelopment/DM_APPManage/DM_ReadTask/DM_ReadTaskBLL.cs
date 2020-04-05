using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_ReadTaskBLL : DM_ReadTaskIBLL
	{
		private DM_ReadTaskService dM_ReadTaskService = new DM_ReadTaskService();

		public IEnumerable<dm_readtaskEntity> GetList(string queryJson)
		{
			try
			{
				return dM_ReadTaskService.GetList(queryJson);
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

		public IEnumerable<dm_readtaskEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_ReadTaskService.GetPageList(pagination, queryJson);
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

		public dm_readtaskEntity GetEntity(int keyValue)
		{
			try
			{
				return dM_ReadTaskService.GetEntity(keyValue);
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

		public IEnumerable<dm_readtaskEntity> GetPageListByCache(Pagination pagination, string appid)
		{
			try
			{
				return dM_ReadTaskService.GetPageListByCache(pagination, appid);
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

		public void DeleteEntity(int keyValue)
		{
			try
			{
				dM_ReadTaskService.DeleteEntity(keyValue);
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

		public void SaveEntity(int keyValue, dm_readtaskEntity entity)
		{
			try
			{
				dM_ReadTaskService.SaveEntity(keyValue, entity);
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

		public void AddClickReadEarnTask(int id, int count, string appid)
		{
			try
			{
				dM_ReadTaskService.AddClickReadEarnTask(id, count, appid);
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
