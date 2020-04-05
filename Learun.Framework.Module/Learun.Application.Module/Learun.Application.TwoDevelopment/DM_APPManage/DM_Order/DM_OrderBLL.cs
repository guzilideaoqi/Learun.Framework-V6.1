using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_OrderBLL : DM_OrderIBLL
	{
		private DM_OrderService dM_OrderService = new DM_OrderService();

		public IEnumerable<dm_orderEntity> GetList(string queryJson)
		{
			try
			{
				return dM_OrderService.GetList(queryJson);
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

		public IEnumerable<dm_orderEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_OrderService.GetPageList(pagination, queryJson);
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

		public dm_orderEntity GetEntity(string keyValue)
		{
			try
			{
				return dM_OrderService.GetEntity(keyValue);
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
				dM_OrderService.DeleteEntity(keyValue);
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

		public void SaveEntity(string keyValue, dm_orderEntity entity)
		{
			try
			{
				dM_OrderService.SaveEntity(keyValue, entity);
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
