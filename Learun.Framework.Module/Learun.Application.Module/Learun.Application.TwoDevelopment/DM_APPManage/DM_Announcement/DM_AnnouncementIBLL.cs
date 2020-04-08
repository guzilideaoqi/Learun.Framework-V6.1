using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_AnnouncementIBLL
	{
		IEnumerable<dm_announcementEntity> GetList(string queryJson);

		IEnumerable<dm_announcementEntity> GetPageList(Pagination pagination, string queryJson);

		IEnumerable<dm_announcementEntity> GetPageListByCache(Pagination pagination, string appid);

		dm_announcementEntity GetEntity(int keyValue);

		void DeleteEntity(int keyValue);

		void SaveEntity(int keyValue, dm_announcementEntity entity);

    }
}
