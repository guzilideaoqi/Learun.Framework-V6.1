using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_IntergralDetailBLL : DM_IntergralDetailIBLL
	{
		private DM_IntergralDetailService dM_IntergralDetailService = new DM_IntergralDetailService();

		public IEnumerable<dm_intergraldetailEntity> GetList(string queryJson)
		{
			try
			{
				return dM_IntergralDetailService.GetList(queryJson);
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

		public IEnumerable<dm_intergraldetailEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_IntergralDetailService.GetPageList(pagination, queryJson);
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

		public dm_intergraldetailEntity GetEntity(int? keyValue)
		{
			try
			{
				return dM_IntergralDetailService.GetEntity(keyValue);
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
				dM_IntergralDetailService.DeleteEntity(keyValue);
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

		public void SaveEntity(int? keyValue, dm_intergraldetailEntity entity)
		{
			try
			{
				dM_IntergralDetailService.SaveEntity(keyValue, entity);
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
