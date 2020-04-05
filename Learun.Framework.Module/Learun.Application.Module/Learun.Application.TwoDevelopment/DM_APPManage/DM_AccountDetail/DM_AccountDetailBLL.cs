using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_AccountDetailBLL : DM_AccountDetailIBLL
	{
		private DM_AccountDetailService dM_AccountDetailService = new DM_AccountDetailService();

		public IEnumerable<dm_accountdetailEntity> GetList(string queryJson)
		{
			try
			{
				return dM_AccountDetailService.GetList(queryJson);
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

		public IEnumerable<dm_accountdetailEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_AccountDetailService.GetPageList(pagination, queryJson);
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

		public dm_accountdetailEntity GetEntity(int? keyValue)
		{
			try
			{
				return dM_AccountDetailService.GetEntity(keyValue);
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

		public void DeleteEntity(int? keyValue)
		{
			try
			{
				dM_AccountDetailService.DeleteEntity(keyValue);
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

		public void SaveEntity(int? keyValue, dm_accountdetailEntity entity)
		{
			try
			{
				dM_AccountDetailService.SaveEntity(keyValue, entity);
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
