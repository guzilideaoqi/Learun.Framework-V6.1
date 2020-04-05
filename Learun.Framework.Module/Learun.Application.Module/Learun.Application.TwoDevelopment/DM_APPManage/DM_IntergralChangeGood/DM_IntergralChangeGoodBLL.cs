using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_IntergralChangeGoodBLL : DM_IntergralChangeGoodIBLL
	{
		private DM_IntergralChangeGoodService dM_IntergralChangeGoodService = new DM_IntergralChangeGoodService();

		public IEnumerable<dm_intergralchangegoodEntity> GetList(string queryJson)
		{
			try
			{
				return dM_IntergralChangeGoodService.GetList(queryJson);
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

		public IEnumerable<dm_intergralchangegoodEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_IntergralChangeGoodService.GetPageList(pagination, queryJson);
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

		public IEnumerable<dm_intergralchangegoodEntity> GetPageListByCache(Pagination pagination, string appid)
		{
			try
			{
				return dM_IntergralChangeGoodService.GetPageListByCache(pagination, appid);
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

		public dm_intergralchangegoodEntity GetEntity(int keyValue)
		{
			try
			{
				return dM_IntergralChangeGoodService.GetEntity(keyValue);
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
				dM_IntergralChangeGoodService.DeleteEntity(keyValue);
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

		public void SaveEntity(int keyValue, dm_intergralchangegoodEntity entity)
		{
			try
			{
				dM_IntergralChangeGoodService.SaveEntity(keyValue, entity);
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
