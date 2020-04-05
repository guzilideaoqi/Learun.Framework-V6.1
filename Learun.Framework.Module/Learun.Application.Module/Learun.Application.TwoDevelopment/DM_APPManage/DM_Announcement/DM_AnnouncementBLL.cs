using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_AnnouncementBLL : DM_AnnouncementIBLL
	{
		private DM_AnnouncementService dM_AnnouncementService = new DM_AnnouncementService();

		public IEnumerable<dm_announcementEntity> GetList(string queryJson)
		{
			try
			{
				return dM_AnnouncementService.GetList(queryJson);
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

		public IEnumerable<dm_announcementEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_AnnouncementService.GetPageList(pagination, queryJson);
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

		public IEnumerable<dm_announcementEntity> GetPageListByCache(Pagination pagination, string appid)
		{
			try
			{
				return dM_AnnouncementService.GetPageListByCache(pagination, appid);
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

		public dm_announcementEntity GetEntity(int keyValue)
		{
			try
			{
				return dM_AnnouncementService.GetEntity(keyValue);
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
				dM_AnnouncementService.DeleteEntity(keyValue);
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

		public void SaveEntity(int keyValue, dm_announcementEntity entity)
		{
			try
			{
				dM_AnnouncementService.SaveEntity(keyValue, entity);
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
