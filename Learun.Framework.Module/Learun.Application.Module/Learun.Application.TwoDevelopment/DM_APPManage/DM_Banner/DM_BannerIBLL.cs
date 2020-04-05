using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_BannerIBLL
	{
		IEnumerable<dm_bannerEntity> GetList(string queryJson);

		IEnumerable<dm_bannerEntity> GetPageList(Pagination pagination, string queryJson);

		IEnumerable<dm_bannerEntity> GetPageListByCache(Pagination pagination, int type, string appid);

		dm_bannerEntity GetEntity(int keyValue);

		void DeleteEntity(int keyValue);

		void SaveEntity(int keyValue, dm_bannerEntity entity);
	}
}
